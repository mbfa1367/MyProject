using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19.Models
{
    public interface IPatientRepository
    {
        Person GetPerson(int Id);
        IEnumerable<Person> GetAllPersons();
        Person AddPerson(Person patient);
        Person UpdatePerson(Person patientChanges);
        Person DeletePerson(int Id);

        Person FindEmailPassword(Person person);
        SelfAssessment AddSelfAssessment(SelfAssessment selfAssessment);
        IEnumerable<MonitorPatient_Dto> GetAllPersonsAnswers();
        IEnumerable<MonitorPatient_Dto> GetAllPersonsAppointment(int nurseID);
        IEnumerable<Report_Dto> GetReport();
        IEnumerable<Report_Dto> GetDailyReport(int month);
        IEnumerable<Report_Dto> GetWeeklyReport();
        IEnumerable<Report_Dto> GetMonthlyReport();
        SelfAssessment UpdatePersonStatus(SelfAssessment selfAssessment);
        SelfAssessment FindSelfAssessment(int personID);
        IEnumerable<DoctorList_Dto> GetAllDrName();

        Schedule AddSchedule(Schedule schedule);
        List<Person> getDoctor();
        Schedule FindNurseSchedule(int personID);
        Schedule FindSchedule(int personID);
        Schedule FindDoctorSchedule(int personID);
        SelfAssessment DeleteSelfAssessment(int Id);
        Schedule DeleteSchedule(int Id);
        List<SelfAssessment> DeleteDoctor(int Id);
        List<SelfAssessment> DeleteNurse(int Id);
        IEnumerable<MonitorPatient_Dto> GetAllDoctorAppPersons();
    }
}

