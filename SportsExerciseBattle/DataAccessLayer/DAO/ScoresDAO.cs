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
    public class ScoresDAO
    {
        public List<Stats> GetScores()
        {
            var scores = new List<Stats>();
            try
            {
                using (var connection = DatabaseConnection.CreateConnection())
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand("SELECT username, name, elo, count FROM person ORDER BY elo DESC", connection))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                scores.Add(new Stats
                                {
                                    Username = reader.GetString(0),
                                    Name = reader.GetString(1),
                                    Elo = reader.GetInt32(2),
                                    Count = reader.GetInt32(3)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving scores: {ex.Message}");
            }
            return scores;
        }
    }
}
