using Npgsql;
using SportsExerciseBattle.DataAccessLayer.Connection;
using SportsExerciseBattle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.DataAccessLayer
{
    public class ScoresRepository : IScoresRepository
    {
        

        public ScoresRepository()
        {
            
        }

        public async Task<List<ScoreboardEntry>> GetScoresAsync()
        {
            var scores = new List<ScoreboardEntry>();
            using (var conn = DBConnectionManager.Instance.CreateConnection())
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand("SELECT username, elo, pushup_count FROM users ORDER BY elo DESC, pushup_count DESC", conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            scores.Add(new ScoreboardEntry
                            {
                                Username = reader.GetString(0),
                                TotalPushUps = reader.GetInt32(2),
                                Elo = reader.GetInt32(1)
                            });
                        }
                    }
                }
            }
            return scores;
        }
    }

}
