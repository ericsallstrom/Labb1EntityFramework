using System.ComponentModel.DataAnnotations;

namespace Labb1EntityFramework.Models
{
    public class Employee
    {
        [Key]        
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(100, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(120, MinimumLength = 1)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DateOfBirth { get; set; }

        public virtual ICollection<LeaveApplication>? LeaveApplications { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
