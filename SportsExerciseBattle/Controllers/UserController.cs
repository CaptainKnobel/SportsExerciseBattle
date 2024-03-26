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
        private bool isLoggedIn = false;

        public UserController(UserService userService)
        {
            this.userService = userService;
        }

        public void Run()
        {
            Console.WriteLine("... Starting Up ...");
            bool isQuit = false;
            while(!isQuit)
            {
                if(isLoggedIn)
                {
                    isQuit = MenuLoggedIn();
                }
                else
                {
                    isQuit = MenuNotLoggedIn ();
                }
            }
            Console.WriteLine("... Shutting Down ...");
        }

        private bool MenuLoggedIn()
        {
            Console.WriteLine("Menu Options:");
            Console.WriteLine("1. Start Tournament");
            Console.WriteLine("2. Update Profile");
            Console.WriteLine("L. Logout");
            Console.WriteLine("Q. Quit");

            string? userInput = Console.ReadLine()?.Trim();

            switch(userInput)
            {
                case "1":
                    StartTournament();
                    return false; //continue loop
                case "2":
                    UpdateProfile();
                    return false;
                case "l":
                    Logout();
                    return false;
                case "Q":
                case "q":
                    Logout();
                    return true; //end loop
                default:
                    Console.WriteLine("Invalid input! Please try again.");
                    return false;
            }
        }

        private bool MenuNotLoggedIn()
        {
            Console.WriteLine("Menu Options:");
            Console.WriteLine("1. Register User");
            Console.WriteLine("2. Login");
            Console.WriteLine("Q. Quit");

            string? userInput = Console.ReadLine()?.Trim();

            switch (userInput)
            {
                case "1":
                    RegisterUser();
                    return false;
                case "2":
                    Login();
                    return false;
                case "Q":
                case "q":
                    return true;
                default:
                    Console.WriteLine("Invalid input! Please try again.");
                    return false;
            }
        }

        private void RegisterUser()
        {
            // TODO: Implement user registration logic
            //       Must make the user logged in when successfully registered
            //       Must lead to MenuLoggedIn() on successful register
            isLoggedIn = true; // placeholder!
        }

        private void Login()
        {
            // TODO: Implement user login logic
            //       Must lead to MenuLoggedIn() on successful login
            isLoggedIn = true; // placeholder!
        }

        private void Logout()
        {
            // TODO: Logout function
            Console.WriteLine("Logged out successfully!");
            isLoggedIn = false; // placeholder!
        }

        private void StartTournament()
        {
            Console.WriteLine("Tournament started!");
            // do the tournament
        }

        private void UpdateProfile()
        {
            Console.WriteLine("Profile updated!");
            // let the user edit their profile
        }
    }
}