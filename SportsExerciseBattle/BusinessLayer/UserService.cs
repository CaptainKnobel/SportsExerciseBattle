using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsExerciseBattle.Models;
using SportsExerciseBattle.DataAccessLayer;

namespace SportsExerciseBattle.BusinessLayer
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task RegisterUser(string username, string password, string name, string bio, string image, int elo)
        {
            // Create a User object from the provided data
            User newUser = new User
            {
                Username = username,
                Name = name,
                Bio = bio,
                Image = image,
                Elo = elo
            };

            // Call AddUser with the new User object
            await _userRepository.AddUser(username, password, name, bio, image, elo);
        }

        public async Task<User> Login(string username, string password)
        {
            if (await _userRepository.VerifyPassword(username, password))
            {
                return await _userRepository.GetUserByUsername(username);
            }
            throw new Exception("Invalid credentials");
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
