using SportsExerciseBattle.Models;
using SportsExerciseBattle.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Web.Endpoints
{
    public static class TournamentHelper
    {
        public static void ManageTournament()
        {
            var tournament = Tournament.Instance;
            if (tournament.IsRunning == false)
            {
                var tournamentManager = new TournamentManager();
                tournamentManager.StartTournament();
                tournament.Log.Add(tournament.StartTime + "Arena is open! Join the fight quickly!");
            }
            else
            {
                tournament.Log.Add(DateTime.Now + "Challenge accepted! A Competitor(Entry) joined the tournament");
            }
        }
    }
}
