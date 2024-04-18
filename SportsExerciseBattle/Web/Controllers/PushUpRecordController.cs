using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SportsExerciseBattle.BusinessLayer;

namespace SportsExerciseBattle.Web.Controllers
{
    public class PushUpRecordController
    {
        private readonly PushUpRecordService _pushUpRecordService;

        public PushUpRecordController(PushUpRecordService pushUpRecordService)
        {
            _pushUpRecordService = pushUpRecordService;
        }

        public async Task HandleAddPushUpRecord(StreamWriter writer, string body)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var request = JsonSerializer.Deserialize<AddPushUpRecordRequest>(body, options);
            if (request == null || string.IsNullOrWhiteSpace(request.Username) || request.Count <= 0 || request.Duration <= TimeSpan.Zero)
            {
                await writer.WriteLineAsync("HTTP/1.1 400 Bad Request\r\nContent-Type: text/plain\r\n\r\nInvalid push-up record data.");
                return;
            }

            bool success = await _pushUpRecordService.AddPushUpRecord(request.Username, request.Count, request.Duration);
            if (success)
                await writer.WriteLineAsync("HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\n\r\nPush-up record added successfully.");
            else
                await writer.WriteLineAsync("HTTP/1.1 500 Internal Server Error\r\nContent-Type: text/plain\r\n\r\nFailed to add push-up record.");
        }
    }

    public class AddPushUpRecordRequest
    {
        public string Username { get; set; } = "";
        public int Count { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
