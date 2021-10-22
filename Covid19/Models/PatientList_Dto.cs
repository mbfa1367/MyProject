using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19.Models
{
    public class PatientList_Dto
    {
        public int PersonID { get; set; }
        public String Status { get; set; }
        public String PersonName { get; set; }
        public Boolean EmailAddress { get; set; }
        public Boolean PhoneNumber { get; set; }

    }
}
