using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.DataAccessLayer.Connection
{
    public static class DatabaseConnection
    {
        // Hardcoded connection string
        private static readonly string _connectionString = "Host=localhost;Port=5432;Database=seb_db;Username=seb_admin;Password=seb_password;Persist Security Info=True; Include Error Detail=True";

        public static NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}

// "Host=localhost;Port=5432;Database=seb_db;Username=seb_admin;Password=seb_password;Persist Security Info=True; Include Error Detail=True"