using System;
using System.Security.Cryptography;
using System.Text;

namespace SportsExerciseBattle.Utilities
{
    public static class TokenService
    {
        private static readonly TimeSpan TokenValidityDuration = TimeSpan.FromHours(10); // Token ist für 1 Stunde gültig

        // Generiert einen einfachen Token basierend auf dem Benutzernamen und einem Ablaufdatum.
        public static string GenerateToken(string username)
        {
            var expires = DateTime.UtcNow.Add(TokenValidityDuration);
            string tokenContent = $"{username}:{expires:o}";
            var base64Token = Convert.ToBase64String(Encoding.UTF8.GetBytes(tokenContent));
            return base64Token.Replace("+", "-").Replace("/", "_").Replace("=", ""); // URL-sichere Kodierung
        }

        public static string CleanBase64String(string input)
        {
            var output = input.Trim();
            output = output.Replace("-", "+").Replace("_", "/"); // Revert URL-sichere Kodierung

            int mod4 = output.Length % 4;
            if (mod4 > 0)
            {
                output += new string('=', 4 - mod4);
            }

            return output;
        }

        // Validiert einen Token, indem er dekodiert und überprüft wird, ob der Benutzername korrekt ist und der Token noch gültig ist.
        public static bool ValidateToken(string token, string username)
        {
            try
            {
                token = CleanBase64String(token);
                string decodedContent = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                var parts = decodedContent.Split(':');
                if (parts.Length != 2)
                    throw new FormatException("Token format is invalid.");

                string tokenUsername = parts[0];
                DateTime expires = DateTime.Parse(parts[1]);

                return tokenUsername == username && expires > DateTime.UtcNow;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Token validation failed: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error during token validation: {ex.Message}");
                return false;
            }
        }

        // Extrahiert und gibt den Benutzernamen aus dem Token zurück, wenn gültig; null andernfalls.
        public static string ValidateTokenAndGetUsername(string token)
        {
            try
            {
                token = CleanBase64String(token);
                string decodedContent = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                var parts = decodedContent.Split(':');
                if (parts.Length != 2)
                    return "";

                string tokenUsername = parts[0];
                DateTime expires = DateTime.Parse(parts[1]);

                if (expires > DateTime.UtcNow)
                    return tokenUsername;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting username from token: {ex.Message}");
            }
            return "";
        }
    }
}
