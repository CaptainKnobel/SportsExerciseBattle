﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Models
{
    public class UserStats
    {
        public string Username { get; set; } = string.Empty;
        public int TotalPushUps { get; set; } = 0;
        public int Elo { get; set; } = 0;
    }
}
