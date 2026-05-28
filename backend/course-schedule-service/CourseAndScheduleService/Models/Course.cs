using System.ComponentModel.DataAnnotations;

namespace CourseAndScheduleService.Models
{
    public class Course
    {
        [Key]
        public int CourseID { get; set; }

        [Required]
        [StringLength(100)]
        public string CourseName { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int Credits { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Active";

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        // Navigation Property
        public ICollection<Class> Classes { get; set; } = new List<Class>();
    }
}
