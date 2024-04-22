// See https://aka.ms/new-console-template for more information

using System;
using System.Net;
using Npgsql;
using SportsExerciseBattle.Models;
using SportsExerciseBattle.BusinessLayer;
using SportsExerciseBattle.DataAccessLayer;
using SportsExerciseBattle.DataAccessLayer.Connection;
using SportsExerciseBattle.Database;
using SportsExerciseBattle.Web.HTTP;
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
                // Setup database with the new refactored DatabaseSetup that does not require a connection string passed
                var dbSetup = new DatabaseSetup();
                await dbSetup.SetupDatabaseAsync();
                Console.WriteLine("Database setup complete.");

                // Start the HTTP server with endpoints
                HttpServer httpServer = new HttpServer(IPAddress.Any, 10001);
                httpServer.RegisterEndpoint("users", new UsersEndpoint());
                httpServer.RegisterEndpoint("sessions", new SessionsEndpoint());
                httpServer.RegisterEndpoint("stats", new StatsEndpoint());
                httpServer.RegisterEndpoint("score", new ScoresEndpoint());
                httpServer.RegisterEndpoint("history", new HistoryEndpoint());
                httpServer.RegisterEndpoint("tournament", new TournamentEndpoint());
                httpServer.RegisterEndpoint("ratio", new TournamentStatsEndpoint());
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