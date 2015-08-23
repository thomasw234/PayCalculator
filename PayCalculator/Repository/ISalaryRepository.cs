using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PayCalculator.Models;

namespace PayCalculator.Repository
{
    public interface ISalaryRepository
    {
        Task CreateUser(Guid sessionId, string ipAddress, decimal? latitude, decimal? longitude);
        Task InsertSalaryDetail(Guid sessionId, SalaryType salaryType, decimal salaryValue);
        
        /// <summary>
        /// Will return null if no data is found
        /// </summary>
        Task<SalaryDetail> GetLatestSalaryDetail(Guid sessionId);
        // Returns the working hours ID
        Task<int> InsertWorkingHours(Weekday day, DateTime startTime, DateTime endTime, int? workingHoursId);

        Task<IEnumerable<WorkingHours>> GetWorkingHours(int workingHoursId);

        Task<bool> IsSalaryDataAvailable(Guid sessionId);

        Task AddWorkingScheduleToLatestSalaryDetail(Guid sessionId, int workingHoursId);

        Task<IEnumerable<int>> GetWorkingDays(Guid sessionId);
    }
}
