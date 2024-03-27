using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEB.Models;
using SEB.Services;

namespace SEB.Controllers
{
    public class TournamentController
    {
        private readonly TournamentService tournamentService;

        public TournamentController(TournamentService tournamentService)
        {
            this.tournamentService = tournamentService;
        }

        public void Run()
        {
            // Create a new tournament instance with the current time as the start time
            var tournament = new Tournament { StartTime = DateTime.Now };

            // Start the tournament
            tournamentService.StartTournament(tournament);
        }
    }
}
