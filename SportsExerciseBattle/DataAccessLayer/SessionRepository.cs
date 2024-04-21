using Npgsql;
using SportsExerciseBattle.DataAccessLayer.Connection;
using SportsExerciseBattle.Models;
using SportsExerciseBattle.Web.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.DataAccessLayer
{
    public class SessionRepository : ISessionRepository
    {
        public bool Login(User loginRequest, HttpResponse response)
        {
            try
            {
                using (var connection = DatabaseConnection.CreateConnection())
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand("SELECT password FROM person WHERE username = @username", connection))
                    {
                        cmd.Parameters.AddWithValue("username", loginRequest.Username);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedPassword = reader.GetString(0);
                                if (VerifyPassword(loginRequest.Password, storedPassword))
                                {
                                    response.SetSuccess("Login successful", 200);
                                    return true;
                                }
                                else
                                {
                                    response.SetClientError("Invalid username or password", 401);
                                    return false;
                                }
                            }
                            else
                            {
                                response.SetClientError("Invalid username or password", 401);
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.SetServerError($"Internal server error: {ex.Message}");
                return false;
            }
        }

        private bool VerifyPassword(string providedPassword, string storedPassword)
        {
            return providedPassword == storedPassword; // TODO: better 
        }
    }
}
