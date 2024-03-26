// See https://aka.ms/new-console-template for more information

using System;
using Npgsql;
using SEB.Controllers;
using SEB.Models;
using SEB.Services;

namespace SEB
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
                string connectionString = "my_postgresql_connection_string";

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
} // <- End of SEB namesspace