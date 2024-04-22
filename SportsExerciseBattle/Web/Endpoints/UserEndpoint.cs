using SportsExerciseBattle.Models;
using SportsExerciseBattle.Web.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HttpMethod = SportsExerciseBattle.Web.HTTP.HttpMethod; // makes sure the correct HttpMethod is used
using SportsExerciseBattle.DataAccessLayer;
using SportsExerciseBattle.Utilities;

namespace SportsExerciseBattle.Web.Endpoints
{
    public class UsersEndpoint : IHttpEndpoint
    {
        private readonly IUserRepository userRepository = new UserRepository();

        public UsersEndpoint()
        {
            
        }

        public bool HandleRequest(HttpRequest rq, HttpResponse rs)
        {
            try
            {
                switch (rq.Method)
                {
                    case HttpMethod.POST:
                        return CreateUser(rq, rs).GetAwaiter().GetResult();
                    case HttpMethod.GET:
                        return GetUserData(rq, rs).GetAwaiter().GetResult();
                    case HttpMethod.PUT:
                        return UpdateUser(rq, rs).GetAwaiter().GetResult();
                    default:
                        return false;
                }
            }
            catch (Exception ex)
            {
                rs.SetServerError($"Internal server error: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> CreateUser(HttpRequest rq, HttpResponse rs)
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

                await userRepository.AddUser(user.Username, user.Password, user.Name, user.Bio ?? "", user.Image ?? "", user.Elo);
                rs.ResponseCode = 201;
                rs.Content = "User created successfully.";
                return true;
            }
            catch (JsonException)
            {
                rs.ResponseCode = 400;
                rs.Content = "Failed to parse user data.";
                return false;
            }
        }

        private async Task<bool> GetUserData(HttpRequest rq, HttpResponse rs)
        {
            var username = rq.Path.LastOrDefault() ?? "";
            if (!TryAuthorize(rq, rs, username))
            {
                return false;
            }

            try
            {
                var user = await userRepository.GetUserByUsername(username);
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
                rs.SetServerError(ex.Message);
                return false;
            }
        }

        private async Task<bool> UpdateUser(HttpRequest rq, HttpResponse rs)
        {
            var username = rq.Path.LastOrDefault() ?? "";
            if (!TryAuthorize(rq, rs, username))
            {
                return false;
            }

            try
            {
                var currentUser = await userRepository.GetUserByUsername(username);
                if (currentUser == null)
                {
                    rs.ResponseCode = 404;
                    rs.Content = "User not found.";
                    return false;
                }

                var newData = JsonSerializer.Deserialize<User>(rq.Content ?? "{}");
                if (newData == null)
                {
                    rs.ResponseCode = 400;
                    rs.Content = "Invalid update data.";
                    return false;
                }

                // Merging data with existing user details
                currentUser.Name = newData.Name ?? currentUser.Name;
                currentUser.Bio = newData.Bio ?? currentUser.Bio;
                currentUser.Image = newData.Image ?? currentUser.Image;
                currentUser.Elo = newData.Elo;

                await userRepository.UpdateUser(currentUser);
                rs.ResponseCode = 200;
                rs.Content = "User updated successfully.";
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
                rs.SetServerError(ex.Message);
                return false;
            }
        }

        private bool TryAuthorize(HttpRequest rq, HttpResponse rs, string username)
        {
            if (!rq.Headers.TryGetValue("Authorization", out string? authHeader) || !authHeader.StartsWith("Basic "))
            {
                rs.ResponseCode = 401;
                rs.Content = "Unauthorized: Authentication required.";
                return false;
            }

            var token = authHeader.Substring("Basic ".Length);
            return TokenService.ValidateToken(token, username); // Assume ValidateToken returns a Task<bool>
        }
    }
}

