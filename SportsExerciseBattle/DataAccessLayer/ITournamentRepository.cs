using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsExerciseBattle.Models;

namespace SportsExerciseBattle.DataAccessLayer
{
    public interface ITournamentRepository
    {
        Task<bool> StartUserTournament(string username, string tournamentData);
        Task<bool> UpdateEloScores(string username, int eloChange);
        Task<string?> GetTournamentResults(int tournamentId);
        Task<int> RecordTournamentOutcome(string winnerUsername, IEnumerable<string> participantUsernames);

    }
}
