using Npgsql;
using SportsExerciseBattle.DataAccessLayer.Connection;
using SportsExerciseBattle.Models;
using SportsExerciseBattle.Web.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.DataAccessLayer.DAO
{
    public class HistoryDAO
    {
        private StatsDAO statsDAO = new StatsDAO();

        public void AddEntry(string username, Entry entry, HttpResponse rs)
        {
            try
            {
                using (var connection = DatabaseConnection.CreateConnection())
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand(@"INSERT INTO ""history""(username, entryname, count, duration, timestamp) VALUES (@username, @entryname, @count, @duration, @timestamp)", connection))
                    {
                        cmd.Parameters.AddWithValue("username", username);
                        cmd.Parameters.AddWithValue("entryname", entry.EntryName);
                        cmd.Parameters.AddWithValue("count", entry.Count);
                        cmd.Parameters.AddWithValue("duration", entry.DurationInSeconds);
                        cmd.Parameters.AddWithValue("timestamp", entry.Timestamp);

                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            rs.SetSuccess("Entry added successfully", 201);
                            statsDAO.AddToCount(username, entry.Count);
                        }
                        else
                        {
                            rs.SetClientError("Failed to add entry", 400);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                rs.SetServerError($"Internal server error: {ex.Message}");
            }
        }

        public List<Entry> GetEntries(string username)
        {
            var entries = new List<Entry>();
            try
            {
                using (var connection = DatabaseConnection.CreateConnection())
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand("SELECT username, entryname, count, duration, timestamp FROM history WHERE username = @username", connection))
                    {
                        cmd.Parameters.AddWithValue("username", username);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                entries.Add(new Entry
                                {
                                    Username = reader.GetString(0),
                                    EntryName = reader.GetString(1),
                                    Count = reader.GetInt32(2),
                                    DurationInSeconds = reader.GetInt32(3),
                                    Timestamp = reader.GetDateTime(4)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving entries: {ex.Message}");
            }
            return entries;
        }
    }
}
