using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StudentAttendanceService.Models;

public class Attendance
{
    public int Id { get; set; }

    [Required]
    public int StudentId { get; set; }

    [Required]
    public int ScheduleId { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;

    public bool IsPresent { get; set; }

    [MaxLength(250)]
    public string? Note { get; set; }

    [JsonIgnore]
    public Student? Student { get; set; }
}
