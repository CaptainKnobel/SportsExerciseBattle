using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEB.Models;
using SEB.Services;

namespace SEB.Services
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

