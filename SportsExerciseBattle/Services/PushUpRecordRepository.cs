using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using SEB.Models;

namespace SEB.Services
{
    public class PushUpRecordRepository
    {
        private readonly string connectionString;

        public PushUpRecordRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void AddPushUpRecord(PushUpRecord record)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                // TODO: Perform SQL insert operation to add push-up record to database
            }
        }

        // TODO: what ever other methods I need
    }
}