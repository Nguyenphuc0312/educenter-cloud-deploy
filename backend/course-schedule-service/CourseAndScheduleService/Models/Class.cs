using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseAndScheduleService.Models
{
    public class Class
    {
        [Key]
        public int ClassID { get; set; }

        [Required]
        [StringLength(50)]
        public string ClassCode { get; set; } = string.Empty;

        [Required]
        [ForeignKey("Course")]
        public int CourseID { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        public int EnrolledStudents { get; set; } = 0;

        [StringLength(100)]
        public string Instructor { get; set; } = string.Empty;

        [StringLength(50)]
        public string Status { get; set; } = "Active";

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        public Course Course { get; set; } = null!;
        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
