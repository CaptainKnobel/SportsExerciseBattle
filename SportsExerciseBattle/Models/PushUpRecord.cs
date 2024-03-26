﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEB.Models
{
    public class PushUpRecord
    {
        public int UserId { get; set; }
        public int Count { get; set; }
        public int DurationInSeconds { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

