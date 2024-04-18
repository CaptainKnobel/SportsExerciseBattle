using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsExerciseBattle.Models;
using SportsExerciseBattle.DataAccessLayer;

namespace SportsExerciseBattle.BusinessLayer
{
    public class PushUpRecordService
    {
        private readonly PushUpRecordRepository _pushUpRecordRepository;

        public PushUpRecordService(PushUpRecordRepository pushUpRecordRepository)
        {
            _pushUpRecordRepository = pushUpRecordRepository;
        }

        public async Task<bool> AddPushUpRecord(string username, int count, TimeSpan duration)
        {
            if (string.IsNullOrWhiteSpace(username) || count <= 0 || duration <= TimeSpan.Zero)
            {
                return false; // Ensure valid input parameters
            }
            await _pushUpRecordRepository.AddPushUpRecord(username, count, duration);
            return true;
        }
    }
}

