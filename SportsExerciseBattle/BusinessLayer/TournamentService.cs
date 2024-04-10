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
        private readonly TournamentRepository tournamentRepository;

        public TournamentService(TournamentRepository tournamentRepository)
        {
            this.tournamentRepository = tournamentRepository;
        }

        public void StartTournament(Tournament tournament)
        {
            // TODO: Perform additional logic before starting the tournament
            tournamentRepository.StartTournament(tournament);
        }

        // what ever other methods for managing tournaments, retrieving tournament data, etc.
    }
}
