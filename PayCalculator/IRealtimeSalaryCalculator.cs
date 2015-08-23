﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayCalculator
{
    public interface IRealtimeSalaryCalculator
    {
        Task<decimal> GetAmountEarnedToday(Guid sessionId);
    }
}
