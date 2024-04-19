using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter.Xml;
using SportsExerciseBattle.DataAccessLayer;
using SportsExerciseBattle.Services;
using SportsExerciseBattle.Models;
using SportsExerciseBattle.Web.HTTP;
using HttpMethod = SportsExerciseBattle.Web.HTTP.HttpMethod; // makes sure the correct HttpMethod is used

namespace SportsExerciseBattle.Web.Endpoints
{
    public class HistoryEndpoint : IHttpEndpoint
    {
        private HistoryDAO historyDAO = new HistoryDAO();

        public bool HandleRequest(HttpRequest rq, HttpResponse rs)
        {
            switch (rq.Method)
            {
                case HttpMethod.POST:
                    return AddEntry(rq, rs);
                case HttpMethod.GET:
                    return GetEntries(rq, rs);
                default:
                    return false;
            }
        }

        private bool AddEntry(HttpRequest rq, HttpResponse rs)
        {
            if (!TryAuthorize(rq, rs, out string username))
                return false;

            try
            {
                var entry = JsonSerializer.Deserialize<Entry>(rq.Content ?? "{}");
                if (entry == null)
                {
                    rs.ResponseCode = 400;
                    rs.Content = "Invalid request data";
                    return false;
                }

                historyDAO.AddEntry(username, entry, rs); // Provide HttpResponse parameter
                TournamentHelper.ManageTournament();
                rs.SetSuccess("Entry added successfully", 201); // Provide code
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
                rs.SetServerError(ex.Message); // Pass exception message
                return false;
            }
        }


        private bool GetEntries(HttpRequest rq, HttpResponse rs)
        {
            if (!TryAuthorize(rq, rs, out string username))
                return false;

            var entries = historyDAO.GetEntries(username);
            if (entries == null || entries.Count == 0)
            {
                rs.ResponseCode = 404;
                rs.Content = "No entries found";
                return false;
            }

            rs.Content = JsonSerializer.Serialize(entries);
            rs.SetJsonContentType();
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
            username = token.Split("-")[0];

            if (!TokenService.ValidateToken(token, username))
            {
                rs.ResponseCode = 401;
                rs.Content = "Unauthorized";
                return false;
            }

            return true;
        }
    }
}
