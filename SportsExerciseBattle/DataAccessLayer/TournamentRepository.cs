using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Npgsql;
using SportsExerciseBattle.DataAccessLayer.Connection;
using SportsExerciseBattle.Models;

namespace SportsExerciseBattle.DataAccessLayer
{
    public class TournamentRepository : ITournamentRepository
    {
        public List<string> GetParticipants(Tournament tournament)
        {
            var participants = new List<string>();
            using (var connection = DatabaseConnection.CreateConnection())
            {
                connection.Open();
                var cmd = new NpgsqlCommand(@"
                    SELECT DISTINCT username FROM history 
                    WHERE timestamp BETWEEN @start_timestamp AND @end_timestamp", connection);
                cmd.Parameters.AddWithValue("@start_timestamp", tournament.StartTime);
                cmd.Parameters.AddWithValue("@end_timestamp", tournament.StartTime.AddMinutes(2));
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        participants.Add(reader.GetString(0));
                    }
                }
            }
            return participants;
        }

        public void UpdateElo(Tournament tournament, bool isDraw)
        {
            using (var connection = DatabaseConnection.CreateConnection())
            {
                connection.Open();
                string cmdText = isDraw ?
                    "UPDATE users SET elo = elo + 1 WHERE username = ANY(@participants);" :
                    "UPDATE users SET elo = elo + 2 WHERE username IN @leaders; UPDATE users SET elo = elo - 1 WHERE username NOT IN @participants;";
                using (var cmd = new NpgsqlCommand(cmdText, connection))
                {
                    cmd.Parameters.AddWithValue("@participants", NpgsqlTypes.NpgsqlDbType.Array | NpgsqlTypes.NpgsqlDbType.Text, tournament.Participants.ToArray());
                    if (!isDraw)
                    {
                        cmd.Parameters.AddWithValue("@leaders", NpgsqlTypes.NpgsqlDbType.Array | NpgsqlTypes.NpgsqlDbType.Text, tournament.LeadingUsers.ToArray());
                    }
                    cmd.ExecuteNonQuery();
                }
            }
        }
    } // <- End of TournamentRepository class
} // <- End of SportsExerciseBattle.Services namesspace
