using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SportsExerciseBattle.Models
{
    public class Tournament
    {
        private static readonly Lazy<Tournament> LazyInstance = new Lazy<Tournament>(() => new Tournament());

        private Tournament() { }
        public static Tournament Instance => LazyInstance.Value;
        public bool IsRunning { get; set; } = false;
        public List<string> Participants { get; set; } = new List<string>();
        public List<string> LeadingUsers { get; set; } = new List<string>();
        public DateTime StartTime { get; set; } = DateTime.MinValue;
        public List<string> Log { get; set; } = new List<string>();
    }
}
