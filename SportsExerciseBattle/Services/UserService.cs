using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEB.Models;
using SEB.Services;

namespace SEB.Services
{
    public class UserService
    {
        private readonly UserRepository userRepository;

        public UserService(UserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public void RegisterUser(User user)
        {
            // TODO:
            // Validate user data (e.g., check if username is available, syntactic requirements, etc.)
            // Hash the user's password before storing it in the database (because d'uh)
            // What ever else that comes to my mind
            userRepository.AddUser(user);
        }

        public User Login(string username, string password)
        {
            // TODO:
            // Validate credentials
            // Retrieve user from database and return
            // Blackmail client and extort them for all their money if credentials are wrong
            return userRepository.GetUserByUsername(username);
        }
        public void UpdateUser(User user)
        {
            // idk yet
            // check if edited and client are same user ig
            // ask for password again?
        }
        public void DeleteUser(User user)
        {
            // should i do this?
            // access restrictions?
        }

        // TODO: Implement other methods for profile updates, password changes, etc.
    }
}
