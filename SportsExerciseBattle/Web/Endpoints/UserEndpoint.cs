using SportsExerciseBattle.Models;
using SportsExerciseBattle.Services;
using SportsExerciseBattle.Web.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HttpMethod = SportsExerciseBattle.Web.HTTP.HttpMethod;
using SportsExerciseBattle.DataAccessLayer.DAO; // makes sure the correct HttpMethod is used

namespace SportsExerciseBattle.Web.Endpoints
{
    public class UsersEndpoint : IHttpEndpoint
    {
        private UserDAO userDAO = new UserDAO();

        public bool HandleRequest(HttpRequest rq, HttpResponse rs)
        {
            switch (rq.Method)
            {
                case HttpMethod.POST:
                    return CreateUser(rq, rs);
                case HttpMethod.GET:
                    return GetUserData(rq, rs);
                case HttpMethod.PUT:
                    return UpdateUser(rq, rs);
                default:
                    return false;
            }
        }

        private bool CreateUser(HttpRequest rq, HttpResponse rs)
        {
            try
            {
                var user = JsonSerializer.Deserialize<User>(rq.Content ?? "{}");
                if (user == null)
                {
                    rs.ResponseCode = 400;
                    rs.Content = "Invalid user data provided.";
                    return false;
                }

                userDAO.CreateUser(rq, rs, user); // Pass HttpRequest and HttpResponse to the CreateUser method
                return true;
            }
            catch (JsonException)
            {
                rs.ResponseCode = 400;
                rs.Content = "Failed to parse user data.";
                return false;
            }
        }

        private bool GetUserData(HttpRequest rq, HttpResponse rs)
        {
            var username = rq.Path.LastOrDefault();
            if (!TryAuthorize(rq, rs, username))
                return false;

            try
            {
                var user = userDAO.GetUserByUsername(username);
                if (user == null)
                {
                    rs.ResponseCode = 404;
                    rs.Content = "User not found.";
                    return false;
                }

                rs.Content = JsonSerializer.Serialize(user);
                rs.SetJsonContentType();
                rs.ResponseCode = 200;
                return true;
            }
            catch (Exception ex)
            {
                rs.SetServerError(ex.Message); // Set the error message as the content
                return false;
            }
        }

        private bool UpdateUser(HttpRequest rq, HttpResponse rs)
        {
            var username = rq.Path.LastOrDefault();
            if (!TryAuthorize(rq, rs, username))
                return false;

            try
            {
                var user = JsonSerializer.Deserialize<User>(rq.Content ?? "{}");
                if (user == null)
                {
                    rs.ResponseCode = 400;
                    rs.Content = "Invalid update data provided.";
                    return false;
                }

                userDAO.UpdateUser(rq, rs, user, username); // Pass HttpRequest and HttpResponse to the UpdateUser method
                return true;
            }
            catch (JsonException)
            {
                rs.ResponseCode = 400;
                rs.Content = "Failed to parse update data.";
                return false;
            }
            catch (Exception ex)
            {
                rs.SetServerError(ex.Message); // Set the error message as the content
                return false;
            }
        }

        private bool TryAuthorize(HttpRequest rq, HttpResponse rs, string username)
        {
            if (!rq.Headers.TryGetValue("Authorization", out string authHeader) || !authHeader.StartsWith("Basic "))
            {
                rs.ResponseCode = 401;
                rs.Content = "Unauthorized: Authentication required.";
                return false;
            }

            var token = authHeader.Substring("Basic ".Length);
            if (!TokenService.ValidateToken(token, username))
            {
                rs.ResponseCode = 401;
                rs.Content = "Unauthorized: Invalid token or username.";
                return false;
            }

            return true;
        }
    }
}
