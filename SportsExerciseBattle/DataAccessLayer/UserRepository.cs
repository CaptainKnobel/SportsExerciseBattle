using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Npgsql;
using SportsExerciseBattle.Models;

namespace SportsExerciseBattle.DataAccessLayer
{
    public class UserRepository
    {
        private readonly string connectionString;

        public UserRepository(string connectionString)
        {
            this.connectionString = connectionString;
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
        public bool VerifyPassword(string username, string password)
        {
            // Retrieve the stored hash and salt for the user
            string storedHash;
            string storedSalt;
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("SELECT PasswordHash, PasswordSalt FROM Users WHERE Username = @Username", connection))
                {
                    cmd.Parameters.AddWithValue("Username", username);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
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

            // Hash the provided password with the stored salt
            var saltBytes = Convert.FromBase64String(storedSalt);
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var combinedBytes = new byte[saltBytes.Length + passwordBytes.Length];
            Buffer.BlockCopy(saltBytes, 0, combinedBytes, 0, saltBytes.Length);
            Buffer.BlockCopy(passwordBytes, 0, combinedBytes, saltBytes.Length, passwordBytes.Length);

            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(combinedBytes);
                string hash = Convert.ToBase64String(hashBytes);

                // Verify the password
                return hash == storedHash;
            }
        }
        public void AddUser(string username, string password, string name, string bio, string image, int elo)
        {
            var (hash, salt) = HashPassword(password);
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("INSERT INTO Users (Username, PasswordHash, PasswordSalt, Name, Bio, Image, Elo) VALUES (@Username, @PasswordHash, @PasswordSalt, @Name, @Bio, @Image, @Elo)", connection))
                {
                    cmd.Parameters.AddWithValue("Username", username);
                    cmd.Parameters.AddWithValue("PasswordHash", hash);
                    cmd.Parameters.AddWithValue("PasswordSalt", salt);
                    cmd.Parameters.AddWithValue("Name", name);
                    cmd.Parameters.AddWithValue("Bio", bio ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("Image", image ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("Elo", elo);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public User GetUserByUsername(string username)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("SELECT Username, Name, Bio, Image, Elo FROM Users WHERE Username = @Username", connection))
                {
                    cmd.Parameters.AddWithValue("Username", username);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                Username = reader.GetString(reader.GetOrdinal("Username")),
                                // No Password
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Bio = reader.IsDBNull(reader.GetOrdinal("Bio")) ? null : reader.GetString(reader.GetOrdinal("Bio")),
                                Image = reader.IsDBNull(reader.GetOrdinal("Image")) ? null : reader.GetString(reader.GetOrdinal("Image")),
                                Elo = reader.GetInt32(reader.GetOrdinal("Elo"))
                            };
                        }
                    }
                }
            }

            return null; // or throw an exception if user not found
        }

        // TODO: what ever other methods I need
    }
}
