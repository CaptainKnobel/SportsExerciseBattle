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

namespace SportsExerciseBattle.Web.Endpoints
{
    public class UsersEndpoint : IHttpEndpoint
    {
        private readonly UserRepository userRepository = new UserRepository();

        public UsersEndpoint()
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
                    case HttpMethod.GET:
                        return HandleGet(request, response).Result;
                    case HttpMethod.PUT:
                        return HandlePut(request, response).Result;
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
                var user = JsonSerializer.Deserialize<User>(request.Content ?? string.Empty);
                if (user == null)
                {
                    response.ResponseCode = 400; // Bad Request
                    response.ResponseMessage = "Invalid user data";
                    return false;
                }

                await userRepository.AddUser(user.Username, user.Password, user.Name, user.Bio, user.Image, user.Elo);
                response.ResponseCode = 201; // Created
                response.ResponseMessage = "User created successfully";
                return true;
            }
            catch (Exception ex)
            {
                response.ResponseCode = 400; // Bad Request
                response.ResponseMessage = $"Error creating user: {ex.Message}";
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
            var user = await userRepository.GetUserByUsername(username);
            if (user == null)
            {
                response.ResponseCode = 404; // Not Found
                response.ResponseMessage = "User not found";
                return false;
            }

            response.ResponseCode = 200; // OK
            response.ResponseMessage = "OK";
            response.Content = JsonSerializer.Serialize(user);
            response.Headers.Add("Content-Type", "application/json");
            return true;
        }

        private async Task<bool> HandlePut(HttpRequest request, HttpResponse response)
        {
            if (request.Path.Length < 3)
            {
                response.ResponseCode = 400; // Bad Request
                response.ResponseMessage = "Username is required for update";
                return false;
            }

            string username = request.Path[2];
            var userUpdate = JsonSerializer.Deserialize<User>(request.Content ?? string.Empty);
            if (userUpdate == null)
            {
                response.ResponseCode = 400; // Bad Request
                response.ResponseMessage = "Invalid user data";
                return false;
            }

            // Assuming a method in UserRepository to update user data
            await userRepository.UpdateUser(username, userUpdate);
            response.ResponseCode = 200; // OK
            response.ResponseMessage = "User updated successfully";
            return true;
        }
    }
}
