using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Models
{
    public class Stats
    {
        public string Username { get; set; } = "";
        public string Name { get; set; } = "";
        public int Elo { get; set; } = 100;
        public int Count { get; set; } = 0;
    }
}
