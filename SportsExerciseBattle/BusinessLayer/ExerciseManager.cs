using SportsExerciseBattle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.BusinessLayer
{
    public class ExerciseManager
    {
        private readonly ExerciseRepository _exerciseRepository;

        public ExerciseManager(string connectionString)
        {
            _exerciseRepository = new ExerciseRepository(connectionString);
        }

        public bool AddExerciseEntry(string username, ExerciseType type, int count, int duration)
        {
            if (Enum.IsDefined(typeof(ExerciseType), type) && count > 0 && duration > 0)
            {
                return _exerciseRepository.AddEntry(username, type, count, duration);
            }
            return false;
        }
    }
}
