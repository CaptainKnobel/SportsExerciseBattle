using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SportsExerciseBattle.Datatypes;

namespace SportsExerciseBattle.Models
{
    public class Tournament
    {
        public int TournamentId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsRunning { get; set; } = false;
        public ExerciseType ExerciseType { get; set; }

    }
}
