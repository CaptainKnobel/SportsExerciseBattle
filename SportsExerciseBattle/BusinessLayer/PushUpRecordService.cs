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
        private readonly PushUpRecordRepository pushUpRecordRepository;

        public PushUpRecordService(PushUpRecordRepository pushUpRecordRepository)
        {
            this.pushUpRecordRepository = pushUpRecordRepository;
        }

        public void AddPushUpRecord(PushUpRecord record)
        {
            // TODO: Perform additional validation or business logic as needed
            pushUpRecordRepository.AddPushUpRecord(record);
        }

        // what ever other methods for retrieving push-up records, calculating statistics, etc.
    }
}

