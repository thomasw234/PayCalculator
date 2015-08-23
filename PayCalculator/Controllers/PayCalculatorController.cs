using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PayCalculator.Models;
using PayCalculator.Repository;

namespace PayCalculator.Controllers
{
    public class PayCalculatorController : AsyncController
    {
        private readonly ISalaryRepository _salaryRepository;
        private readonly ISessionManager _sessionManager;
        private readonly IRealtimeSalaryCalculator _salaryCalculator;
        
        public PayCalculatorController(ISalaryRepository salaryRepository, ISessionManager sessionManager, IRealtimeSalaryCalculator salaryCalculator)
        {
            _salaryRepository = salaryRepository;
            _sessionManager = sessionManager;
            _salaryCalculator = salaryCalculator;
        }

        public async Task<ActionResult> Index()
        {
            var sessionId = _sessionManager.GetSessionId(Request, Response);

            ViewBag.SessionId = sessionId;

            if ((await _salaryRepository.IsSalaryDataAvailable(sessionId)) == false)
            {
                return RedirectToAction("Salary");
            }

            ViewBag.CurrentlyEarned = await _salaryCalculator.GetAmountEarnedToday(sessionId);

            return View();
        }

        private async Task<decimal> GetCurrentSalary()
        {
            var sessionId = _sessionManager.GetSessionId(Request, Response);
            var salaryInfo = await _salaryRepository.GetLatestSalaryDetail(sessionId);

            if (salaryInfo != null)
            {
                return salaryInfo.SalaryValue;
            }
            else
            {
                return 0;
            }
        }
        
        public async Task<ActionResult> Salary()
        {
            ViewBag.Salary = await this.GetCurrentSalary();
            
            return View();
        }

        public async Task<ActionResult> UpdateSalary(string newSalary)
        {
            decimal salary = 0;
            var sessionId = _sessionManager.GetSessionId(Request, Response);

            if (!decimal.TryParse(newSalary, out salary))
            {
                ViewBag.Salary = await this.GetCurrentSalary();
                ViewBag.ErrorMessage = "Invalid salary input. Try again";

                return View("Salary");
            }
            else
            {
                var currentWorkingDays = (await _salaryRepository.GetWorkingDays(sessionId)).ToList();

                await _salaryRepository.InsertSalaryDetail(sessionId, SalaryType.Annual, salary);

                var workingDays = new WorkingDays
                {
                    Monday = currentWorkingDays.Contains(1),
                    Tuesday = currentWorkingDays.Contains(2),
                    Wednesday = currentWorkingDays.Contains(3),
                    Thursday = currentWorkingDays.Contains(4),
                    Friday = currentWorkingDays.Contains(5),
                    Saturday = currentWorkingDays.Contains(6),
                    Sunday = currentWorkingDays.Contains(7)
                };

                return View("WorkingDays", workingDays);
            }

        }

        public async Task<ActionResult> WorkingDays(WorkingDays days)
        {
            var sessionId = _sessionManager.GetSessionId(Request, Response);
            var workingHours = new List<WorkingHoursView>();

            var salaryDetail = await _salaryRepository.GetLatestSalaryDetail(sessionId);

            if (salaryDetail.WorkingHoursId.HasValue)
            {
                var storedWorkingHours = await _salaryRepository.GetWorkingHours(salaryDetail.WorkingHoursId.Value);
                workingHours = storedWorkingHours.ToWorkingHoursView().ToList();
                
                // TODO Make this cleaner
                // Add any missing days
                #region nastiness
                if (days.Monday && !workingHours.Any(workingDay => workingDay.WeekdayId == 1))
                {
                    workingHours.AddRange(GetDefaultWorkingHours(new WorkingDays() {Monday = true}));
                }
                if (days.Tuesday && !workingHours.Any(workingDay => workingDay.WeekdayId == 2))
                {
                    workingHours.AddRange(GetDefaultWorkingHours(new WorkingDays() { Tuesday = true }));
                }
                if (days.Wednesday && !workingHours.Any(workingDay => workingDay.WeekdayId == 3))
                {
                    workingHours.AddRange(GetDefaultWorkingHours(new WorkingDays() { Wednesday = true }));
                }
                if (days.Thursday && !workingHours.Any(workingDay => workingDay.WeekdayId == 4))
                {
                    workingHours.AddRange(GetDefaultWorkingHours(new WorkingDays() { Monday = true }));
                }
                if (days.Friday && !workingHours.Any(workingDay => workingDay.WeekdayId == 5))
                {
                    workingHours.AddRange(GetDefaultWorkingHours(new WorkingDays() { Friday = true }));
                }
                if (days.Saturday && !workingHours.Any(workingDay => workingDay.WeekdayId == 6))
                {
                    workingHours.AddRange(GetDefaultWorkingHours(new WorkingDays() { Saturday = true }));
                }
                if (days.Sunday && !workingHours.Any(workingDay => workingDay.WeekdayId == 7))
                {
                    workingHours.AddRange(GetDefaultWorkingHours(new WorkingDays() { Sunday = true }));
                }
                #endregion
                // Remove all removed days
                #region MoreNastiness
                if (!days.Monday)
                {
                    workingHours.RemoveAll(workingHour => workingHour.WeekdayId == 1);
                }
                if (!days.Tuesday)
                {
                    workingHours.RemoveAll(workingHour => workingHour.WeekdayId == 2);
                }
                if (!days.Wednesday)
                {
                    workingHours.RemoveAll(workingHour => workingHour.WeekdayId == 3);
                }
                if (!days.Thursday)
                {
                    workingHours.RemoveAll(workingHour => workingHour.WeekdayId == 4);
                }
                if (!days.Friday)
                {
                    workingHours.RemoveAll(workingHour => workingHour.WeekdayId == 5);
                }
                if (!days.Saturday)
                {
                    workingHours.RemoveAll(workingHour => workingHour.WeekdayId == 6);
                }
                if (!days.Sunday)
                {
                    workingHours.RemoveAll(workingHour => workingHour.WeekdayId == 7);
                }
                #endregion
            }
            else
            {
                workingHours = this.GetDefaultWorkingHours(days);
            }
            
            
            return View("WorkingHours", workingHours);
        }

