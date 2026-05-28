using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StudentAttendanceService.Models;

public class Enrollment
{
    public int Id { get; set; }

    [Required]
    public int StudentId { get; set; }

    [Required]
    public int CourseId { get; set; }

    public DateTime EnrolledDate { get; set; } = DateTime.Now;

    [MaxLength(50)]
    public string Status { get; set; } = "Enrolled";

    [JsonIgnore]
    public Student? Student { get; set; }
}
