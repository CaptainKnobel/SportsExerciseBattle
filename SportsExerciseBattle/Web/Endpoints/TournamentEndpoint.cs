using SportsExerciseBattle.Models;
using SportsExerciseBattle.Web.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HttpMethod = SportsExerciseBattle.Web.HTTP.HttpMethod;
using SportsExerciseBattle.DataAccessLayer;
using SportsExerciseBattle.BusinessLayer;
using SportsExerciseBattle.Utilities;

namespace SportsExerciseBattle.Web.Endpoints
{
    public class TournamentEndpoint : IHttpEndpoint
    {
        private ITournamentRepository tournamentRepository;
        private TournamentManager tournamentManager = new TournamentManager();

        public TournamentEndpoint()
        {
            this.tournamentRepository = new TournamentRepository();
        }

        public bool HandleRequest(HttpRequest rq, HttpResponse rs)
        {
            switch (rq.Method)
            {
                case HttpMethod.GET:
                    return GetTournamentInfo(rq, rs).GetAwaiter().GetResult();
                default:
                    return false;
            }
        }

        private async Task<bool> GetTournamentInfo(HttpRequest rq, HttpResponse rs)
        {
            try
            {
                var tournament = await tournamentRepository.GetCurrentTournamentAsync();
                if (tournament == null || !tournament.IsRunning)
                {
                    rs.ResponseCode = 404;
                    rs.Content = "No tournament currently running.";
                    return false;
                }

                rs.Content = JsonSerializer.Serialize(tournament);
                rs.SetJsonContentType();
                rs.ResponseCode = 200;
                return true;
            }
            catch (Exception ex)
            {
                rs.SetServerError(ex.Message); // Handle error properly
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
            username = TokenService.ValidateTokenAndGetUsername(token);
            if (string.IsNullOrEmpty(username))
            {
                rs.ResponseCode = 401;
                rs.Content = "Unauthorized: Invalid token.";
                return false;
            }

            return true;
        }
    }
}