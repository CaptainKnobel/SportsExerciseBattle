using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter.Xml;
using SportsExerciseBattle.Models;
using SportsExerciseBattle.Web.HTTP;
using HttpMethod = SportsExerciseBattle.Web.HTTP.HttpMethod;
using SportsExerciseBattle.DataAccessLayer; // makes sure the correct HttpMethod is used
using SportsExerciseBattle.Utilities;

namespace SportsExerciseBattle.Web.Endpoints
{
    public class HistoryEndpoint : IHttpEndpoint
    {
        private IHistoryRepository _historyRepository;
        public HistoryEndpoint()
        {
            _historyRepository = new HistoryRepository();
        }

        public bool HandleRequest(HttpRequest rq, HttpResponse rs)
        {
            switch (rq.Method)
            {
                case HttpMethod.POST:
                    return AddEntry(rq, rs).GetAwaiter().GetResult();
                case HttpMethod.GET:
                    return GetEntries(rq, rs).GetAwaiter().GetResult();
                default:
                    return false;
            }
        }

        private async Task<bool> AddEntry(HttpRequest rq, HttpResponse rs)
        {
            if (!TryAuthorize(rq, rs, out string username))
                return false;

            try
            {
                var entry = JsonSerializer.Deserialize<HistoryEntry>(rq.Content ?? "{}");
                if (entry == null)
                {
                    rs.ResponseCode = 400;
                    rs.Content = "Invalid request data";
                    return false;
                }

                await _historyRepository.AddEntry(username, entry);
                rs.ResponseCode = 201; // HTTP created
                rs.Content = "Entry added successfully";
                return true;
            }
            catch (JsonException)
            {
                rs.ResponseCode = 400;
                rs.Content = "Failed to parse entry data";
                return false;
            }
            catch (Exception ex)
            {
                rs.ResponseCode = 500; // Internal server error
                rs.Content = ex.Message;
                return false;
            }
        }

        private async Task<bool> GetEntries(HttpRequest rq, HttpResponse rs)
        {
            if (!TryAuthorize(rq, rs, out string username))
                return false;

            var entries = await _historyRepository.GetEntries(username);
            if (entries == null || entries.Count == 0)
            {
                rs.ResponseCode = 404;
                rs.Content = "No entries found";
                return false;
            }

            rs.Content = JsonSerializer.Serialize(entries);
            rs.SetJsonContentType(); // Sets Content-Type to application/json
            rs.ResponseCode = 200;
            return true;
        }

        private bool TryAuthorize(HttpRequest rq, HttpResponse rs, out string username)
        {
            username = "";
            if (!rq.Headers.TryGetValue("Authorization", out string authHeader) || !authHeader.StartsWith("Basic "))
            {
                rs.ResponseCode = 401;
                rs.Content = "Unauthorized";
                return false;
            }

            var token = authHeader.Substring("Basic ".Length);
            username = token.Split(':')[0];  // Simplified username extraction
            return TokenService.ValidateToken(token, username); // Simplified validation
        }
    }
}
