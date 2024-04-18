using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsExerciseBattle.Models;
using SportsExerciseBattle.DataAccessLayer;

namespace SportsExerciseBattle.BusinessLayer
{
    public class TournamentService
    {
        private readonly ITournamentRepository _tournamentRepository;

        public TournamentService(ITournamentRepository tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        // Starts a tournament and resolves it by determining a winner and updating ELO scores
        public async Task<bool> StartAndResolveTournament(string initiatorUsername, List<string> participants)
        {
            string tournamentData = "Example Tournament Data"; // This would typically be more detailed and derived from some input

            // Start the tournament
            bool tournamentStarted = await _tournamentRepository.StartUserTournament(initiatorUsername, tournamentData);
            if (!tournamentStarted)
            {
                Console.WriteLine("Error: Tournament could not be started.");
                return false;
            }

            // Simulate determining a winner and updating scores
            string winnerUsername = DetermineWinner(participants);
            int winnerEloChange = participants.Count > 1 ? 2 : 0;  // +2 ELO for winner if more than one participant
            int loserEloChange = -1;  // -1 ELO for losers

            // Update ELO for the winner
            await _tournamentRepository.UpdateEloScores(winnerUsername, winnerEloChange);

            // Update ELO for losers
            foreach (var participant in participants)
            {
                if (participant != winnerUsername)
                {
                    await _tournamentRepository.UpdateEloScores(participant, loserEloChange);
                }
            }

            return true;
        }

        // Helper method to determine the winner based on a simple rule: the first participant in the list
        private string DetermineWinner(List<string> participants)
        {
            // This method would ideally involve more complex logic perhaps involving historical data or scores
            return participants[0];  // Simplistically picking the first as the winner
        }

        // Fetch tournament results
        public async Task<string> GetTournamentDetails(int tournamentId)
        {
            return await _tournamentRepository.GetTournamentResults(tournamentId);
        }
    }
}
