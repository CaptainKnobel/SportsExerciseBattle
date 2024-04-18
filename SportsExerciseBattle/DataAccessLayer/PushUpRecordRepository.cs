using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using SportsExerciseBattle.Models;

namespace SportsExerciseBattle.DataAccessLayer
{
    public class PushUpRecordRepository
    {
        private readonly string _connectionString;

        public PushUpRecordRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddPushUpRecord(string username, int count, TimeSpan duration)
        {
            const string query = @"
                INSERT INTO PushUpRecords (Username, Count, Duration)
                VALUES (@username, @count, @duration);";

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@count", count);
                    cmd.Parameters.AddWithValue("@duration", duration.TotalSeconds);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // TODO: what ever other methods I need
    }
}