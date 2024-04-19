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
using SportsExerciseBattle.DataAccessLayer;
using SportsExerciseBattle.Web.HTTP;
using SportsExerciseBattle.Services;

namespace SportsExerciseBattle.Web.Endpoints
{
    public class SessionsEndpoint : IHttpEndpoint
    {
        private readonly UserRepository _userRepository = new UserRepository();

        public SessionsEndpoint()
        {
        }

        public bool HandleRequest(HttpRequest request, HttpResponse response)
        {
            try
            {
                switch (request.Method)
                {
                    case HttpMethod.POST:
                        return HandlePost(request, response).Result;
                    default:
                        response.ResponseCode = 405;  // Method Not Allowed
                        return false;
                }
            }
            catch (Exception ex)
            {
                response.ResponseCode = 500; // Internal Server Error
                response.ResponseMessage = $"Internal server error: {ex.Message}";
                return false;
            }
        }

        private async Task<bool> HandlePost(HttpRequest request, HttpResponse response)
        {
            try
            {
                var loginRequest = JsonSerializer.Deserialize<LoginRequest>(request.Content ?? string.Empty);
                if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
                {
                    response.ResponseMessage = "Invalid login data"; // Sending back a response for invalid data
                    return false;
                }

                if (await _userRepository.VerifyPassword(loginRequest.Username, loginRequest.Password))
                {
                    var user = await _userRepository.GetUserByUsername(loginRequest.Username);
                    if (user != null)
                    {
                        var token = TokenService.CreateToken(user.Username); // Generating token using the TokenService
                        response.Content = JsonSerializer.Serialize(new { Token = token });
                        response.Headers.Add("Content-Type", "application/json");
                        response.ResponseMessage = "Login successful";
                        return true;
                    }
                }

                response.ResponseMessage = "Invalid username or password"; // Sending back a response for failed authentication
                return false;
            }
            catch (Exception ex)
            {
                response.ResponseMessage = $"Error processing request: {ex.Message}"; // Handling exceptions
                return false;
            }
        }
    }
}
