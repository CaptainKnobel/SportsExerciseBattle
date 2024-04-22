using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Models
{
    public class HistoryEntry
    {
        public string Username { get; set; }        // User who made the entry
        public string EntryName { get; set; }       // Name or type of the entry
        public int Count { get; set; }              // A numeric value associated with the entry (e.g., score, amount)
        public int DurationInSeconds { get; set; }  // Duration of the event in seconds
        public DateTime Timestamp { get; set; }     // When the entry was made

        public HistoryEntry()
        {
            Username = string.Empty;
            EntryName = string.Empty;
        }

        public HistoryEntry(string username, string entryName, int count, int durationInSeconds, DateTime timestamp)
        {
            Username = username;
            EntryName = entryName;
            Count = count;
            DurationInSeconds = durationInSeconds;
            Timestamp = timestamp;
        }
    }
}
