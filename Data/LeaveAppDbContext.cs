using Labb1EntityFramework.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Labb1EntityFramework.Data
{
    public class LeaveAppDbContext : IdentityDbContext
    {
        public LeaveAppDbContext(DbContextOptions<LeaveAppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<LeaveApplication> LeaveApplications { get; set; }        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1, FirstName = "Eric", LastName = "Sällström", DateOfBirth = new DateTime(1991, 1, 5) },
                new Employee { Id = 2, FirstName = "Amanda", LastName = "Jonsson", DateOfBirth = new DateTime(1989, 10, 20) },
                new Employee { Id = 3, FirstName = "John", LastName = "Smith", DateOfBirth = new DateTime(1985, 7, 12) },
                new Employee { Id = 4, FirstName = "Emily", LastName = "Ferguson", DateOfBirth = new DateTime(1992, 4, 25) },
                new Employee { Id = 5, FirstName = "David", LastName = "Brown", DateOfBirth = new DateTime(1978, 10, 15) },
                new Employee { Id = 6, FirstName = "Sarah", LastName = "Wilson", DateOfBirth = new DateTime(1989, 12, 3) }
            );

            modelBuilder.Entity<LeaveApplication>().HasData(
                new LeaveApplication
                {
                    Id = 1,
                    FkEmployeeId = 1,                    
                    StartDate = new DateTime(2024, 4, 15),
                    EndDate = new DateTime(2024, 4, 18),
                    LeaveType = LeaveType.Vacation
                },
                new LeaveApplication
                {
                    Id = 2,
                    FkEmployeeId = 2,                    
                    StartDate = new DateTime(2024, 6, 2),
                    EndDate = new DateTime(2024, 6, 9),
                    LeaveType = LeaveType.Personal
                },
                new LeaveApplication
                {
                    Id = 3,
                    FkEmployeeId = 3,                    
                    StartDate = new DateTime(2023, 8, 01),
                    EndDate = new DateTime(2023, 8, 10),
                    LeaveType = LeaveType.Sick
                },
                new LeaveApplication
                {
                    Id = 4,
                    FkEmployeeId = 4,                    
                    StartDate = new DateTime(2024, 7, 20),
                    EndDate = new DateTime(2024, 7, 25),
                    LeaveType = LeaveType.Childcare
                },
                 new LeaveApplication
                 {
                     Id = 5,
                     FkEmployeeId = 5,                     
                     StartDate = new DateTime(2023, 9, 5),
                     EndDate = new DateTime(2023, 9, 10),
                     LeaveType = LeaveType.Vacation
                 },
                new LeaveApplication
                {
                    Id = 6,
                    FkEmployeeId = 6,                    
                    StartDate = new DateTime(2024, 6, 15),
                    EndDate = new DateTime(2024, 8, 15),
                    LeaveType = LeaveType.Personal
                }
            );
        }
    }
}
