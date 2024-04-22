using SportsExerciseBattle.Web.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HttpMethod = SportsExerciseBattle.Web.HTTP.HttpMethod; // makes sure the correct HttpMethod is used
using SportsExerciseBattle.Models;
using SportsExerciseBattle.DataAccessLayer;
using SportsExerciseBattle.Utilities;

namespace SportsExerciseBattle.Web.Endpoints
{
    public class TournamentStatsEndpoint : IHttpEndpoint
    {
        private ITournamentRepository _tournamentRepository = new TournamentRepository();

        public bool HandleRequest(HttpRequest rq, HttpResponse rs)
        {
            if (rq.Method == HttpMethod.GET)
            {
                return GetTournamentStats(rq, rs);
            }
            return false;
        }

        private bool GetTournamentStats(HttpRequest rq, HttpResponse rs)
        {
            if (!TryAuthorize(rq, rs, out string username))
            {
                return false;
            }

            try
            {
                var task = Task.Run(async () => await _tournamentRepository.GetTournamentResults(1)); // Assuming a tournament ID
                var stats = task.Result; // This blocks the current thread until the task is complete
                if (stats == null)
                {
                    rs.ResponseCode = 404;
                    rs.Content = "Tournament stats not found.";
                    return false;
                }

                rs.Content = JsonSerializer.Serialize(stats);
                rs.SetJsonContentType();
                rs.ResponseCode = 200;
                return true;
            }
            catch (Exception ex)
            {
                rs.SetServerError(ex.Message);
                return false;
            }
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
            username = token.Split('-')[0];

            return TokenService.ValidateToken(token, username);
        }
    }
}
