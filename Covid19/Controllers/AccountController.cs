using Covid19.Models;
using Covid19.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Covid19.Controllers
{
    public class AccountController : Controller
    {
        // GET:
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        public IPatientRepository service { get; set; }
        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, IPatientRepository IPService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.service = IPService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Profile()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Profile(Person person)
        {
            IPatientRepository services = (IPatientRepository)service;
            services.AddPerson(person);
            //return View(person);

            //return RedirectToAction("Login");

            if (HttpContext.Session.GetString("EmailAddress") == "admin")
            {
                return RedirectToAction("MonitorUsers");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Copy data from RegisterViewModel to IdentityUser
                var user = new IdentityUser
                {
                    UserName = model.EmailAddress,
                    Email = model.EmailAddress
                };

                // Store user data in AspNetUsers database table
                var result = await userManager.CreateAsync(user, model.Password);

                // If user is successfully created, sign-in the user using
                // SignInManager and redirect to index action of HomeController
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "home");
                }

                // If there are any errors, add them to the ModelState object
                // which will be displayed by the validation summary tag helper
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Person person)
        {
            if (ModelState.IsValid)
            {
                IPatientRepository services = (IPatientRepository)service;
                var obj = services.FindEmailPassword(person);
                if (obj != null)
                {
                    var test = services.FindSelfAssessment(obj.ID);

                    if (test != null)
                    {
                        HttpContext.Session.SetString("SelfAssessmentID", test.ID.ToString());

                        if (test.Status != null)
                            HttpContext.Session.SetString("Status", test.Status.ToString());
                        else
                            HttpContext.Session.SetString("Status", "---");

                    }
                    else
                    {
                        HttpContext.Session.SetString("SelfAssessmentID", "N/A");
                    }

                    HttpContext.Session.SetString("UserID", obj.ID.ToString());
                    HttpContext.Session.SetString("Name", obj.Name.ToString());
                    HttpContext.Session.SetString("DateOfBirth", obj.DateOfBirth.ToString());
                    HttpContext.Session.SetString("Address", obj.Address.ToString());
                    HttpContext.Session.SetString("PhoneNumber", obj.PhoneNumber.ToString());
                    HttpContext.Session.SetString("EmailAddress", obj.EmailAddress.ToString());                    

                    if (obj.role == Person.Role.Patient)
                        return RedirectToAction("PatientPage");
                    else if (obj.role == Person.Role.Nurse)
                        return RedirectToAction("NursePage");
                    else if (obj.role == Person.Role.Doctor)
                        return RedirectToAction("DoctorPage");
                    else
                        return RedirectToAction("ManagerPage");
                }
                else
                {
                    ViewBag.error = "Error";
                }

            }
            return View(person);
        }

        public ActionResult PatientPage()
        {
            if (HttpContext.Session.GetString("UserID") != null)
            {
                IPatientRepository services = (IPatientRepository)service;
                var schedule = services.FindSelfAssessment(Int32.Parse(HttpContext.Session.GetString("UserID")));


                return View(schedule);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult NursePage()
        {
            if (HttpContext.Session.GetString("UserID") != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult DoctorPage()
        {
            if (HttpContext.Session.GetString("UserID") != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult ManagerPage()
        {
            if (HttpContext.Session.GetString("UserID") != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpGet]
        public IActionResult SelfAssessment()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SelfAssessment(SelfAssessment selfAssessment)
        {
            IPatientRepository services = (IPatientRepository)service;
            var test = services.FindSelfAssessment(Int32.Parse(HttpContext.Session.GetString("UserID")));
            if (test != null)
            {
                test.Q1 = selfAssessment.Q1;
                test.Q2 = selfAssessment.Q2;
                test.Q3 = selfAssessment.Q3;
                test.Q4 = selfAssessment.Q4;
                test.Q5 = selfAssessment.Q5;
                test.Q6 = selfAssessment.Q6;
                test.Q7 = selfAssessment.Q7;
                test.Q8 = selfAssessment.Q8;
                if (test.Status == "reject")
                {
                    test.Status = "submit";
                    HttpContext.Session.SetString("Status", "submit");
                }

                services.UpdatePersonStatus(test);
            }
            else
            {

                HttpContext.Session.SetString("SelfAssessmentID", selfAssessment.ID.ToString());
                HttpContext.Session.SetString("Status", "submit");
                var perasonID = HttpContext.Session.GetString("UserID");
                selfAssessment.PersonID = int.Parse(perasonID);
                selfAssessment.Status = "submit";

                var obj = services.AddSelfAssessment(selfAssessment);
            }
            return RedirectToAction("PatientPage");
        }


        [HttpGet]
        public async Task<IActionResult> MonitorPatient()
        {
            IPatientRepository services = (IPatientRepository)service;

           AssessmentResultModelView result = new AssessmentResultModelView();
            result.PatientList=services.GetAllPersonsAnswers();
            List<Person> doctor = new List<Person>();
            doctor = service.getDoctor();
            foreach (Person p in doctor) {
                p.Name = "Doctor: " + p.Name;
            }
            //doctor.Insert(0, new Person { ID = 0 , Name="--Select Doctor--"});
            doctor.Insert(0, new Person { ID = 0, Name = "Nurse: " + HttpContext.Session.GetString("Name") });
            ViewBag.ListofDocotor = doctor;
            return View(result);
            

        }

        [HttpGet]
        public async Task<IActionResult> MonitorUsers()
        {
            IPatientRepository services = (IPatientRepository)service;
            var person = services.GetAllPersons();
            return View(person);
        } 
        
        
        [HttpGet]
        public async Task<IActionResult> ManagerReport()
        {
            IPatientRepository services = (IPatientRepository)service;

            var person = services.GetReport();
            return View(person);
        }



        [HttpGet]
        public async Task<IActionResult> NursePatientAppointment()
        {
            var nurseID = HttpContext.Session.GetString("UserID");
            IPatientRepository services = (IPatientRepository)service;
            //IEnumerable<MonitorPatient_Dto> result = new IEnumerable<MonitorPatient_Dto>();
           var result = services.GetAllPersonsAppointment(int.Parse(nurseID));
            return View(result);
        }

        public async Task<IActionResult> MonitorPatients(int personID, int doctor, DateTime date)
        {
            var nurseID = HttpContext.Session.GetString("UserID");
            IPatientRepository services = (IPatientRepository)service;
            var obj = services.FindSelfAssessment(personID);
            if (doctor != 0)
            {
                obj.Status = "DoctorApp";
                obj.DoctorPersonID = doctor;
                obj.AppointmentTime = date;
            }
            else
            {
                obj.Status = "nurseapp";
                obj.AppointmentTime = date;
                obj.NursePersonID = int.Parse(nurseID);
            }

            services.UpdatePersonStatus(obj);
            return RedirectToAction("MonitorPatient");
        }

        public async Task<IActionResult> doctorAcceptPatient(int personID)
        {
            var doctorID = HttpContext.Session.GetString("UserID");
            IPatientRepository services = (IPatientRepository)service;
            var obj = services.FindSelfAssessment(personID);
            obj.Status = "DoctorApp";
            obj.DoctorPersonID = int.Parse(doctorID);

            services.UpdatePersonStatus(obj);
            return RedirectToAction("ListOfPatient");
        }

        [HttpGet]
        public async Task<IActionResult> Report(string reportType,int month, string MonthforDailyReport)
        {

            IEnumerable<Report_Dto> yearly = Enumerable.Empty<Report_Dto>();
            IEnumerable<Report_Dto> weekly = Enumerable.Empty<Report_Dto>();
            IEnumerable<Report_Dto> monthly = Enumerable.Empty<Report_Dto>();

            IPatientRepository services = (IPatientRepository)service;
            if (month > 0)
            {
                 yearly = services.GetDailyReport(month);

            }
            if (reportType == "Weekly") {
                 weekly = services.GetWeeklyReport();
            }
            if (reportType == "Monthly")
            {
                 monthly = services.GetMonthlyReport();
            }

                var viewmodel = new ReportModelView
            {
                yearlyReport = yearly,
                weeklyReport = weekly,
                monthlyReport = monthly,
                    MonthforDailyReport = MonthforDailyReport
                };
            return View(viewmodel);
        }

        [HttpGet]
        public async Task<IActionResult> ListOfPatient()
        {
            IPatientRepository services = (IPatientRepository)service;
            var result = services.GetAllDoctorAppPersons();
            return View(result);
        }

        public async Task<IActionResult> changeAppointment(int personID, DateTime date)
        {
            
            IPatientRepository services = (IPatientRepository)service;
           
            var obj = services.FindSelfAssessment(personID);
            if (date != null)
            {
                obj.AppointmentTime = date;
            }

            services.UpdatePersonStatus(obj);
            return RedirectToAction("ListOfPatient");

        }

        public async Task<IActionResult> logOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("index", "home");
        }

        public async Task<IActionResult> removePatient(int personID)
        {
            IPatientRepository services = (IPatientRepository)service;
            var obj = services.FindSelfAssessment(personID);
            obj.Status = "reject";
            obj.NursePersonID = 0;
            services.UpdatePersonStatus(obj);
            return RedirectToAction("MonitorPatient");
        }

        public async Task<IActionResult> doctorRemovePatient(int personID)
        {
            IPatientRepository services = (IPatientRepository)service;
            var obj = services.FindSelfAssessment(personID);
            obj.Status = "reject";
            obj.DoctorPersonID = 0;
            services.UpdatePersonStatus(obj);
            return RedirectToAction("ListOfPatient");
        }

        public async Task<IActionResult> removeUser(int personID)
        {
            IPatientRepository services = (IPatientRepository)service;
            var person = services.GetPerson(personID);

            if (person.role == Person.Role.Doctor) {
                services.DeletePerson(person.ID);
                services.DeleteDoctor(person.ID);
            }

            if (person.role == Person.Role.Nurse)
            {
                services.DeletePerson(person.ID);
                services.DeleteNurse(person.ID);
            }

            if (person.role == Person.Role.Patient)
            {
                services.DeletePerson(person.ID);
                var selfAssessment = services.FindSelfAssessment(personID);
                if(selfAssessment != null)
                    services.DeleteSelfAssessment(selfAssessment.ID);
            }

            return RedirectToAction("MonitorUsers");
        }
    }

}
