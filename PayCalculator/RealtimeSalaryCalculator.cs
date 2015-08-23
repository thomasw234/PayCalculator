using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using PayCalculator.Models;
using PayCalculator.Repository;
using WebGrease.Css.Extensions;

namespace PayCalculator
{
    public class RealtimeSalaryCalculator : IRealtimeSalaryCalculator
    {
        private readonly ISalaryRepository _salaryRepository;
        public RealtimeSalaryCalculator(ISalaryRepository salaryRepository)
        {
            _salaryRepository = salaryRepository;
        }

        private double GetWorkingHoursThisYear(IEnumerable<WorkingHours> workingHours)
        {
            var year = DateTime.Now.Year;
            var workingHoursList = workingHours.ToList();

            DateTime current = new DateTime(year, 1, 1);
            DateTime to = new DateTime(year + 1, 1, 1);
            double hoursWorked = 0;
            while (current < to)
            {
                var currentDate = current;

                workingHoursList.Where(day => day.WeekDayId == currentDate.ToWorkDayNumber())
                    .ForEach(day => hoursWorked += day.HoursWorked());

                current = current.AddDays(1);
            }

            return hoursWorked;
        }

        // TODO Deal with client timezone, as this will only work if the server timezone is the same
        public async Task<decimal> GetAmountEarnedToday(Guid sessionId)
        {
            var salaryInfo = await _salaryRepository.GetLatestSalaryDetail(sessionId);

            if (!salaryInfo.WorkingHoursId.HasValue)
            {
                return 0;
            }

            var workingHours = await _salaryRepository.GetWorkingHours(salaryInfo.WorkingHoursId.Value);

            var totalHours = await Task.Run(() =>this.GetWorkingHoursThisYear(workingHours));

            if (totalHours <= 0.0)
            {
                return 0;
            }

            if (salaryInfo.SalaryType != SalaryType.Annual)
            {
                throw new NotImplementedException();
            }

            var amountEarnedPerHour = salaryInfo.SalaryValue/(decimal)totalHours;

            // So now we know how much they earn per hour, but must work out how long they've been working today

            var hoursWorkedToday = workingHours.HoursWorkedToday();

            return amountEarnedPerHour*(decimal)hoursWorkedToday;
        }
    }
}