using Npgsql;
using SportsExerciseBattle.DataAccessLayer;
using SportsExerciseBattle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.DataAccessLayer
{
    public class TournamentStatsDAO
    {
        public TournamentStats GetTournamentStats(string username)
        {
            TournamentStats tournamentStats = null;

            try
            {
                using (var connection = DatabaseConnection.CreateConnection())
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand("SELECT wins, draws, losses FROM person WHERE username = @username", connection))
                    {
                        cmd.Parameters.AddWithValue("username", username);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                tournamentStats = new TournamentStats
                                {
                                    Wins = reader.GetInt32(0),
                                    Draws = reader.GetInt32(1),
                                    Losses = reader.GetInt32(2)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving tournament stats: {ex.Message}");
            }

            return tournamentStats;
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
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding to count: {ex.Message}");
            }
        }
    }
}
