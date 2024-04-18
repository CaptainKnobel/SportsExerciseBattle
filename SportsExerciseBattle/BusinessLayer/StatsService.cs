using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SportsExerciseBattle.Models;
using SportsExerciseBattle.DataAccessLayer;

namespace SportsExerciseBattle.BusinessLayer
{
    public class StatsService
    {
        private readonly IUserRepository _userRepository;

        public StatsService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Scoreboard> GetScoreboard()
        {
            var entries = await _userRepository.GetScoreboardData();
            return new Scoreboard { Entries = entries };
        }

        public async Task<UserStats> GetUserStats(string username)
        {
            return await _userRepository.GetUserStats(username);
        }
    }
}
