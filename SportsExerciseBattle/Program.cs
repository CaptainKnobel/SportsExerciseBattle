﻿// See https://aka.ms/new-console-template for more information

using System;
using Npgsql;
using SportsExerciseBattle.Models;
using SportsExerciseBattle.BusinessLayer;
using SportsExerciseBattle.BusinessLayer.Menus;
using SportsExerciseBattle.DataAccessLayer;

namespace SportsExerciseBattle
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Program Start ...");
            Console.WriteLine("=*=*=*=[ Sports Exercise Battle Server ]=*=*=*=");

            try
            {
                // Initialize database connection string
                string connectionString = "Host=localhost;Database=mydb;Username=postgres;Password=postgres;Persist Security Info=True; Include Error Detail=True";

                // Initialize repositories
                var userRepository = new UserRepository(connectionString);
                var pushUpRecordRepository = new PushUpRecordRepository(connectionString);
                var tournamentRepository = new TournamentRepository(connectionString);

                // Initialize services
                var userService = new UserService(userRepository);
                var pushUpRecordService = new PushUpRecordService(pushUpRecordRepository);
                var tournamentService = new TournamentService(tournamentRepository);

                // Initialize controllers
                var userController = new UserController(userService);
                var tournamentController = new TournamentController(tournamentService);
                var profileController = new ProfileController(userService);

                // Run the application (userController is master)
                userController.Run();
            }
            catch (NpgsqlException ex)
            {
                // Handle PostgreSQL exceptions separately
                Console.WriteLine("A PostgreSQL exception occurred: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Generic exception handling for any other unexpected errors
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            finally
            {
                Console.WriteLine("... Program End");
            }
        } // <- End of Main function
    } // <- End of Program class
} // <- End of SportsExerciseBattle namesspace


// docker run --name SEB_db -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -p 5432:5432 -v pgdata:/var/lib/postgresql/data postgres