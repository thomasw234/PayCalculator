using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PayCalculator.Repository;

namespace PayCalculator.Models
{
    public class SalaryDetail
    {
        public decimal SalaryValue { get; set; }
        public SalaryType SalaryType { get; set; }
        public DateTime? CreatedUtc { get; set; }

        public int? WorkingHoursId { get; set; }
    }
}