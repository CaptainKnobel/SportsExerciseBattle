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
        [Required] public required string Username { get; set; }
        [Required] public required string Password { get; set; }
        [Required] public required string Name { get; set; }

        public string? Bio { get; set; }

        public string? Image { get; set; }

        public int Elo { get; set; }
    }
}
