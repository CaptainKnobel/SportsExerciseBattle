// See https://aka.ms/new-console-template for more information

using System;
using Npgsql;
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

            bool isQuit = false;
            int idiotFilter = 0;
            while(!isQuit || idiotFilter == 4)
            {
                Console.WriteLine("Menu Options:");
                Console.WriteLine("1. Register User");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Start Tournament");
                Console.WriteLine("4. Update Profile");
                Console.WriteLine("Q. Quit");
                string? userInput = Console.ReadLine();
            
                if (string.Equals(userInput, "1", StringComparison.OrdinalIgnoreCase))
                {
                    // register a user
                }
                else if(string.Equals(userInput, "2", StringComparison.OrdinalIgnoreCase))
                {
                    // Login user
                }
                else if(string.Equals(userInput, "3", StringComparison.OrdinalIgnoreCase))
                {
                    // start tournament ... if logged in
                }
                else if(string.Equals(userInput, "4", StringComparison.OrdinalIgnoreCase))
                {
                    // edit the logged in user's profile
                }
                else if(string.Equals(userInput, "Q", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Shutting Down ...");
                }
                else
                {
                    Console.WriteLine("Invalid input! Please try again.");
                    idiotFilter++;
                }
            }
            

            Console.WriteLine("... Program End");
        }
    }
}