using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covid19.Models;
using System;


namespace Covid19.ViewModels
{
    public class AssessmentResultModelView
    {
        public int PersonID { get; set; }
        public String Status { get; set; }
        public String PersonName { get; set; }
        public Boolean Q1 { get; set; }
        public Boolean Q2 { get; set; }
        public Boolean Q3 { get; set; }
        public Boolean Q4 { get; set; }
        public Boolean Q5 { get; set; }
        public Boolean Q6 { get; set; }
        public Boolean Q7 { get; set; }
        public Boolean Q8 { get; set; }
        public int PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public int PersonRole { get; set; }
        public DateTime AppointmentTime { get; set; }
        public IEnumerable<MonitorPatient_Dto> PatientList { get; set; }
        public IEnumerable<DoctorList_Dto> DoctorList { get; set; }

       // public SelectList DoctorDropDownList { get; set; }
      


    }

}

