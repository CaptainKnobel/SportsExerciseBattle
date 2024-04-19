using SportsExerciseBattle.DataAccessLayer;
using SportsExerciseBattle.Web.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HttpMethod = SportsExerciseBattle.Web.HTTP.HttpMethod; // makes sure the correct HttpMethod is used

namespace SportsExerciseBattle.Web.Endpoints
{
    public class StatsEndpoint : IHttpEndpoint
    {
        private readonly UserRepository _userRepository = new UserRepository();

        public StatsEndpoint()
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
                response.ResponseCode = 405;  // Method Not Allowed
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
            if (request.Path.Length < 3)
            {
                response.ResponseCode = 400; // Bad Request
                response.ResponseMessage = "Username is required";
                return false;
            }

            string username = request.Path[2];
            var userStats = await _userRepository.GetUserStats(username);
            if (userStats == null)
            {
                response.ResponseCode = 404; // Not Found
                response.ResponseMessage = "Stats not found";
                return false;
            }

            response.ResponseCode = 200; // OK
            response.ResponseMessage = "OK";
            response.Content = JsonSerializer.Serialize(userStats);
            response.Headers.Add("Content-Type", "application/json");
            return true;
        }
    }
}
