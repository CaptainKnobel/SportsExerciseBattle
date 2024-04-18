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
        private readonly TournamentRepository _tournamentRepository;

        public TournamentService(TournamentRepository tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public async Task<bool> StartAndResolveTournament(string initiatorUsername, List<string> participants)
        {
            if (string.IsNullOrWhiteSpace(initiatorUsername) || participants == null || participants.Count == 0)
            {
                return false; // Early exit if initiator or participants are not properly defined
            }

            var winnerUsername = participants.OrderByDescending(u => u.Length).FirstOrDefault() ?? throw new InvalidOperationException("No participants provided.");
            var result = await _tournamentRepository.RecordTournamentOutcome(winnerUsername, participants);
            return result > 0;
        }
    }
}
