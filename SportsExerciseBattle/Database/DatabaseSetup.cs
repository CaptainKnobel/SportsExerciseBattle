﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using SportsExerciseBattle.DataAccessLayer.Connection;

namespace SportsExerciseBattle.Database
{
    public class DatabaseSetup
    {
        public DatabaseSetup()
        {
            
        }
        public async Task SetupDatabaseAsync()
        {
            // Ensure database and initial roles exist
            //await EnsureDatabaseExists();

            // Create roles and set permissions
            //await CreateRoleAndGrantPrivileges();

            // Create tables and setup initial structure
            await CreateTablesIfNotExistAsync();

            // Insert initial users
            await InsertInitialUsers();

            // Insert initial data if necessary
            await InsertInitialData();
        }
        private async Task EnsureDatabaseExists()
        {
            using (var conn = DBConnectionManager.Instance.CreateConnection())
            {
                await conn.OpenAsync();
                var sql = @"
                SELECT 'CREATE DATABASE seb_db'
                WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'seb_db');
                ";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    await cmd.ExecuteNonQueryAsync(); // Execute the command to ensure DB exists
                }
            }
        }
        private async Task CreateRoleAndGrantPrivileges()
        {
            using (var conn = DBConnectionManager.Instance.CreateConnection())
            {
                await conn.OpenAsync();
                var sql = @"
                DROP ROLE IF EXISTS seb_admin;
                CREATE ROLE seb_admin WITH LOGIN PASSWORD 'seb_password';
                GRANT ALL PRIVILEGES ON DATABASE seb_db TO seb_admin;
                ";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task CreateTablesIfNotExistAsync()
        {
            using (var conn = DBConnectionManager.Instance.CreateConnection())
            {
                await conn.OpenAsync();
                var sql = @"
                DELETE FROM users WHERE TRUE;

                DROP VIEW IF EXISTS get_stats;
                DROP VIEW IF EXISTS get_score;
                DROP VIEW IF EXISTS get_history;
                DROP TABLE IF EXISTS history CASCADE;
                DROP TABLE IF EXISTS users CASCADE;
                DROP TABLE IF EXISTS tournaments CASCADE;

                CREATE TABLE IF NOT EXISTS users (
                    user_id SERIAL PRIMARY KEY,
                    username VARCHAR(50) NOT NULL UNIQUE,
                    passwordHash VARCHAR(60) NOT NULL,
                    userELO INT DEFAULT 100,
                    userToken VARCHAR(50),
                    bio TEXT DEFAULT '',
                    image TEXT DEFAULT '',
                    profileName VARCHAR(50) DEFAULT ''
                );

                CREATE TABLE IF NOT EXISTS history (
                    history_id SERIAL PRIMARY KEY,
                    fk_user_id INTEGER REFERENCES users(user_id) ON DELETE CASCADE,
                    entryDateTime TIMESTAMP DEFAULT current_timestamp,
                    exerciseType VARCHAR(50) NOT NULL,
                    count INTEGER NOT NULL,
                    duration INTEGER NOT NULL,
                    recordEntry BOOL DEFAULT false
                );

                CREATE TABLE IF NOT EXISTS tournaments (
                    tournament_id SERIAL PRIMARY KEY,
                    tournament_started TIMESTAMP DEFAULT current_timestamp,
                    exercise_type VARCHAR(50) NOT NULL,
                    winner VARCHAR(200) NOT NULL,
                    participant_count INTEGER NOT NULL,
                    is_running BOOLEAN DEFAULT FALSE
                );

                CREATE VIEW get_stats AS
                SELECT u.username, u.userelo, COALESCE(SUM(h.count),0) AS totalcount FROM users AS u
                LEFT JOIN history AS h ON u.user_id = h.fk_user_id
                GROUP BY 1, 2 ORDER BY 2, 3 DESC;

                CREATE VIEW get_score AS
                SELECT u.profileName, u.userelo, COALESCE(SUM(h.count),0) AS totalcount FROM users AS u
                LEFT JOIN history AS h ON u.user_id = h.fk_user_id
                GROUP BY 1, 2 ORDER BY 2, 3 DESC;

                CREATE VIEW get_history AS
                SELECT u.username, h.exerciseType, h.count, h.duration FROM users AS u
                INNER JOIN history AS h ON u.user_id = h.fk_user_id
                ORDER BY 2, 3 DESC;
                
                GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO seb_admin;
                GRANT USAGE ON ALL SEQUENCES IN SCHEMA public TO seb_admin;
                ";

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        private async Task InsertInitialUsers()
        {
            using (var conn = DBConnectionManager.Instance.CreateConnection())
            {
                await conn.OpenAsync();
                var sql = @"
                    INSERT INTO users (username, passwordHash, userELO, userToken, bio, image, profileName)
                    VALUES 
                    ('User1', 'hash1', 100, 'token1', 'bio1', 'image1', 'profile1'),
                    ('User2', 'hash2', 100, 'token2', 'bio2', 'image2', 'profile2');
                ";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        private async Task InsertInitialData()
        {
            using (var conn = DBConnectionManager.Instance.CreateConnection())
            {
                await conn.OpenAsync();
                var sql = @"
                INSERT INTO history (fk_user_id, exerciseType, count, duration, recordEntry) VALUES (1, 'PushUp', 13, 11, false);
                INSERT INTO history (fk_user_id, exerciseType, count, duration, recordEntry) VALUES (1, 'PushUp', 33, 11, false);
                INSERT INTO history (fk_user_id, exerciseType, count, duration, recordEntry) VALUES (1, 'PushUp', 29, 11, false);
                INSERT INTO history (fk_user_id, exerciseType, count, duration, recordEntry) VALUES (2, 'PushUp', 13, 11, false);
                INSERT INTO history (fk_user_id, exerciseType, count, duration, recordEntry) VALUES (2, 'PushUp', 33, 11, false);
                INSERT INTO history (fk_user_id, exerciseType, count, duration, recordEntry) VALUES (2, 'PushUp', 29, 11, false);
                ";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

    } // <- End of DatabaseSetup class
} // <- End of SportsExerciseBattle.DataAccessLayer.Database namespace
