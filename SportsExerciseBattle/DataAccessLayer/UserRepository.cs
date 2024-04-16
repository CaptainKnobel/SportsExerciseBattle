using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public void AddUser(User user)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("INSERT INTO Users (Username, Password, Name, Bio, Image, Elo) VALUES (@Username, @Password, @Name, @Bio, @Image, @Elo)", connection))
                {
                    cmd.Parameters.AddWithValue("Username", user.Username);
                    cmd.Parameters.AddWithValue("Password", user.Password); // TODO: hashing the password!
                    cmd.Parameters.AddWithValue("Name", user.Name);
                    cmd.Parameters.AddWithValue("Bio", user.Bio ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("Image", user.Image ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("Elo", user.Elo);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public User GetUserByUsername(string username)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("SELECT Username, Password, Name, Bio, Image, Elo FROM Users WHERE Username = @Username", connection))
                {
                    cmd.Parameters.AddWithValue("Username", username);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                Username = reader.GetString(reader.GetOrdinal("Username")),
                                Password = reader.GetString(reader.GetOrdinal("Password")), // Password should be hashed and never exposed
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
