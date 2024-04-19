using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter.Xml;
using SportsExerciseBattle.DataAccessLayer;
using SportsExerciseBattle.Services;
using SportsExerciseBattle.Web.HTTP;
using HttpMethod = SportsExerciseBattle.Web.HTTP.HttpMethod; // makes sure the correct HttpMethod is used

namespace SportsExerciseBattle.Web.Endpoints
{
    public class HistoryEndpoint : IHttpEndpoint
    {
        private readonly HistoryRepository _historyRepository = new HistoryRepository();
        private readonly TournamentService _tournamentService = new TournamentService();

        public HistoryEndpoint()
        {
        }

        public bool HandleRequest(HttpRequest request, HttpResponse response)
        {
            try
            {
                switch (request.Method)
                {
                    case HttpMethod.POST:
                        return HandlePost(request, response).Result;
                    case HttpMethod.GET:
                        return HandleGet(request, response).Result;
                    default:
                        response.ResponseCode = 405;  // Method Not Allowed
                        return false;
                }
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500; // Internal Server Error
                response.ResponseMessage = $"Internal server error: {ex.Message}";
                return false;
            }
        }

        private async Task<bool> HandlePost(HttpRequest request, HttpResponse response)
        {
            try
            {
                var entry = JsonSerializer.Deserialize<Entry>(request.Content ?? string.Empty);
                if (entry == null)
                {
                    response.ResponseCode = 400; // Bad Request
                    response.ResponseMessage = "Invalid entry data";
                    return false;
                }

                // Assume ValidateToken checks and decodes the token into a username
                var username = TokenService.ValidateRequestAndGetUsername(request);
                if (username == null)
                {
                    response.ResponseCode = 401; // Unauthorized
                    return false;
                }

                await _historyRepository.AddEntry(username, entry);
                response.ResponseCode = 201; // Created
                response.ResponseMessage = "Entry added";
                return true;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500; // Internal Server Error
                response.ResponseMessage = $"Error processing request: {ex.Message}";
                return false;
            }
        }

        private async Task<bool> HandleGet(HttpRequest request, HttpResponse response)
        {
            try
            {
                var username = TokenService.ValidateRequestAndGetUsername(request);
                if (username == null)
                {
                    response.ResponseCode = 401; // Unauthorized
                    return false;
                }

                var entries = await _historyRepository.GetEntriesByUsername(username);
                if (!entries.Any())
                {
                    response.ResponseCode = 404; // Not Found
                    response.ResponseMessage = "No history entries found";
                    return false;
                }

                response.Content = JsonSerializer.Serialize(entries);
                response.Headers.Add("Content-Type", "application/json");
                response.ResponseCode = 200; // OK
                response.ResponseMessage = "OK";
                return true;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500; // Internal Server Error
                response.ResponseMessage = $"Error retrieving history: {ex.Message}";
                return false;
            }
        }
    }
}
