using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Covid19.Models
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Person> Person { get; set; }
        public DbSet<Schedule> Schedule { get; set; }
        public DbSet<SelfAssessment> SelfAssessment { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Person>().HasData(
                new Person
                {
                    ID = 1,
                    Name = "Jhon",
                    DateOfBirth = DateTime.ParseExact("2009-05-08 14:40:52,531", "yyyy-MM-dd HH:mm:ss,fff", System.Globalization.CultureInfo.InvariantCulture),
                    Address = "Main",
                    PhoneNumber = 123,
                    EmailAddress = "abc@efg.com",
                    RegistrationNumber = 123,
                    role = Models.Person.Role.Patient
                }
            );
        }
    }
}
