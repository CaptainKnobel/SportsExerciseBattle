using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Models
{
    public class HistoryEntry
    {
        public int HistoryId { get; set; }
        public int UserId { get; set; }
        public string ExerciseType { get; set; }
        public int Count { get; set; }
        public int Duration { get; set; } // Duration in seconds
        public DateTime EntryDateTime { get; set; }
    }
}
