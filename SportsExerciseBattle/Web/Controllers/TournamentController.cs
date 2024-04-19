using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SportsExerciseBattle.Models;
using SportsExerciseBattle.Services;

namespace SportsExerciseBattle.Web.Controllers
{
    public class TournamentController
    {
        private readonly TournamentService _tournamentService;

        public TournamentController(TournamentService tournamentService)
        {
            _tournamentService = tournamentService;
        }

        public async Task HandleStartTournament(StreamWriter writer, string body)
        {
            // Deserializing the request body to extract tournament participants
            var participants = JsonSerializer.Deserialize<List<string>>(body);
            if (participants == null || participants.Count == 0)
            {
                await writer.WriteLineAsync("HTTP/1.1 400 Bad Request\r\nContent-Type: text/plain\r\n\r\nInvalid tournament data.");
                return;
            }

            // Starting the tournament and resolving the results
            bool success = await _tournamentService.StartAndResolveTournament(participants.First(), participants);
            if (!success)
            {
                await writer.WriteLineAsync("HTTP/1.1 500 Internal Server Error\r\nContent-Type: text/plain\r\n\r\nFailed to start or resolve the tournament.");
                return;
            }

            await writer.WriteLineAsync("HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\n\r\nTournament successfully started and resolved.");
        }

    }
}
