using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StudentAttendanceService.Models;

public class Result
{
    public int Id { get; set; }

    [Required]
    public int StudentId { get; set; }

    [Required]
    public int CourseId { get; set; }

    [Range(typeof(decimal), "0", "10")]
    public decimal Score { get; set; }

    public DateTime ResultDate { get; set; } = DateTime.Now;

    [MaxLength(250)]
    public string? Note { get; set; }

    [JsonIgnore]
    public Student? Student { get; set; }
}
