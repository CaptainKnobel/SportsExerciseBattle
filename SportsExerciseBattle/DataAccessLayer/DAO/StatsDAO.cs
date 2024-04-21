using Npgsql;
using SportsExerciseBattle.DataAccessLayer.Connection;
using SportsExerciseBattle.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.DataAccessLayer.DAO
{
    public class StatsDAO
    {
        public Stats GetStats(string username)
        {
            Stats stats = null;

            try
            {
                using (var connection = DatabaseConnection.CreateConnection())
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand("SELECT username, name, elo, count FROM person WHERE username = @username", connection))
                    {
                        cmd.Parameters.AddWithValue("username", username);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                stats = new Stats
                                {
                                    Username = reader.GetString(0),
                                    Name = reader.GetString(1),
                                    Elo = reader.GetInt32(2),
                                    Count = reader.GetInt32(3)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving stats for {username}: {ex.Message}");
            }

            return stats;
        }

        public void AddToCount(string username, int count)
        {
            try
            {
                using (var connection = DatabaseConnection.CreateConnection())
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand("UPDATE person SET count = count + @count WHERE username = @username", connection))
                    {
                        cmd.Parameters.AddWithValue("count", count);
                        cmd.Parameters.AddWithValue("username", username);

                        if (cmd.ExecuteNonQuery() == 0)
                        {
                            Console.WriteLine($"No rows affected when updating count for {username}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating count for {username}: {ex.Message}");
            }
        }
    }
}
