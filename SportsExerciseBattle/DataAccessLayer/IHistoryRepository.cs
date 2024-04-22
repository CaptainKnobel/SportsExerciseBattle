using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using SportsExerciseBattle.Models;
using SportsExerciseBattle.DataAccessLayer.Connection;

namespace SportsExerciseBattle.DataAccessLayer
{
    public interface IHistoryRepository
    {
        Task<List<HistoryEntry>> GetEntries(string username);
        Task AddEntry(string username, HistoryEntry entry);
    }
}
