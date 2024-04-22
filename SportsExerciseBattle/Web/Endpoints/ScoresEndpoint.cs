using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SportsExerciseBattle.DataAccessLayer;
using SportsExerciseBattle.Utilities;
using SportsExerciseBattle.Web.HTTP;
using HttpMethod = SportsExerciseBattle.Web.HTTP.HttpMethod; // makes sure the correct HttpMethod is used

namespace SportsExerciseBattle.Web.Endpoints
{
    public class ScoresEndpoint : IHttpEndpoint
    {
        private readonly IScoresRepository _scoresRepository;

        public ScoresEndpoint()
        {
            _scoresRepository = new ScoresRepository();
        }

        public bool HandleRequest(HttpRequest rq, HttpResponse rs)
        {
            if (rq.Method != HttpMethod.GET)
            {
                return false;
            }

            // Handle the asynchronous operation within the synchronous method
            return GetScores(rq, rs).GetAwaiter().GetResult();
        }

        private async Task<bool> GetScores(HttpRequest rq, HttpResponse rs)
        {
            if (!TryAuthorize(rq, rs, out string username))
            {
                return false;
            }

            try
            {
                var scores = await _scoresRepository.GetScoresAsync();
                if (scores == null || !scores.Any())
                {
                    rs.ResponseCode = 404;
                    rs.Content = "Scores not found";
                    return false;
                }

                rs.Content = JsonSerializer.Serialize(scores);
                rs.SetJsonContentType();
                rs.ResponseCode = 200;
                return true;
            }
            catch (Exception ex)
            {
                rs.SetServerError(ex.Message); // Pass the exception message
                return false;
            }
        }

        private bool TryAuthorize(HttpRequest rq, HttpResponse rs, out string username)
        {
            username = "";
            if (!rq.Headers.TryGetValue("Authorization", out string? authHeader) || !authHeader.StartsWith("Basic "))
            {
                rs.ResponseCode = 401;
                rs.Content = "Unauthorized";
                return false;
            }

            var token = authHeader.Substring("Basic ".Length);
            username = token.Split('-')[0];

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
