using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19.Models
{
    public class SQLPatientRepository : IPatientRepository
    {
        private readonly AppDbContext context;

        public SQLPatientRepository(AppDbContext context)
        {
            this.context = context;
        }

        #region Person
        public Person AddPerson(Person person)
        {
            person.AddDate = DateTime.Now;
            context.Person.Add(person);
            var i =  context.SaveChanges();
            return person;
        }

        public Person FindEmailPassword(Person person)
        {
            person = context.Person.Where(a => a.EmailAddress.Equals(person.EmailAddress) && a.Password.Equals(person.Password)).FirstOrDefault();
            /*context.Person.Add(person);
            var i = context.SaveChanges();*/
            return person;
        }

        public Person DeletePerson(int Id)
        {
            Person person = context.Person.Find(Id);
            if (person != null)
            {
                context.Person.Remove(person);
                context.SaveChanges();
            }
            return person;
        }

        public SelfAssessment DeleteSelfAssessment(int Id)
        {
            SelfAssessment selfAssessment = context.SelfAssessment.Find(Id);
            if (selfAssessment != null)
            {
                context.SelfAssessment.Remove(selfAssessment);
                context.SaveChanges();
            }
            return selfAssessment;
        }

        public Schedule DeleteSchedule(int Id)
        {
            Schedule schedule = context.Schedule.Find(Id);
            if (schedule != null)
            {
                context.Schedule.Remove(schedule);
                context.SaveChanges();
            }
            return schedule;
        }

        public IEnumerable<Person> GetAllPersons()
        {
            var person = from p in context.Person
                         where (int)p.role != 3
                         select p;
                         

            return person;
            //return context.Person;
        }

        public Person GetPerson(int Id)
        {
            return context.Person.Find(Id);
        }

        public Person UpdatePerson(Person patientChanges)
        {
            var employee = context.Person.Attach(patientChanges);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return patientChanges;
        }
        #endregion
        #region Schedule
        public List<SelfAssessment> DeleteDoctor(int Id)
        {

            List<SelfAssessment> selfAssesment = new List<SelfAssessment>();
            selfAssesment = context.SelfAssessment.Where(a => a.DoctorPersonID.Equals(Id)).ToList();

            foreach (SelfAssessment s in selfAssesment) {
                s.Status = "submit";
                s.DoctorPersonID = 0;
            }
            context.SaveChanges();
            return selfAssesment;

        }
        public List<SelfAssessment> DeleteNurse(int Id)
        {

            List<SelfAssessment> selfAssesment = new List<SelfAssessment>();
            selfAssesment = context.SelfAssessment.Where(a => a.NursePersonID.Equals(Id)).ToList();

            foreach (SelfAssessment s in selfAssesment)
            {
                s.Status = "submit";
                s.NursePersonID = 0;
            }
            context.SaveChanges();
            return selfAssesment;

        }
        public Schedule AddSchedule(Schedule schedule)
        {
            var obj = FindSchedule(schedule.PatientPersonID);
            if (obj == null)
            {
                context.Schedule.Add(schedule);
                var i = context.SaveChanges();
                return schedule;
            }
            else
                return null;
        }


        #endregion

        #region SelfAssessment
        public SelfAssessment AddSelfAssessment(SelfAssessment selfAssessment)
        {
            var obj = FindSelfAssessment(selfAssessment.PersonID);
            if (obj == null)
            {
                context.SelfAssessment.Add(selfAssessment);
                var i = context.SaveChanges();
                return selfAssessment;
            }
            else
                return null;
        }

        public IEnumerable<MonitorPatient_Dto> GetAllPersonsAnswers()
        {
            var person = from p in context.Person
                         join d in context.SelfAssessment on p.ID equals d.PersonID
                         where d.Status == "submit"
                         select new MonitorPatient_Dto
                         {
                             PersonID = p.ID,
                             Status = d.Status,
                             PersonName = p.Name,
                             Q1 = d.Q1,
                             Q2 = d.Q2,
                             Q3 = d.Q3,
                             Q4 = d.Q4,
                             Q5 = d.Q5,
                             Q6 = d.Q6,
                             Q7 = d.Q7,
                             Q8 = d.Q8,
                             PersonRole = (int)p.role,
                             DoctorPersonID = d.DoctorPersonID,
                             NursePersonID = d.NursePersonID,
                             AppointmentTime = d.AppointmentTime
            
                             
                         };

            return person;
        }

        public IEnumerable<MonitorPatient_Dto> GetAllDoctorAppPersons()
        {
            var person = from p in context.Person
                         join d in context.SelfAssessment on p.ID equals d.PersonID
                         where d.Status == "DoctorApp"
                         select new MonitorPatient_Dto
                         {
                             PersonID = p.ID,
                             Status = d.Status,
                             PersonName = p.Name,
                             Q1 = d.Q1,
                             Q2 = d.Q2,
                             Q3 = d.Q3,
                             Q4 = d.Q4,
                             Q5 = d.Q5,
                             Q6 = d.Q6,
                             Q7 = d.Q7,
                             Q8 = d.Q8,
                             PersonRole = (int)p.role,
                             DoctorPersonID = d.DoctorPersonID,
                             NursePersonID = d.NursePersonID,
                             AppointmentTime = d.AppointmentTime,
                             EmailAddress = p.EmailAddress

                         };

            return person;
        }

        public IEnumerable<MonitorPatient_Dto> GetAllPersonsAppointment(int nurseID)
        {
            var person = from p in context.Person
                         join d in context.SelfAssessment on p.ID equals d.PersonID
                         where d.Status == "nurseapp" && d.NursePersonID == nurseID
                         select new MonitorPatient_Dto
                         {
                             PersonID = p.ID,
                             Status = d.Status,
                             PersonName = p.Name,
                             Q1 = d.Q1,
                             Q2 = d.Q2,
                             Q3 = d.Q3,
                             Q4 = d.Q4,
                             Q5 = d.Q5,
                             Q6 = d.Q6,
                             Q7 = d.Q7,
                             Q8 = d.Q8,
                             PersonRole = (int)p.role,
                             DoctorPersonID = d.DoctorPersonID,
                             NursePersonID = d.NursePersonID,
                             AppointmentTime = d.AppointmentTime,
                             EmailAddress = p.EmailAddress,
                             PhoneNumber = p.PhoneNumber
                         };

            return person;
        }

        public IEnumerable<DoctorList_Dto> GetAllDrName()
        {
            var person = from p in context.Person
                         where p.role == Person.Role.Doctor
                         select new DoctorList_Dto
                         {
                             PersonID = p.ID,
                             PersonName = p.Name
                         };

            return person;
        }

        public List<Person> getDoctor()
        {
            List<Person> doctor = new List<Person>();
            doctor = (from p in context.Person
                      where p.role == Person.Role.Doctor
                      select p).ToList();
            return doctor;
        }

        public SelfAssessment UpdatePersonStatus(SelfAssessment selfAssessment)
        {
            var employee = context.SelfAssessment.Attach(selfAssessment);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return selfAssessment;
        }

        public SelfAssessment FindSelfAssessment(int personID)
        {
            SelfAssessment selfAssessment = context.SelfAssessment.Where(a => a.PersonID.Equals(personID)).FirstOrDefault();
            return selfAssessment;
        }


        public Schedule FindSchedule(int personID)
        {
            Schedule schedule = context.Schedule.Where(a => a.PatientPersonID.Equals(personID)).FirstOrDefault();
            return schedule;
        }

        public Schedule FindNurseSchedule(int personID)
        {
            Schedule schedule = context.Schedule.Where(a => a.NursePersonID.Equals(personID)).FirstOrDefault();
            return schedule;
        }

        public Schedule FindDoctorSchedule(int personID)
        {
            Schedule schedule = context.Schedule.Where(a => a.DoctorPersonID.Equals(personID)).FirstOrDefault();
            return schedule;
        }


        public IEnumerable<Report_Dto> GetReport()
        {

            var yearlyReport = GetDailyReport(1);
            var weeklyReport = GetWeeklyReport();
            var monthlyReport = GetMonthlyReport();



            return yearlyReport;
        }

        public IEnumerable<Report_Dto> GetDailyReport(int month)
        {
            var report = context.Person
                    .Where(x=> x.AddDate.Month == month && x.AddDate.Year == 2021 && x.role==Person.Role.Patient)
                   .GroupBy(p => p.AddDate.Day)
                   .Select(g => new Report_Dto
                   {
                       key = g.Key,
                       count = g.Count()
                   })
               .ToList();
            
            return report;
        }

        public IEnumerable<Report_Dto> GetWeeklyReport()
        {

            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            DateTime date1 = new DateTime(2021, 1, 1);
            Calendar cal = dfi.Calendar;

            int daysOffset = DayOfWeek.Thursday - date1.DayOfWeek;
            DateTime firstThursday = date1.AddDays(daysOffset);

            cal.GetWeekOfYear(date1, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);


            var pp = (from p in context.Person
                      where p.AddDate.Year == 2021 && p.role == Person.Role.Patient
                      select new Report_Dto
                      {
                          key = cal.GetWeekOfYear(p.AddDate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek),
                          count = p.ID
                      }).ToList();

            var report = pp
                .GroupBy(p => p.key)
                .Select(g => new Report_Dto
                {
                    key = g.Key,
                    count = g.Count(),
                    startDate = firstThursday.AddDays((g.Key - 1) * 7).AddDays(-4),
                    endDate = firstThursday.AddDays((g.Key - 1) * 7).AddDays(2)
                })
            .ToList();
            return report;

        }
        public IEnumerable<Report_Dto> GetMonthlyReport()
        {
            var report = context.Person
                .Where(x =>  x.AddDate.Year == 2021 && x.role == Person.Role.Patient)
                .GroupBy(p => p.AddDate.Month)
                .Select(g => new Report_Dto
                {
                    key = g.Key,
                    count = g.Count()
                })
            .ToList();
            return report;
        }

        private DateTime GetFirstMondayOfYear(int year)
        {
            var dt = new DateTime(year, 1, 1);
            while (dt.DayOfWeek != DayOfWeek.Monday)
            {
                dt = dt.AddDays(1);
            }

            return dt;
        }
        #endregion

    }
}
