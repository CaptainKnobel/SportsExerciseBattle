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
        User CreateUser(User user);
        User GetUserByUsername(string username);
        bool UpdateUser(User user);
        bool DeleteUser(string username);
        /*
        Task<User?> GetUserByUsername(string username);
        Task AddUser(string username, string password, string name, string bio, string image, int elo);
        Task<bool> VerifyPassword(string username, string password);
        Task<UserStats> GetUserStats(string username);
        Task<List<ScoreboardEntry>> GetScoreboardData();
        */
    }
}
