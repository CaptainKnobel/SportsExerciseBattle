using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Npgsql;
using SportsExerciseBattle.Models;
using SportsExerciseBattle.DataAccessLayer;
using SportsExerciseBattle.DataAccessLayer.Connection;

namespace SportsExerciseBattle.DataAccessLayer
{
    public class UserRepository : IUserRepository
    {
        public UserRepository()
        {
            
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
            using (var connection = DBConnectionManager.Instance.CreateConnection())
            {
                await connection.OpenAsync();
                using (var cmd = new NpgsqlCommand("SELECT PasswordHash, PasswordSalt FROM Users WHERE Username = @Username", connection))
                {
                    cmd.Parameters.AddWithValue("Username", username);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            storedHash = reader.GetString(reader.GetOrdinal("PasswordHash"));
                            storedSalt = reader.GetString(reader.GetOrdinal("PasswordSalt"));
                        }
                        else
                        {
                            return false; // User not found
                        }
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
            using (var connection = DBConnectionManager.Instance.CreateConnection())
            {
                await connection.OpenAsync();
                using (var cmd = new NpgsqlCommand("INSERT INTO Users (Username, PasswordHash, PasswordSalt, Name, Bio, Image, Elo) VALUES (@Username, @PasswordHash, @PasswordSalt, @Name, @Bio, @Image, @Elo)", connection))
                {
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
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            using (var connection = DBConnectionManager.Instance.CreateConnection())
            {
                await connection.OpenAsync();
                using (var cmd = new NpgsqlCommand("SELECT Username, Name, Bio, Image, Elo FROM Users WHERE Username = @Username", connection))
                {
                    cmd.Parameters.AddWithValue("Username", username);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new User
                            {
                                Username = reader.GetString(reader.GetOrdinal("Username")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Password = reader.GetString(reader.GetOrdinal("Password")),
                                Bio = reader.IsDBNull(reader.GetOrdinal("Bio")) ? null : reader.GetString(reader.GetOrdinal("Bio")),
                                Image = reader.IsDBNull(reader.GetOrdinal("Image")) ? null : reader.GetString(reader.GetOrdinal("Image")),
                                Elo = reader.GetInt32(reader.GetOrdinal("Elo"))
                            };
                        }
                    }
                }
            }
            return null;
        }
        public async Task UpdateUser(User user)
        {
            using (var conn = DBConnectionManager.Instance.CreateConnection())
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand("UPDATE Users SET Name = @Name, Bio = @Bio, Image = @Image, Elo = @Elo WHERE Username = @Username", conn))
                {
                    cmd.Parameters.AddWithValue("@Name", user.Name ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Bio", user.Bio ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Image", user.Image ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Elo", user.Elo);
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<UserStats> GetUserStats(string username)
        {
            using (var connection = DBConnectionManager.Instance.CreateConnection())
            {
                await connection.OpenAsync();
                using (var cmd = new NpgsqlCommand("SELECT TotalPushUps, Elo FROM UserStats WHERE Username = @Username", connection))
                {
                    cmd.Parameters.AddWithValue("Username", username);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new UserStats
                            {
                                Username = username,
                                TotalPushUps = reader.GetInt32(0),
                                Elo = reader.GetInt32(1)
                            };
                        }
                    }
                }
            }
            return new UserStats { Username = username, TotalPushUps = 0, Elo = 1200 }; // Default stats if none found
        }

        public async Task<List<ScoreboardEntry>> GetScoreboardData()
        {
            var entries = new List<ScoreboardEntry>();
            using (var connection = DBConnectionManager.Instance.CreateConnection())
            {
                await connection.OpenAsync();
                using (var cmd = new NpgsqlCommand("SELECT Username, TotalPushUps, Elo FROM Scoreboard ORDER BY Elo DESC", connection))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            entries.Add(new ScoreboardEntry
                            {
                                Username = reader.GetString(0),
                                TotalPushUps = reader.GetInt32(1),
                                Elo = reader.GetInt32(2)
                            });
                        }
                    }
                }
            }
            return entries;
        }
    }
}
