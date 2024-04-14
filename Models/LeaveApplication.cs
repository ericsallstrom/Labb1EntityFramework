using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Labb1EntityFramework.Models
{
    public class LeaveApplication
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Submission Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ApplicationDate { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Type of Leave")]
        public LeaveType? LeaveType { get; set; }

        [Required]
        [ForeignKey("Employee")]
        [Display(Name = "Employee")]
        public int FkEmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }

    public enum LeaveType
    {
        [Display(Name = "Childcare Leave")]
        Childcare,

        [Display(Name = "Parental Leave")]
        Parental,

        [Display(Name = "Personal Leave")]
        Personal,

        [Display(Name = "Sick Leave")]
        Sick,

        [Display(Name = "Vacation Leave")]
        Vacation,

        [Display(Name = "Other")]
        Other
    }
}
