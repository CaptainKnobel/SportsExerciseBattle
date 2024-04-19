using Npgsql;
using SportsExerciseBattle.DataAccessLayer;
using SportsExerciseBattle.Models;
using SportsExerciseBattle.Web.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.DataAccessLayer
{
    internal class SessionDAO
    {
        public bool Login(User loginRequest, HttpResponse rs)
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
                                    rs.SetSuccess("Login successful", 200);
                                    return true;
                                }
                                else
                                {
                                    rs.SetClientError("Invalid username or password", 401);
                                    return false;
                                }
                            }
                            else
                            {
                                rs.SetClientError("Invalid username or password", 401);
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                rs.SetServerError($"Internal server error: {ex.Message}");
                return false;
            }
        }

        // Assuming a method to verify password correctness.
        private bool VerifyPassword(string providedPassword, string storedPassword)
        {
            // Here, implement your password verification logic, which might include hashing or encryption comparisons.
            // Example: return HashPassword(providedPassword) == storedPassword;
            return providedPassword == storedPassword;  // Simplified for illustration. Use hashed passwords in production.
        }

        // Example method to hash a password. Implement your own hashing logic.
        private string HashPassword(string password)
        {
            // Implement your password hashing logic here
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}
