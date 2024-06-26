﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SportsExerciseBattle.DataAccessLayer;
using SportsExerciseBattle.Services;
using SportsExerciseBattle.Web.HTTP;
using HttpMethod = SportsExerciseBattle.Web.HTTP.HttpMethod; // makes sure the correct HttpMethod is used

namespace SportsExerciseBattle.Web.Endpoints
{
    public class ScoresEndpoint : IHttpEndpoint
    {
        private ScoresDAO scoresDAO = new ScoresDAO();

        public bool HandleRequest(HttpRequest rq, HttpResponse rs)
        {
            if (rq.Method == HttpMethod.GET)
            {
                return GetScores(rq, rs);
            }
            return false;
        }

        private bool GetScores(HttpRequest rq, HttpResponse rs)
        {
            if (!TryAuthorize(rq, rs, out string username))
                return false;

            try
            {
                var scores = scoresDAO.GetScores();
                if (scores == null || scores.Count == 0)
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
