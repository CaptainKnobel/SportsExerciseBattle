using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            // TODO: Implement tournament
        }
    }
}