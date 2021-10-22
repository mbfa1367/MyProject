using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19.Models
{
    public class SelfAssessment
    {
        [Key]
        public int ID { get; set; }
        public String Status { get; set; }
        [ForeignKey("Person")]
        public int PersonID { get; set; }
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
        public Person Person { get; set; }
    }
}
