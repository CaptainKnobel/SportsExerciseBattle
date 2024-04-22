using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsExerciseBattle.Models;

namespace SportsExerciseBattle.DataAccessLayer
{
    public interface IUserRepository
    {
        Task<bool> VerifyPassword(string username, string password);
        Task AddUser(string username, string password, string name, string bio, string image, int elo);
        Task<User?> GetUserByUsername(string username);
        Task UpdateUser(User user);
        Task<UserStats> GetUserStats(string username);
        Task<List<ScoreboardEntry>> GetScoreboardData();
    }
}
