using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Models
{
    public class Entry
    {
        public string Username { get; set; } = "";
        public string EntryName { get; set; } = "";
        public int Count { get; set; } = 0;
        public int DurationInSeconds { get; set; } = 0;
        public DateTime Timestamp { get; set; } = DateTime.Now;

    }
}
