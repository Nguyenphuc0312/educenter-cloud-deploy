namespace CourseAndScheduleService.DTOs
{
    /// <summary>
    /// DTO ?? tr? v? Schedule (Read Operation)
    /// </summary>
    public class ScheduleDto
    {
        public int ScheduleID { get; set; }
        public int ClassID { get; set; }
        public string Room { get; set; } = string.Empty;
        public DateTime ClassDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; } = "Active";
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public ClassDto? Class { get; set; }
    }

    /// <summary>
    /// DTO ?? t?o Schedule m?i (Create Operation)
    /// </summary>
    public class CreateScheduleDto
    {
        public int ClassID { get; set; }
        public string Room { get; set; } = string.Empty;
        public DateTime ClassDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; } = "Active";
    }

    /// <summary>
    /// DTO ?? c?p nh?t Schedule (Update Operation)
    /// </summary>
    public class UpdateScheduleDto
    {
        public int ScheduleID { get; set; }
        public int ClassID { get; set; }
        public string Room { get; set; } = string.Empty;
        public DateTime ClassDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; } = "Active";
    }

    /// <summary>
    /// DTO cho Class (dùng khi tr? v? Schedule v?i Class info)
    /// </summary>
    public class ClassDto
    {
        public int ClassID { get; set; }
        public string ClassCode { get; set; } = string.Empty;
        public int CourseID { get; set; }
        public int Capacity { get; set; }
        public int EnrolledStudents { get; set; }
        public string Instructor { get; set; } = string.Empty;
        public string Status { get; set; } = "Active";
    }
}
