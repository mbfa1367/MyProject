using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19.Models
{
    public class MonitorPatient_Dto
    {
        public int PersonID { get; set; }
        public String  Status { get; set; }
        public String  PersonName { get; set; }
        public Boolean Q1 { get; set; }
        public String Q2 { get; set; }
        public Boolean Q3 { get; set; }
        public Boolean Q4 { get; set; }
        public Boolean Q5 { get; set; }
        public Boolean Q6 { get; set; }
        public Boolean Q7 { get; set; }
        public Boolean Q8 { get; set; }
        public int DoctorPersonID { get; set; }
        public int NursePersonID { get; set; }
        public DateTime AppointmentTime { get; set; }
        public int? PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public int PersonRole { get; set; }

        //public List<String> DoctorNameList { get; set; }
    }
}
