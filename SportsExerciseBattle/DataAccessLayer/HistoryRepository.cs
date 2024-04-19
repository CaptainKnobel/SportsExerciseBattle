using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using SportsExerciseBattle.Models;

namespace SportsExerciseBattle.DataAccessLayer
{
    public class HistoryRepository : IHistoryRepository
    {
        private readonly string _connectionString;

        public HistoryRepository()
        {
            _connectionString = RepositoryConnection.connectionString;
        }

        // Method to add an exercise entry
        public async Task AddEntry(int userId, string exerciseType, int count, int duration)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand(@"
                    INSERT INTO history (fk_user_id, exerciseType, count, duration, recordEntry) 
                    VALUES (@UserId, @ExerciseType, @Count, @Duration, @RecordEntry)", conn);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ExerciseType", exerciseType);
                cmd.Parameters.AddWithValue("@Count", count);
                cmd.Parameters.AddWithValue("@Duration", duration);
                cmd.Parameters.AddWithValue("@RecordEntry", false); // Default to false for record entry

                await cmd.ExecuteNonQueryAsync();
            }
        }

        // Method to retrieve all entries for a user
        public async Task<List<HistoryEntry>> GetEntries(int userId)
        {
            var entries = new List<HistoryEntry>();
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand(@"
                    SELECT history_id, exerciseType, count, duration, entryDateTime 
                    FROM history 
                    WHERE fk_user_id = @UserId 
                    ORDER BY entryDateTime DESC", conn);
                cmd.Parameters.AddWithValue("@UserId", userId);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        entries.Add(new HistoryEntry
                        {
                            HistoryId = reader.GetInt32(0),
                            ExerciseType = reader.GetString(1),
                            Count = reader.GetInt32(2),
                            Duration = reader.GetInt32(3),
                            EntryDateTime = reader.GetDateTime(4)
                        });
                    }
                }
            }
            return entries;
        }
    }
}
