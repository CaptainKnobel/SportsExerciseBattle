using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace SportsExerciseBattle.DataAccessLayer.Database
{
    public class DatabaseSetup
    {
        private readonly string _connectionString;
        //private readonly string _defaultConnectionString = "Host=localhost;Username=postgres;Password=postgres;Database=postgres";

        public DatabaseSetup(string connectionString)
        {
            _connectionString = connectionString;
        }
        /*
        // Setup Everything
        public async Task SetupDatabaseAsync()
        {
            await CreateDatabaseIfNotExistsAsync();
            await CreateTablesIfNotExistAsync();
        }

        // Setup the Database
        private async Task CreateDatabaseIfNotExistsAsync()
        {
            var dbName = new NpgsqlConnectionStringBuilder(_connectionString).Database;
            using (var conn = new NpgsqlConnection(_defaultConnectionString))
            {
                await conn.OpenAsync();
                // Check if database exists
                var checkDbExistsCommand = new NpgsqlCommand($"SELECT 1 FROM pg_database WHERE datname = '{dbName}'", conn);
                if ((await checkDbExistsCommand.ExecuteScalarAsync()) == null)
                {
                    // Create database if it doesn't exist
                    var createDbCommand = new NpgsqlCommand($"CREATE DATABASE \"{dbName}\"", conn);
                    await createDbCommand.ExecuteNonQueryAsync();
                }
            }
        }
        */
        // Setup the Tables
        public async Task CreateTablesIfNotExistAsync()
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var sql = @"
                CREATE TABLE IF NOT EXISTS Users (
                    UserID SERIAL PRIMARY KEY,
                    Username VARCHAR(255) UNIQUE NOT NULL,
                    PasswordHash VARCHAR(255) NOT NULL,
                    PasswordSalt VARCHAR(255) NOT NULL,
                    Elo INT DEFAULT 1000,
                    Bio TEXT,
                    Image VARCHAR(255)
                );

                CREATE TABLE IF NOT EXISTS Tournaments (
                    TournamentID SERIAL PRIMARY KEY,
                    Username VARCHAR(255) NOT NULL,
                    Data TEXT NOT NULL,
                    StartDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (Username) REFERENCES Users(Username)
                );

                CREATE TABLE IF NOT EXISTS PushUpRecords (
                    RecordID SERIAL PRIMARY KEY,
                    UserID INT NOT NULL,
                    Count INT NOT NULL,
                    Duration INT NOT NULL,
                    RecordDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (UserID) REFERENCES Users(UserID)
                );
            ";

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

    } // <- End of DatabaseSetup class
} // <- End of SportsExerciseBattle.DataAccessLayer.Database namespace
