using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SportsExerciseBattle.Models
{
    public class User
    {
        public required string Username { get; set; }
        public required string Name { get; set; }
        public string Password { get; set; }
        public string? Bio { get; set; } = string.Empty;

        public string? Image { get; set; }

        public int Elo { get; set; } = 0;
    }
}
