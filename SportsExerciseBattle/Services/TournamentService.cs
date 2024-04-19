using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsExerciseBattle.Models;
using SportsExerciseBattle.DataAccessLayer;

namespace SportsExerciseBattle.Services
{
    public class TournamentService
    {
        private readonly TournamentRepository _tournamentRepository = new TournamentRepository();

        public TournamentService()
        {
        }

        // Method to start a new tournament
        public async Task<bool> StartTournament(string organizerUsername, string tournamentDetails)
        {
            return await _tournamentRepository.StartUserTournament(organizerUsername, tournamentDetails);
        }

        // Method to conclude a tournament
        public async Task<bool> ConcludeTournament(int tournamentId, string winnerUsername)
        {
            int result = await _tournamentRepository.RecordTournamentOutcome(winnerUsername, new[] { "otherParticipant1", "otherParticipant2" });
            // Assume that a result of 1 or greater indicates success
            return result > 0;
        }

        // Fetch details of a specific tournament
        public async Task<string> GetTournamentDetails(int tournamentId)
        {
            return await _tournamentRepository.GetTournamentResults(tournamentId);
        }
    }
}