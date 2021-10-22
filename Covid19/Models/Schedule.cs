using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19.Models
{
    public class Schedule
    {

        [Key]
        public int ID { get; set; }

        [ForeignKey("Person")]
        public int PatientPersonID { get; set; }
        public int DoctorPersonID { get; set; }
        public int NursePersonID { get; set; }
        public DateTime AppointmentTime { get; set; }

        
        public Person Person { get; set; }

    }
}
