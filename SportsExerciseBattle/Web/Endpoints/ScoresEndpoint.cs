using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SportsExerciseBattle.DataAccessLayer;
using SportsExerciseBattle.Web.HTTP;
using HttpMethod = SportsExerciseBattle.Web.HTTP.HttpMethod; // makes sure the correct HttpMethod is used

namespace SportsExerciseBattle.Web.Endpoints
{
    public class ScoresEndpoint : IHttpEndpoint
    {
        private readonly UserRepository _userRepository = new UserRepository();

        public ScoresEndpoint()
        {
        }

        public bool HandleRequest(HttpRequest request, HttpResponse response)
        {
            try
            {
                if (request.Method == HttpMethod.GET)
                {
                    return HandleGet(request, response).Result;
                }
                response.ResponseCode = 405; // Method Not Allowed
                return false;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500; // Internal Server Error
                response.ResponseMessage = $"Internal server error: {ex.Message}";
                return false;
            }
        }

        private async Task<bool> HandleGet(HttpRequest request, HttpResponse response)
        {
            try
            {
                var scores = await _userRepository.GetScoreboardData();
                if (scores == null || scores.Count == 0)
                {
                    response.ResponseCode = 404; // Not Found
                    response.ResponseMessage = "Scores not found";
                    return false;
                }

                response.ResponseCode = 200; // OK
                response.ResponseMessage = "OK";
                response.Content = JsonSerializer.Serialize(scores);
                response.Headers.Add("Content-Type", "application/json");
                return true;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500; // Internal Server Error
                response.ResponseMessage = $"Error processing request: {ex.Message}";
                return false;
            }
        }
    }
}
