using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsExerciseBattle.Models;

namespace SportsExerciseBattle.DataAccessLayer
{
    public interface IHistoryRepository
    {
        Task AddEntry(int userId, string exerciseType, int count, int duration);
        Task<List<HistoryEntry>> GetEntries(int userId);

    }
}