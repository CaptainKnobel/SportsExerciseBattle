using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using SportsExerciseBattle.Models;

namespace SportsExerciseBattle.DataAccessLayer
{
    public class UserRepository
    {
        private readonly string connectionString;

        public UserRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void AddUser(User user)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                // TODO: Perform SQL insert operation to add user to database
            }
        }

        public User GetUserByUsername(string username)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                // TODO: Perform SQL select operation to retrieve user by username from database
                // TODO: Construct and return a User object
                return null; // TODO: Placeholder, to be replaced with actual implementation
            }
        }

        // TODO: what ever other methods I need
    }
}
