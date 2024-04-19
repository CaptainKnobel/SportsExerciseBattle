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
    public class TournamentDAO
    {
        public void GetParticipants()
        {
            var tournament = Tournament.Instance;
            try
            {
                using (var connection = DatabaseConnection.CreateConnection())
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand("SELECT DISTINCT username FROM history WHERE timestamp BETWEEN @start_timestamp AND @end_timestamp", connection))
                    {
                        cmd.Parameters.AddWithValue("start_timestamp", tournament.StartTime);
                        cmd.Parameters.AddWithValue("end_timestamp", tournament.StartTime.AddMinutes(2));

                        using (var reader = cmd.ExecuteReader())
                        {
                            var uniqueParticipants = new HashSet<string>();
                            while (reader.Read())
                            {
                                uniqueParticipants.Add(reader.GetString(0));
                            }
                            tournament.Participants = uniqueParticipants.ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving participants: {ex.Message}");
            }
        }

        public void GetLeaders()
        {
            var tournament = Tournament.Instance;
            try
            {
                using (var connection = DatabaseConnection.CreateConnection())
                {
                    connection.Open();
                    int maxPushups = GetMaxPushups(tournament, connection);

                    using (var cmd = new NpgsqlCommand("SELECT username FROM history WHERE timestamp BETWEEN @start_timestamp AND @end_timestamp GROUP BY username HAVING SUM(count) = @max_pushups", connection))
                    {
                        cmd.Parameters.AddWithValue("start_timestamp", tournament.StartTime);
                        cmd.Parameters.AddWithValue("end_timestamp", tournament.StartTime.AddMinutes(2));
                        cmd.Parameters.AddWithValue("max_pushups", maxPushups);

                        using (var reader = cmd.ExecuteReader())
                        {
                            var leadingUsers = new HashSet<string>();
                            while (reader.Read())
                            {
                                leadingUsers.Add(reader.GetString(0));
                            }
                            tournament.LeadingUsers = leadingUsers.ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving leaders: {ex.Message}");
            }
        }

        private int GetMaxPushups(Tournament tournament, NpgsqlConnection connection)
        {
            using (var cmd = new NpgsqlCommand("SELECT MAX(total_pushups) FROM (SELECT SUM(count) AS total_pushups FROM history WHERE timestamp BETWEEN @start_timestamp AND @end_timestamp GROUP BY username) AS subquery", connection))
            {
                cmd.Parameters.AddWithValue("start_timestamp", tournament.StartTime);
                cmd.Parameters.AddWithValue("end_timestamp", tournament.StartTime.AddMinutes(2));
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read() && !reader.IsDBNull(0))
                        return reader.GetInt32(0);
                }
            }
            return 0;
        }

        public void UpdateElo()
        {
            var tournament = Tournament.Instance;
            try
            {
                using (var connection = DatabaseConnection.CreateConnection())
                {
                    connection.Open();
                    string participantsParam = string.Join(",", tournament.Participants);
                    string leadersParam = string.Join(",", tournament.LeadingUsers);

                    var cmdText = tournament.LeadingUsers.Count > 1 ?
                                  "UPDATE person SET elo = elo + 1 WHERE username = ANY(@usernames);" :
                                  "UPDATE person SET elo = elo + 2 WHERE username = ANY(@usernames); UPDATE person SET elo = elo - 1 WHERE username != ALL(@usernames);";

                    using (var cmd = new NpgsqlCommand(cmdText, connection))
                    {
                        cmd.Parameters.AddWithValue("@usernames", NpgsqlTypes.NpgsqlDbType.Array | NpgsqlTypes.NpgsqlDbType.Text, tournament.Participants.ToArray());
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating Elo: {ex.Message}");
            }
        }

        public void UpdateTournamentStats()
        {
            var tournament = Tournament.Instance;
            try
            {
                using (var connection = DatabaseConnection.CreateConnection())
                {
                    connection.Open();
                    var cmdText = "UPDATE person SET wins = wins + 1 WHERE username = ANY(@usernames); UPDATE person SET losses = losses + 1 WHERE username != ALL(@usernames);";
                    using (var cmd = new NpgsqlCommand(cmdText, connection))
                    {
                        cmd.Parameters.AddWithValue("@usernames", NpgsqlTypes.NpgsqlDbType.Array | NpgsqlTypes.NpgsqlDbType.Text, tournament.LeadingUsers.ToArray());
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating tournament stats: {ex.Message}");
            }
        }

        public void UpdateTournamentDetails(Tournament tournament)
        {
            GetParticipants();
            GetLeaders();
            UpdateElo();
            UpdateTournamentStats();
        }
    }
}
