﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PayCalculator.Repository;

namespace PayCalculator.Models
{
    public class WorkingHours
    {
        public int WeekDayId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}