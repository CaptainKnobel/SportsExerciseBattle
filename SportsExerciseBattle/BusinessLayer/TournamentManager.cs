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
        private static readonly int TournamentDuration = 120000; // 2 minutes in milliseconds
        private Timer tournamentTimer { get; set; }
        private TournamentDAO tournamentDAO = new TournamentDAO();

        public TournamentManager()
        {
            tournamentTimer = new Timer(TournamentDuration);
            tournamentTimer.Elapsed += OnTournamentElapsed;
            tournamentTimer.AutoReset = false; // Ensures the timer only runs once per start
        }

        public void StartTournament()
        {
            var tournament = Tournament.Instance;
            tournament.IsRunning = true;
            tournament.StartTime = DateTime.Now;
            tournamentTimer.Start();
        }

        private void OnTournamentElapsed(object sender, ElapsedEventArgs e)
        {
            EndTournament();
        }

        public void EndTournament()
        {
            var tournament = Tournament.Instance;
            tournamentTimer.Stop(); // Stop the timer to clean up resources
            tournament.IsRunning = false;

            tournament.Log.Add(DateTime.Now + " The fight is over! Tournament ended!");

            CalculateWinnerAndUpdateElo();

            // Clean up for next tournament
            ResetTournamentState(tournament);
        }

        private void CalculateWinnerAndUpdateElo()
        {
            // Assuming these DAO methods manage database interactions for tournament logic
            tournamentDAO.GetParticipants(); // Potential to store result and use below
            tournamentDAO.GetLeaders();      // Potential to store result and use below
            tournamentDAO.UpdateElo();
            tournamentDAO.UpdateTournamentStats();
        }

        private void ResetTournamentState(Tournament tournament)
        {
            tournament.LeadingUsers.Clear();
            tournament.Participants.Clear();
        }
    }
}
