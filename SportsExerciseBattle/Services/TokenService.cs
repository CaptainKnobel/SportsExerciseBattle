using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Services
{
    public static class TokenService
    {
        public static string CreateToken(string username)
        {
            var issueDate = DateTime.UtcNow;
            var expiryDate = issueDate.AddHours(1); // Token valid for 1 hour
            var data = $"{username}|{expiryDate:O}";

            // Convert to Base64
            var bytes = Encoding.UTF8.GetBytes(data);
            return Convert.ToBase64String(bytes);
        }

        public static bool ValidateToken(string token, string username)
        {
            try
            {
                var bytes = Convert.FromBase64String(token);
                var decoded = Encoding.UTF8.GetString(bytes);
                var parts = decoded.Split('|');
                if (parts.Length != 2)
                    return false;

                var tokenUsername = parts[0];
                var expiry = DateTime.Parse(parts[1]);

                return tokenUsername == username && expiry > DateTime.UtcNow;
            }
            catch
            {
                // Log exception details here to understand what went wrong
                return false;
            }
        }
    }
}
