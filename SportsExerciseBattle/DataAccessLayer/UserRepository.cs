using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Npgsql;
using SportsExerciseBattle.Models;
using SportsExerciseBattle.DataAccessLayer;

namespace SportsExerciseBattle.DataAccessLayer
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private (string Hash, string Salt) HashPassword(string password)
        {
            // Generate a salt
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            // Combine the password and the salt before hashing
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] combinedBytes = new byte[saltBytes.Length + passwordBytes.Length];
            Buffer.BlockCopy(saltBytes, 0, combinedBytes, 0, saltBytes.Length);
            Buffer.BlockCopy(passwordBytes, 0, combinedBytes, saltBytes.Length, passwordBytes.Length);

            // Hash the combined password and salt
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(combinedBytes);
                string hash = Convert.ToBase64String(hashBytes);
                string salt = Convert.ToBase64String(saltBytes);
                return (hash, salt);
            }
        }
        public async Task<bool> VerifyPassword(string username, string password)
        {
            string storedHash;
            string storedSalt;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var cmd = new NpgsqlCommand("SELECT passwordHash, PasswordSalt FROM users WHERE username = @Username", connection);
                cmd.Parameters.AddWithValue("Username", username);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        storedHash = reader.GetString(0);
                        storedSalt = reader.GetString(1);
                    }
                    else
                    {
                        return false; // User not found
                    }
                }
            }

            // Proceed to hash the provided password and compare
            var saltBytes = Convert.FromBase64String(storedSalt);
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var combinedBytes = new byte[saltBytes.Length + passwordBytes.Length];
            Buffer.BlockCopy(saltBytes, 0, combinedBytes, 0, saltBytes.Length);
            Buffer.BlockCopy(passwordBytes, 0, combinedBytes, saltBytes.Length, passwordBytes.Length);
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(combinedBytes);
                string hash = Convert.ToBase64String(hashBytes);
                return hash == storedHash;
            }
        }
        public async Task AddUser(string username, string password, string name, string bio, string image, int elo)
        {
            var (hash, salt) = HashPassword(password);
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var cmd = new NpgsqlCommand("INSERT INTO users (username, passwordHash, PasswordSalt, name, bio, image, elo) VALUES (@Username, @PasswordHash, @PasswordSalt, @Name, @Bio, @Image, @Elo)", connection);
                cmd.Parameters.AddWithValue("Username", username);
                cmd.Parameters.AddWithValue("PasswordHash", hash);
                cmd.Parameters.AddWithValue("PasswordSalt", salt);
                cmd.Parameters.AddWithValue("Name", name);
                cmd.Parameters.AddWithValue("Bio", bio ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("Image", image ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("Elo", elo);
                await cmd.ExecuteNonQueryAsync();
            }
        }
        public async Task<User?> GetUserByUsername(string username)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var cmd = new NpgsqlCommand("SELECT username, name, bio, image, elo FROM users WHERE username = @Username", connection);
                cmd.Parameters.AddWithValue("Username", username);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new User
                        {
                            Username = reader.GetString(0),
                            Name = reader.GetString(1),
                            Bio = reader.IsDBNull(2) ? null : reader.GetString(2),
                            Image = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Elo = reader.GetInt32(4)
                        };
                    }
                }
            }
            return null; // User not found
        }
        public async Task<UserStats> GetUserStats(string username)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var cmd = new NpgsqlCommand(@"
                    SELECT u.username, SUM(h.count) AS total_pushups, MAX(u.elo) AS elo
                    FROM users u
                    LEFT JOIN history h ON u.user_id = h.fk_user_id
                    WHERE u.username = @Username
                    GROUP BY u.username", connection);
                cmd.Parameters.AddWithValue("Username", username);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new UserStats
                        {
                            Username = reader.GetString(reader.GetOrdinal("username")),
                            TotalPushUps = reader.GetInt32(reader.GetOrdinal("total_pushups")),
                            Elo = reader.GetInt32(reader.GetOrdinal("elo"))
                        };
                    }
                }
            }
            // Return default stats if no data is found
            return new UserStats { Username = username, TotalPushUps = 0, Elo = 1000 };
        }

        public async Task<List<ScoreboardEntry>> GetScoreboardData()
        {
            var scoreboardEntries = new List<ScoreboardEntry>();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var cmd = new NpgsqlCommand(@"
                    SELECT username, SUM(h.count) AS total_pushups, MAX(u.elo) AS elo
                    FROM users u
                    LEFT JOIN history h ON u.user_id = h.fk_user_id
                    GROUP BY u.username
                    ORDER BY elo DESC, total_pushups DESC", connection);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        scoreboardEntries.Add(new ScoreboardEntry
                        {
                            Username = reader.GetString(reader.GetOrdinal("username")),
                            TotalPushUps = reader.GetInt32(reader.GetOrdinal("total_pushups")),
                            Elo = reader.GetInt32(reader.GetOrdinal("elo"))
                        });
                    }
                }
            }
            return scoreboardEntries;
        }
    }
}
