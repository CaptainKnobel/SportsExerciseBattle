using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Models
{
    public class Scoreboard
    {
        public List<ScoreboardEntry> Entries { get; set; } = new List<ScoreboardEntry>();
    }
}
