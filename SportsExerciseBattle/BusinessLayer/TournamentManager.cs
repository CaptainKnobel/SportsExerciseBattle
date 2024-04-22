using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;
using SportsExerciseBattle.Models;
using SportsExerciseBattle.Datatypes;
using Timer = System.Timers.Timer;
using SportsExerciseBattle.DataAccessLayer;

namespace SportsExerciseBattle.BusinessLayer
{
    public class TournamentManager
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHistoryRepository _historyRepository;
        private Dictionary<int, Timer> _tournamentTimers = new Dictionary<int, Timer>();

        public TournamentManager()
        {
            _tournamentRepository = new TournamentRepository();
            _userRepository = new UserRepository();
            _historyRepository = new HistoryRepository();
        }

        public void StartTournament(int tournamentId, ExerciseType exerciseType)
        {
            if (_tournamentTimers.ContainsKey(tournamentId))
            {
                Console.WriteLine($"Tournament {tournamentId} is already running.");
                return;
            }

            var timer = new Timer(120000); // Set duration for the tournament
            timer.Elapsed += async (sender, e) => await EndTournament(tournamentId); // Asynchronous lambda to handle the timer elapsed event
            timer.AutoReset = false;
            timer.Start();

            _tournamentTimers[tournamentId] = timer;

            var tournament = new Tournament
            {
                TournamentId = tournamentId,
                ExerciseType = exerciseType,
                StartTime = DateTime.Now,
                IsRunning = true
            };

            // Await with an asynchronous call to StartUserTournament
            Task.Run(() => _tournamentRepository.StartUserTournament("system", JsonSerializer.Serialize(tournament)))
                .GetAwaiter().GetResult(); // Properly wait for the task to complete if synchronous execution is needed
            Console.WriteLine($"{DateTime.Now} - {exerciseType} tournament {tournamentId} started.");

            _historyRepository.AddEntry("system", new HistoryEntry
            {
                EntryName = $"{exerciseType} Tournament Started",
                Count = 0,
                DurationInSeconds = 0,
                Timestamp = DateTime.Now
            }).GetAwaiter().GetResult(); // Ensures this completes synchronously
        }

        private async Task EndTournament(int tournamentId)
        {
            if (!_tournamentTimers.TryGetValue(tournamentId, out var timer))
            {
                return;
            }

            timer.Stop();
            _tournamentTimers.Remove(tournamentId);

            var tournament = new Tournament
            {
                TournamentId = tournamentId,
                IsRunning = false,
                EndTime = DateTime.Now
            };

            await _tournamentRepository.UpdateTournamentAsync(tournament);
            await CalculateWinnerAndUpdateElo(tournamentId);
            Console.WriteLine($"{DateTime.Now} - Tournament {tournamentId} has ended.");
        }

        private async Task CalculateWinnerAndUpdateElo(int tournamentId)
        {
            var results = await _tournamentRepository.GetTournamentResults(tournamentId);
            if (results == null)
            {
                Console.WriteLine("No results found for this tournament.");
                return;
            }

            var resultData = JsonSerializer.Deserialize<TournamentResult>(results);
            if (resultData == null || resultData.Participants == null || !resultData.Participants.Any())
            {
                Console.WriteLine("Failed to parse tournament results or no participants.");
                return;
            }

            // Determine winner based on the exercise type
            Dictionary<string, int> scores = new Dictionary<string, int>();
            foreach (var participant in resultData.Participants)
            {
                if (!scores.ContainsKey(participant.Username))
                    scores[participant.Username] = 0;

                scores[participant.Username] += participant.Score;
            }

            // Find the highest score and list all users with this score
            int maxScore = scores.Values.Max();
            var winners = scores.Where(p => p.Value == maxScore).Select(p => p.Key).ToList();

            // If more than one winner, it's a draw
            if (winners.Count > 1)
            {
                foreach (var winner in winners)
                {
                    await _tournamentRepository.UpdateEloScores(winner, 1); // +1 ELO for draw
                }
                Console.WriteLine($"Draw between: {String.Join(", ", winners)}. Each gets +1 ELO.");
            }
            else if (winners.Count == 1)
            {
                string winner = winners.First();
                await _tournamentRepository.UpdateEloScores(winner, 2); // +2 ELO for the winner
                Console.WriteLine($"Winner: {winner} with score {maxScore}. +2 ELO awarded.");
            }

            // Deduct 1 ELO from all non-winners
            var losers = scores.Keys.Except(winners).ToList();
            foreach (var loser in losers)
            {
                await _tournamentRepository.UpdateEloScores(loser, -1); // -1 ELO for non-winners
            }
        }
    }
}
