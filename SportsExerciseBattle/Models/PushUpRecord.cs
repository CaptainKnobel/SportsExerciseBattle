﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SportsExerciseBattle.Models
{
    public class PushUpRecord
    {
        public int UserId { get; set; }
        public int Count { get; set; } = 0;
        public int DurationInSeconds { get; set; } = 0;
        public DateTime Timestamp { get; set; }
    }
}