        private List<WorkingHoursView> GetDefaultWorkingHours(WorkingDays days)
        {
            var workingHours = new List<WorkingHoursView>();
            var startTime = new DateTime(2015, 01, 01, 9, 0, 0);
            var endTime = new DateTime(2015, 01, 01, 18, 0, 0);
            var lunchStart = new DateTime(2015, 01, 01, 13, 0, 0);
            var lunchEnd = new DateTime(2015, 01, 01, 14, 0, 0);
            if (days.Monday)
            {
                workingHours.Add(new WorkingHoursView
                {
                    Day = DayOfWeek.Monday,
                    WeekdayId = 1,
                    StartTime = startTime,
                    EndTime = endTime,
                    LunchBreak = true,
                    LunchStart = lunchStart,
                    LunchEnd = lunchEnd
                });
            }
            if (days.Tuesday)
            {
                workingHours.Add(new WorkingHoursView
                {
                    Day = DayOfWeek.Tuesday,
                    WeekdayId = 2,
                    StartTime = startTime,
                    EndTime = endTime,
                    LunchBreak = true,
                    LunchStart = lunchStart,
                    LunchEnd = lunchEnd
                });
            }
            if (days.Wednesday)
            {
                workingHours.Add(new WorkingHoursView
                {
                    Day = DayOfWeek.Wednesday,
                    WeekdayId = 3,
                    StartTime = startTime,
                    EndTime = endTime,
                    LunchBreak = true,
                    LunchStart = lunchStart,
                    LunchEnd = lunchEnd
                });
            }
            if (days.Thursday)
            {
                workingHours.Add(new WorkingHoursView
                {
                    Day = DayOfWeek.Thursday,
                    WeekdayId = 4,
                    StartTime = startTime,
                    EndTime = endTime,
                    LunchBreak = true,
                    LunchStart = lunchStart,
                    LunchEnd = lunchEnd
                });
            }
            if (days.Friday)
            {
                workingHours.Add(new WorkingHoursView
                {
                    Day = DayOfWeek.Friday,
                    WeekdayId = 5,
                    StartTime = startTime,
                    EndTime = endTime,
                    LunchBreak = true,
                    LunchStart = lunchStart,
                    LunchEnd = lunchEnd
                });
            }
            if (days.Saturday)
            {
                workingHours.Add(new WorkingHoursView
                {
                    Day = DayOfWeek.Saturday,
                    WeekdayId = 6,
                    StartTime = startTime,
                    EndTime = endTime,
                    LunchBreak = true,
                    LunchStart = lunchStart,
                    LunchEnd = lunchEnd
                });
            }
            if (days.Sunday)
            {
                workingHours.Add(new WorkingHoursView
                {
                    Day = DayOfWeek.Sunday,
                    WeekdayId = 7,
                    StartTime = startTime,
                    EndTime = endTime,
                    LunchBreak = true,
                    LunchStart = lunchStart,
                    LunchEnd = lunchEnd
                });
            }
            return workingHours;
        }

        public async Task<ActionResult> WorkingHours(IList<WorkingHoursView> workingHours)
        {
            var workingHoursList = new List<WorkingHours>();

            foreach (var input in workingHours)
            {
                if (input.LunchBreak == true)
                {
                    workingHoursList.Add(new WorkingHours
                    {
                        WeekDayId = input.WeekdayId,
                        StartTime = input.StartTime,
                        EndTime = input.LunchStart
                    });

                    workingHoursList.Add(new WorkingHours
                    {
                        WeekDayId = input.WeekdayId,
                        StartTime = input.LunchEnd,
                        EndTime = input.EndTime
                    });
                }
                else
                {
                    workingHoursList.Add(new WorkingHours
                    {
                        WeekDayId = input.WeekdayId,
                        StartTime = input.StartTime,
                        EndTime = input.EndTime
                    });
                }
            }

            int? workingHoursId = null;
            foreach (var schedule in workingHoursList)
            {
                workingHoursId = await _salaryRepository.InsertWorkingHours((Weekday)schedule.WeekDayId, schedule.StartTime,
                    schedule.EndTime, workingHoursId);
            }

            var sessionId = _sessionManager.GetSessionId(Request, Response);

            if (workingHoursId.HasValue)
            {
                await _salaryRepository.AddWorkingScheduleToLatestSalaryDetail(sessionId, workingHoursId.Value);
            }

            return RedirectToAction("Index");
        }
    }
}