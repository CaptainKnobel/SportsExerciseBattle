using SportsExerciseBattle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsExerciseBattle.DataAccessLayer;
using SportsExerciseBattle.Web.HTTP;
using SportsExerciseBattle.Services;
using System.Timers;
using Timer = System.Timers.Timer;

namespace SportsExerciseBattle.BusinessLayer
{
    public class TournamentManager
    {
        private Timer tournamentTimer;
        private ITournamentRepository tournamentRepo;

        public TournamentManager(ITournamentRepository repository)
        {
            tournamentRepo = repository;
            tournamentTimer = new Timer(120000); // 2 minutes
            tournamentTimer.Elapsed += OnTournamentElapsed;
            tournamentTimer.AutoReset = false;
        }

        public void StartTournament()
        {
            var tournament = Tournament.Instance;
            tournament.IsRunning = true;
            tournament.StartTime = DateTime.Now;
            tournament.Participants = tournamentRepo.GetParticipants(tournament);
            tournamentTimer.Start();
        }

        private void OnTournamentElapsed(object sender, ElapsedEventArgs e)
        {
            EndTournament();
        }

        public void EndTournament()
        {
            var tournament = Tournament.Instance;
            tournamentTimer.Stop();
            tournament.IsRunning = false;
            bool isDraw = tournament.LeadingUsers.Count > 1;
            tournamentRepo.UpdateElo(tournament, isDraw);
            ResetTournamentState(tournament);
        }

        private void ResetTournamentState(Tournament tournament)
        {
            tournament.Participants.Clear();
            tournament.LeadingUsers.Clear();
        }
    }
}
