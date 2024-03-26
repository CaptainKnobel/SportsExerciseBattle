using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEB.Services;

namespace SEB.Controllers
{
    public class UserController
    {
        private readonly UserService userService;

        public UserController(UserService userService)
        {
            this.userService = userService;
        }

        public void Run()
        {
            Console.WriteLine("... Starting Up ...");
            // TODO: refactor into "logged-in"/"not logged-in" functions for menu/prompt
            
        }

        private void MenuLoggedIn()
        {
            bool isQuit = false;
            while (!isQuit)
            {
                Console.WriteLine("Menu Options:");
                Console.WriteLine("1. Start Tournament");
                Console.WriteLine("2. Update Profile");
                Console.WriteLine("Q. Quit");

                string? userInput = Console.ReadLine();

                switch(userInput)
                {
                    case "1":
                        // TODO: start a tournament
                        break;
                    case "2":
                        //TODO: Update the user profile
                        break;
                    case "Q":
                    case "q":
                        isQuit = true;
                        // TODO: extra log-out procedure??
                        Console.WriteLine("... Shutting Down ...");
                        break;
                    default:
                        Console.WriteLine("Invalid input! Please try again.");
                        break;

                }
            }
                
            
        }

        private void MenuNotLoggedIn()
        {
            bool isQuit = false;
            while (!isQuit)
            {
                Console.WriteLine("Menu Options:");
                Console.WriteLine("1. Register User");
                Console.WriteLine("2. Login");
                Console.WriteLine("Q. Quit");

                string? userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        RegisterUser();
                        break;
                    case "2":
                        Login();
                        break;
                    case "Q":
                    case "q":
                        isQuit = true;
                        Console.WriteLine("... Shutting Down ...");
                        break;
                    default:
                        Console.WriteLine("Invalid input! Please try again.");
                        break;
                }
            }
        }

        private void RegisterUser()
        {
            // TODO: Implement user registration logic
        }

        private void Login()
        {
            // TODO: Implement user login logic
        }
    }
}