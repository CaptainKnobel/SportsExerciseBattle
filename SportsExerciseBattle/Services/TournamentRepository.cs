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
                // TODO: Perform SQL insert operation to start a new tournament
            }
        }

        // TODO: what ever other methods I need
    }
}
