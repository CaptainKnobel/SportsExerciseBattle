using SportsExerciseBattle.Datatypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Models
{
    public class TournamentResult
    {
        public ExerciseType ExerciseType { get; set; }
        public List<ParticipantScore> Participants { get; set; } = new List<ParticipantScore>();
    }

    public class ParticipantScore
    {
        public required string Username { get; set; }
        public int Score { get; set; }
    }
}
