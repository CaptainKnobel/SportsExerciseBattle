using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SportsExerciseBattle.BusinessLayer;

namespace SportsExerciseBattle.Web.Controllers
{
    public class StatsController
    {
        private readonly StatsService _statsService;

        public StatsController(StatsService statsService)
        {
            _statsService = statsService;
        }

        public async Task HandleGetUserStats(StreamWriter writer, string username)
        {
            var stats = await _statsService.GetUserStats(username);
            await writer.WriteLineAsync($"HTTP/1.1 200 OK\r\nContent-Type: application/json\r\n\r\n{JsonSerializer.Serialize(stats)}");
        }

        public async Task HandleGetScoreboard(StreamWriter writer)
        {
            var scoreboard = await _statsService.GetScoreboard();
            await writer.WriteLineAsync($"HTTP/1.1 200 OK\r\nContent-Type: application/json\r\n\r\n{JsonSerializer.Serialize(scoreboard)}");
        }
    }
}
