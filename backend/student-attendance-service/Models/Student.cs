using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StudentAttendanceService.Models;

public class Student
{
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Phone { get; set; }

    public DateTime DateOfBirth { get; set; }

    [MaxLength(250)]
    public string? Address { get; set; }

    public DateTime EnrolledAt { get; set; } = DateTime.Now;

    [MaxLength(50)]
    public string Status { get; set; } = "Active";

    [JsonIgnore]
    public List<Enrollment> Enrollments { get; set; } = new();

    [JsonIgnore]
    public List<Attendance> Attendances { get; set; } = new();

    [JsonIgnore]
    public List<Result> Results { get; set; } = new();
}
