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

            Console.WriteLine("... Program End");
        }
    }
}