using Npgsql;
using SportsExerciseBattle.DataAccessLayer.Connection;
using SportsExerciseBattle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.DataAccessLayer
{
    public class ExerciseRepository
    {
        private readonly string _connectionString = DatabaseConnection.GetConnectionString();

        public bool AddExerciseEntry(int userId, ExerciseType exerciseType, int count, int duration)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new NpgsqlCommand("INSERT INTO history (user_id, exercise_type, count, duration) VALUES (@user_id, @type, @count, @duration)", conn);
                cmd.Parameters.AddWithValue("user_id", userId);
                cmd.Parameters.AddWithValue("type", exerciseType.ToString());
                cmd.Parameters.AddWithValue("count", count);
                cmd.Parameters.AddWithValue("duration", duration);
                int affectedRows = cmd.ExecuteNonQuery();
                return affectedRows > 0;
            }
        }
    }
}
