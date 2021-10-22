using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19.Models
{
    public class Report_Dto
    {
        public int count { get; set; }
        public int key { get; set; }
        public DayOfWeek keys { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
}
