using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Npgsql;
using SportsExerciseBattle.Models;
using SportsExerciseBattle.DataAccessLayer;

namespace SportsExerciseBattle.Services
{
    public static class TokenService
    {
        public static string GenerateToken(User loginRequest)
        {
            var token = loginRequest.Username + "-" + Guid.NewGuid().ToString();
            using (var connection = DatabaseConnection.CreateConnection())
            {
                using (var cmdtoken = new NpgsqlCommand(@"UPDATE ""person"" SET token = @token WHERE username = @username", connection))
                {
                    cmdtoken.Parameters.AddWithValue("token", token);
                    cmdtoken.Parameters.AddWithValue("username", loginRequest.Username);
                    cmdtoken.ExecuteNonQuery();
                }
            }
            return token;
        }

        public static bool ValidateToken(string token, string username)
        {
            using (var connection = DatabaseConnection.CreateConnection())
            {
                using (var cmd = new NpgsqlCommand("SELECT token FROM person WHERE username = @username", connection))
                {
                    cmd.Parameters.AddWithValue("username", username);
                    connection.Open(); // Ensure connection is open before executing the command
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var dbToken = reader.GetString(0);
                            return token == dbToken;
                        }
                        return false;
                    }
                }
            }
        }
    }
}
