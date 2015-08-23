using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using PayCalculator.Models;

namespace PayCalculator
{
    public static class SalaryCalculatorExtensions
    {
        public static double HoursWorked(this WorkingHours workingHours)
        {
            return (workingHours.EndTime.DateToToday().Subtract(workingHours.StartTime.DateToToday()).TotalHours);
        }

        public static double HoursWorkedToday(this IEnumerable<WorkingHours> workingHours)
        {
            double hoursWorked = 0;

            var allTodayHours = workingHours.Where(day => day.WeekDayId == DateTime.Now.ToWorkDayNumber());

            foreach (var workingHourDefinition in allTodayHours)
            {
                if (workingHourDefinition.StartTime.DateToToday().Hour > DateTime.Now.Hour)
                {
                    // Not got there yet
                    continue; 
                }
                else if (workingHourDefinition.StartTime.DateToToday().Hour < DateTime.Now.Hour &&
                         workingHourDefinition.EndTime.DateToToday().Hour < DateTime.Now.Hour)
                {
                    // All finished working
                    hoursWorked +=
                        workingHourDefinition.EndTime.DateToToday()
                            .Subtract(workingHourDefinition.StartTime.DateToToday())
                            .TotalHours;
                }
                else
                {
                    // Part way through working
                    hoursWorked += DateTime.Now.Subtract(workingHourDefinition.StartTime.DateToToday()).TotalHours;
                }
            }

            return hoursWorked;
        }

        public static DateTime DateToToday(this DateTime time)
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, time.Hour, time.Minute,
                time.Second);
        }

        public static int ToWorkDayNumber(this DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return 1;
                case DayOfWeek.Tuesday:
                    return 2;
                case DayOfWeek.Wednesday:
                    return 3;
                case DayOfWeek.Thursday:
                    return 4;
                case DayOfWeek.Friday:
                    return 5;
                case DayOfWeek.Saturday:
                    return 6;
                case DayOfWeek.Sunday:
                    return 7;
                default: // Damn lousy Smarch weather
                    throw new NotImplementedException();
            }
        }
    }
}