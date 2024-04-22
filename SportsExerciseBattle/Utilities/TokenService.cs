using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Utilities
{
    public static class TokenService
    {
        // Generates a simple token based on the username and current timestamp.
        public static string GenerateToken(string username)
        {
            string tokenContent = $"{username}:{DateTime.UtcNow}";
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(tokenContent));
        }

        // Validates a token by decoding it and checking if it contains the correct username.
        public static bool ValidateToken(string token, string username)
        {
            try
            {
                string decodedContent = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                string tokenUsername = decodedContent.Split(':')[0];
                return tokenUsername == username;
            }
            catch
            {
                // If decoding fails, the token is invalid.
                return false;
            }
        }
        // Extracts and returns the username from the token if valid, null otherwise.
        public static string ValidateTokenAndGetUsername(string token)
        {
            string decodedContent = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            string[] parts = decodedContent.Split(':');
            if (parts.Length > 0 && !string.IsNullOrEmpty(parts[0]))
            {
                return parts[0];  // Returns the username part of the token
            }
            return ""; // Return empty if data is invalid
        }

    }
}
