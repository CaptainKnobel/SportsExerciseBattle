// See https://aka.ms/new-console-template for more information

using System;
using System.Net;
using Npgsql;
using SportsExerciseBattle.Models;
using SportsExerciseBattle.BusinessLayer;
using SportsExerciseBattle.DataAccessLayer;
using SportsExerciseBattle.DataAccessLayer.Database;
using SportsExerciseBattle.Web.HTTP;
using SportsExerciseBattle.Web.Controllers;
using SportsExerciseBattle.Web.Endpoints;

namespace SportsExerciseBattle
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Program Start ...");
            Console.WriteLine("=*=*=*=[ Sports Exercise Battle Server ]=*=*=*=");

            try
            {
                // Initialize database connection string
                //string connectionString = "Host=localhost;Port=5432;Database=seb_db;Username=seb_admin;Password=seb_password;Persist Security Info=True; Include Error Detail=True";

                // Setup Database
                var dbSetup = new DatabaseSetup(RepositoryConnection.connectionString);
                await dbSetup.CreateTablesIfNotExistAsync();
                Console.WriteLine("Database setup complete.");
                /*
                // Initialize repositories
                var userRepository = new UserRepository(connectionString);
                var pushUpRecordRepository = new PushUpRecordRepository(connectionString);
                var tournamentRepository = new TournamentRepository(connectionString);

                // Initialize services
                var userService = new UserService(userRepository);
                var tournamentService = new TournamentService(tournamentRepository, userRepository);
                var pushUpRecordService = new PushUpRecordService(pushUpRecordRepository);
                var statsService = new StatsService(userRepository);

                // Initialize controllers
                var userController = new UserController(userService);
                var tournamentController = new TournamentController(tournamentService);
                var pushUpRecordController = new PushUpRecordController(pushUpRecordService);
                var statsController = new StatsController(statsService);
                
                // Router setup
                var router = new Router(userController, tournamentController, pushUpRecordController, statsController);

                // Start the HTTP server
                await HttpServer.StartServer(10001, router);
                Console.WriteLine("Server is running...");
                */


                // ===== I. Start the HTTP-Server =====
                HttpServer httpServer = new HttpServer(IPAddress.Any, 10001);
                //Start Endpoints
                httpServer.RegisterEndpoint("users", new UsersEndpoint());
                httpServer.RegisterEndpoint("sessions", new SessionsEndpoint());
                httpServer.RegisterEndpoint("stats", new StatsEndpoint());
                httpServer.RegisterEndpoint("score", new ScoresEndpoint());
                httpServer.RegisterEndpoint("history", new HistoryEndpoint());
                httpServer.RegisterEndpoint("tournament", new TournamentEndpoint());
                httpServer.RegisterEndpoint("ratio", new TournamentStatsEndpoint());
                //Run the server
                httpServer.Run();


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


/*
docker run --name seb_db -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -p 5432:5432 -v pgdata:/var/lib/postgresql/data postgres
*/