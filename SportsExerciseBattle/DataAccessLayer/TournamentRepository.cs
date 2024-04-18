using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Npgsql;
using SportsExerciseBattle.Models;

namespace SportsExerciseBattle.DataAccessLayer
{
    public class TournamentRepository : ITournamentRepository
    {
        private readonly string _connectionString;

        public TournamentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> StartUserTournament(string username, string tournamentData)
        {
            // Logic to insert a new tournament entry
            const string query = @"
                INSERT INTO Tournaments (Username, Data, StartDate)
                VALUES (@username, @data, NOW())
                RETURNING TournamentID;";

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@data", tournamentData);

                    var tournamentId = await cmd.ExecuteScalarAsync();
                    return tournamentId != null;
                }
            }
        }

        public async Task<bool> UpdateEloScores(string username, int eloChange)
        {
            // Logic to update ELO score
            const string query = @"
                UPDATE Users
                SET Elo = Elo + @eloChange
                WHERE Username = @username;";

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@eloChange", eloChange);
                    cmd.Parameters.AddWithValue("@username", username);

                    var result = await cmd.ExecuteNonQueryAsync();
                    return result > 0;
                }
            }
        }

        public async Task<string> GetTournamentResults(int tournamentId)
        {
            // Logic to fetch tournament results
            const string query = @"
                SELECT Username, Data
                FROM Tournaments
                WHERE TournamentID = @tournamentId;";

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@tournamentId", tournamentId);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            string username = reader.GetString(reader.GetOrdinal("Username"));
                            string data = reader.GetString(reader.GetOrdinal("Data"));
                            return JsonSerializer.Serialize(new { Username = username, Data = data });
                        }
                    }
                }
            }
            return null; // Return null or appropriate default if no results found
        }
        public async Task<int> RecordTournamentOutcome(string winnerUsername, IEnumerable<string> participantUsernames)
        {
            if (string.IsNullOrWhiteSpace(winnerUsername) || participantUsernames.Any(string.IsNullOrWhiteSpace))
            {
                return 0; // Return failure if any username is null or empty
            }
            // Logic to update the winner's ELO and decrement for others
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        // Update the winner's ELO
                        var updateWinnerCmd = new NpgsqlCommand("UPDATE Users SET Elo = Elo + 2 WHERE Username = @Username", conn, tran);
                        updateWinnerCmd.Parameters.AddWithValue("@Username", winnerUsername);
                        await updateWinnerCmd.ExecuteNonQueryAsync();

                        // Update ELO for other participants
                        foreach (var username in participantUsernames)
                        {
                            if (username != winnerUsername)
                            {
                                var updateParticipantCmd = new NpgsqlCommand("UPDATE Users SET Elo = Elo - 1 WHERE Username = @Username", conn, tran);
                                updateParticipantCmd.Parameters.AddWithValue("@Username", username);
                                await updateParticipantCmd.ExecuteNonQueryAsync();
                            }
                        }

                        tran.Commit();
                        return 1; // Success
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        Console.WriteLine("Error during transaction: " + ex.Message);
                        return 0; // Failure
                    }
                }
            }
        }
    } // <- End of TournamentRepository class
} // <- End of SportsExerciseBattle.Services namesspace
