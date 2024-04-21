using SportsExerciseBattle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.DataAccessLayer
{
    public interface ITournamentStatsRepository
    {
        TournamentStats GetTournamentStats(string username);
        void AddToCount(string username, int count);
    }
}
