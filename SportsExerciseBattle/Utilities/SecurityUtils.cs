using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace SportsExerciseBattle.Utilities
{
    public static class SecurityUtils
    {
        private static string secretKey = "totally_secret_key";

        public static string GenerateToken(string username)
        {
            var time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            var key = Encoding.UTF8.GetBytes(secretKey);
            var usernameBytes = Encoding.UTF8.GetBytes(username);

            var tokenData = new byte[time.Length + usernameBytes.Length];
            Buffer.BlockCopy(time, 0, tokenData, 0, time.Length);
            Buffer.BlockCopy(usernameBytes, 0, tokenData, time.Length, usernameBytes.Length);

            using (var hmac = new HMACSHA256(key))
            {
                var token = hmac.ComputeHash(tokenData);
                return Convert.ToBase64String(token);
            }
        }

        public static bool ValidateToken(string token, string username)
        {
            // Regenerate the token from the username and compare with the provided token
            var regeneratedToken = GenerateToken(username);
            return regeneratedToken == token;
        }
    }
}
