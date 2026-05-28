using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseAndScheduleService.Models
{
    public class Schedule
    {
        [Key]
        public int ScheduleID { get; set; }

        [Required]
        [ForeignKey("Class")]
        public int ClassID { get; set; }

        [StringLength(100)]
        public string Room { get; set; } = string.Empty;

        [Required]
        public DateTime ClassDate { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Active";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        // Navigation Property
        public Class Class { get; set; } = null!;
    }
}
