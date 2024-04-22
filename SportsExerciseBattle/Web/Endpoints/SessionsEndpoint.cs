using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HttpMethod = SportsExerciseBattle.Web.HTTP.HttpMethod; // makes sure the correct HttpMethod is used
using SportsExerciseBattle.Models;
using SportsExerciseBattle.Web.HTTP;
using SportsExerciseBattle.Services;
using SportsExerciseBattle.DataAccessLayer.DAO;

namespace SportsExerciseBattle.Web.Endpoints
{
    public class SessionsEndpoint : IHttpEndpoint
    {
        private SessionDAO sessionDAO = new SessionDAO(); // Create an instance of SessionDAO

        public bool HandleRequest(HttpRequest rq, HttpResponse rs)
        {
            if (rq.Method == HttpMethod.POST)
            {
                return Login(rq, rs);
            }
            return false;
        }

        private bool Login(HttpRequest rq, HttpResponse rs)
        {
            try
            {
                var loginRequest = JsonSerializer.Deserialize<User>(rq.Content ?? "{}");
                if (loginRequest == null)
                {
                    rs.ResponseCode = 400;
                    rs.Content = "Invalid login data provided.";
                    return false;
                }

                if (sessionDAO.Login(loginRequest, rs)) // Pass HttpResponse parameter
                {
                    var token = TokenService.GenerateToken(loginRequest);
                    rs.Content = JsonSerializer.Serialize(new { Token = token });
                    rs.SetJsonContentType();
                    rs.ResponseCode = 200;
                    return true;
                }
                else
                {
                    rs.ResponseCode = 401;
                    rs.Content = "Login failed.";
                    return false;
                }
            }
            catch (JsonException)
            {
                rs.ResponseCode = 400;
                rs.Content = "Failed to parse login data.";
                return false;
            }
            catch (Exception ex)
            {
                rs.SetServerError(ex.Message); // Pass the exception message
                return false;
            }
        }
    }
}
