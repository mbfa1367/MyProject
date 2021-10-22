using Covid19.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19.ViewModels
{
    public class ReportModelView
    {
        public IEnumerable<Report_Dto> yearlyReport { get; set; }
        public IEnumerable<Report_Dto> weeklyReport { get; set; }
        public IEnumerable<Report_Dto> monthlyReport { get; set; }
        public string MonthforDailyReport { get; set; }
    }
}
