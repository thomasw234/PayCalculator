using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Dapper;
using PayCalculator.Models;

namespace PayCalculator.Repository
{
    public class SqlSalaryRepository : ISalaryRepository
    {
        private readonly IDbConnection _database;

        public SqlSalaryRepository(IDbConnection database)
        {
            _database = database;
        }

        public async Task CreateUser(Guid sessionId, string ipAddress, decimal? latitude, decimal? longitude)
        {
            await _database.ExecuteAsync("dbo.CreateUser", new
            {
                sessionId,
                ipAddress,
                latitude,
                longitude
            });
        }

        public async Task InsertSalaryDetail(Guid sessionId, SalaryType salaryType, decimal salaryValue)
        {
            await _database.ExecuteAsync("dbo.InsertSalaryDetail", new
            {
                sessionId,
                salaryType = (int) salaryType,
                salaryValue
            });
        }
        
        /// <summary>
        /// Will return null if no data is found
        /// </summary>
        public async Task<SalaryDetail> GetLatestSalaryDetail(Guid sessionId)
        {
            var result = await _database.QueryAsync<SalaryDetail>("dbo.GetLatestSalaryDetail", new
            {
                sessionId
            });

            try
            {
                return result.Single();
            }
            catch (InvalidOperationException)
            {
                // If there are no results
                return null;
            }
        }

        public async Task<int> InsertWorkingHours(Weekday day, DateTime startTime, DateTime endTime, int? workingHoursId)
        {
            var result = await _database.QueryAsync<int>("dbo.InsertWorkingHours", new
            {
                id = workingHoursId,
                weekdayId = (int)day,
                startTime,
                endTime
            });

            return result.Single();
        }

        public async Task<IEnumerable<WorkingHours>> GetWorkingHours(int workingHoursId)
        {
            var workingHours = await _database.QueryAsync<WorkingHours>("dbo.GetWorkingHours", new
            {
                workingHoursId
            });

            return workingHours;
        }

        public async Task<bool> IsSalaryDataAvailable(Guid sessionId)
        {
            var result = await this.GetLatestSalaryDetail(sessionId);

            return result != null;
        }

        public async Task AddWorkingScheduleToLatestSalaryDetail(Guid sessionId, int workingHoursId)
        {
            await _database.ExecuteAsync("dbo.InsertWorkingHoursIdToSalaryDetail", new
            {
                sessionId,
                workingHoursId
            });
        }

        public async Task<IEnumerable<int>> GetWorkingDays(Guid sessionId)
        {
            var result = await _database.QueryAsync<int>("dbo.GetWorkingDays", new
            {
                sessionId
            });

            return result;
        }
    }
}