using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19.Models
{
    public class Person
    {
        public enum Role
        {
            Patient, Doctor, Nurse, Manager
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime AddDate { get; set; }
        public string Address { get; set; }
        public int PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public int RegistrationNumber { get; set; }
        public Role? role { get; set; }
        public ICollection<Schedule> schedule { get; set; }
        public ICollection<SelfAssessment> selfAssessment { get; set; }
        public string Password { get; set; }

    }
}
