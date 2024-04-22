using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using SportsExerciseBattle.Models;
using SportsExerciseBattle.DataAccessLayer.Connection;

namespace SportsExerciseBattle.DataAccessLayer
{
    public class HistoryRepository : IHistoryRepository
    {
        public async Task<List<HistoryEntry>> GetEntries(string username)
        {
            var entries = new List<HistoryEntry>();
            using (var connection = DBConnectionManager.Instance.CreateConnection())
            {
                await connection.OpenAsync();
                using (var cmd = new NpgsqlCommand("SELECT EntryName, Count, DurationInSeconds, Timestamp FROM History WHERE Username = @Username", connection))
                {
                    cmd.Parameters.AddWithValue("Username", username);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            entries.Add(new HistoryEntry
                            {
                                EntryName = reader.GetString(0),
                                Count = reader.GetInt32(1),
                                DurationInSeconds = reader.GetInt32(2),
                                Timestamp = reader.GetDateTime(3)
                            });
                        }
                    }
                }
            }
            return entries;
        }

        public async Task AddEntry(string username, HistoryEntry entry)
        {
            using (var connection = DBConnectionManager.Instance.CreateConnection())
            {
                await connection.OpenAsync();
                using (var cmd = new NpgsqlCommand("INSERT INTO History (Username, EntryName, Count, DurationInSeconds, Timestamp) VALUES (@Username, @EntryName, @Count, @DurationInSeconds, @Timestamp)", connection))
                {
                    cmd.Parameters.AddWithValue("Username", username);
                    cmd.Parameters.AddWithValue("EntryName", entry.EntryName);
                    cmd.Parameters.AddWithValue("Count", entry.Count);
                    cmd.Parameters.AddWithValue("DurationInSeconds", entry.DurationInSeconds);
                    cmd.Parameters.AddWithValue("Timestamp", entry.Timestamp);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
