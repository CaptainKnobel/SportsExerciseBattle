using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsExerciseBattle.Models;
using SportsExerciseBattle.Web.HTTP;

namespace SportsExerciseBattle.DataAccessLayer
{
    public interface IHistoryRepository
    {
        void AddEntry(string username, Entry entry, HttpResponse response);
        List<Entry> GetEntries(string username);
        
        /*
        Task AddEntry(int userId, string exerciseType, int count, int duration);
        Task<List<HistoryEntry>> GetEntries(int userId);
        */
    }
}