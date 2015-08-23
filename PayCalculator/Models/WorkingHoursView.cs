using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayCalculator.Models
{
    public class WorkingHoursView
    {
        public int WeekdayId { get; set; }
        public DayOfWeek Day { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime LunchStart { get; set; }
        public DateTime LunchEnd { get; set; }
        public bool LunchBreak { get; set; }
    }
}