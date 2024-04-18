using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsExerciseBattle.BusinessLayer;

namespace SportsExerciseBattle.Web.Controllers
{
    public class UserController
    {
        private UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        public async Task HandleRegister(StreamWriter writer, string body)
        {
            // Logic for user registration
            Console.WriteLine("Registering user with data: " + body);
            await writer.WriteLineAsync("HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\n\r\nUser registered.");
        }

        public async Task HandleLogin(StreamWriter writer, string body)
        {
            // Logic for user login
            Console.WriteLine("User login with data: " + body);
            await writer.WriteLineAsync("HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\n\r\nUser logged in.");
        }
    }
}