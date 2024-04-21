using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace SportsExerciseBattle.DataAccessLayer
{
    public class ScoreRepository
    {
        private NpgsqlConnection _connection;

        public ScoreRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<Score>> GetScoresAsync()
        {
            var scores = new List<Score>();
            var sql = "SELECT * FROM scores;";
            await using (var cmd = new NpgsqlCommand(sql, _connection))
            {
                await _connection.OpenAsync();
                var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    scores.Add(new Score { Username = reader.GetString(0), Points = reader.GetInt32(1) });
                }
                await _connection.CloseAsync();
            }
            return scores;
        }
    }
}
