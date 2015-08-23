using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using PayCalculator.Models;

namespace PayCalculator.Controllers
{
    public static class ControllerExtensions
    {
        public static int ToDayOfWeekId(this DayOfWeek day)
        {
            switch (day)
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
                default:
                    throw new NotImplementedException();
            }
        }

        public static DayOfWeek ToDayOfWeek(this int workDayId)
        {
            switch (workDayId)
            {
                case 1:
                    return DayOfWeek.Monday;
                case 2:
                    return DayOfWeek.Tuesday;
                case 3:
                    return DayOfWeek.Wednesday;
                case 4:
                    return DayOfWeek.Thursday;
                case 5:
                    return DayOfWeek.Friday;
                case 6:
                    return DayOfWeek.Saturday;
                case 7:
                    return DayOfWeek.Sunday;
                default:
                    throw new NotImplementedException();
            }
        }

        public static IEnumerable<WorkingHoursView> ToWorkingHoursView(this IEnumerable<WorkingHours> storedWorkingHours)
        {
            var workingHoursViews = new List<WorkingHoursView>();
            var storedWorkingHoursList = storedWorkingHours.ToList();

            for (int i = 0; i <= 7; i++)
            {
                if (storedWorkingHoursList.Any(schedule => schedule.WeekDayId == i))
                {
                    var schedules = storedWorkingHoursList.Where(schedule => schedule.WeekDayId == i).ToList();

                    if (schedules.Count == 1)
                    {
                        workingHoursViews.Add(new WorkingHoursView
                        {
                            WeekdayId = schedules[0].WeekDayId,
                            Day = schedules[0].WeekDayId.ToDayOfWeek(),
                            StartTime = schedules[0].StartTime,
                            EndTime = schedules[0].EndTime,
                            LunchBreak = false,
                            LunchStart = new DateTime(2015, 01, 01, 13, 0, 0),
                            LunchEnd = new DateTime(2015, 01, 01, 14, 0, 0)
                        });
                    }
                    else if (schedules.Count == 2)
                    {
                        var ordered = schedules.OrderBy(schedule => schedule.StartTime.DateToToday()).ToList();
                        workingHoursViews.Add(new WorkingHoursView
                        {
                            WeekdayId = ordered[0].WeekDayId,
                            Day = ordered[0].WeekDayId.ToDayOfWeek(),
                            StartTime = ordered[0].StartTime,
                            EndTime = ordered[1].EndTime,
                            LunchBreak = true,
                            LunchStart = ordered[0].EndTime,
                            LunchEnd = ordered[1].StartTime
                        });
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }

                }
            }

            return workingHoursViews;
        } 
    }
}