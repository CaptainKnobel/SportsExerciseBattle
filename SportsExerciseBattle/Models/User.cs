﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SportsExerciseBattle.Models
{
    public class User
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string Bio { get; set; } = "";
        public string Image { get; set; } = "";
        public string Name { get; set; } = "";
        public int Elo { get; set; }
    }
}
