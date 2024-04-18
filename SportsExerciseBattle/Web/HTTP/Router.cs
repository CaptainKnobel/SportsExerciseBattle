using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SportsExerciseBattle.Web.Controllers;

namespace SportsExerciseBattle.Web.HTTP
{
    public class Router
    {
        private UserController _userController;
        private TournamentController _tournamentController;
        private PushUpRecordController _pushUpRecordController;
        private StatsController _statsController;

        public Router(UserController userController, TournamentController tournamentController, PushUpRecordController pushUpRecordController, StatsController statsController)
        {
            _userController = userController;
            _tournamentController = tournamentController;
            _pushUpRecordController = pushUpRecordController;
            _statsController = statsController;
        }

        public async Task RouteRequest(StreamWriter writer, string method, string url, string body)
        {
            switch (url.ToLower())
            {
                case "/register":
                    await _userController.HandleRegister(writer, body);
                    break;
                case "/login":
                    await _userController.HandleLogin(writer, body);
                    break;
                case "/starttournament":
                    await _tournamentController.HandleStartTournament(writer, body);
                    break;
                case "/addpushuprecord":
                    await _pushUpRecordController.HandleAddPushUpRecord(writer, body);
                    break;
                case "/userstats":
                    await _statsController.HandleGetUserStats(writer, ExtractUsername(body));
                    break;
                case "/scoreboard":
                    await _statsController.HandleGetScoreboard(writer);
                    break;
                default:
                    await SendNotFoundResponse(writer);
                    break;
            }
        }
        private static async Task SendNotFoundResponse(StreamWriter writer)
        {
            await writer.WriteLineAsync("HTTP/1.1 404 Not Found\r\nContent-Type: text/plain\r\n\r\nEndpoint not found.");
        }
        private static async Task SendMethodNotAllowed(StreamWriter writer)
        {
            await writer.WriteLineAsync("HTTP/1.1 405 Method Not Allowed\r\nContent-Type: text/plain\r\n\r\nMethod Not Allowed.");
        }
        private string ExtractUsername(string body)
        {
            var userDetail = JsonSerializer.Deserialize<Dictionary<string, string>>(body);
            return userDetail != null && userDetail.ContainsKey("username") ? userDetail["username"] : string.Empty;
        }

    }
}
