using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using SEB.Models;

namespace SEB.Services
{
    public class TournamentRepository
    {
        private readonly string connectionString;

        public TournamentRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void StartTournament(Tournament tournament)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "INSERT INTO Tournaments (StartTime) VALUES (@startTime)";
                    cmd.Parameters.AddWithValue("startTime", tournament.StartTime);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // TODO: what ever other methods I need

    } // <- End of TournamentRepository class
} // <- End of SEB.Services namesspace
