namespace PaymentService.Models;

public class Payroll
{
    public int Id { get; set; }
    public int TeacherId { get; set; }
    public string? TeacherName { get; set; }
    public string? Specialty { get; set; }
    public int TotalClasses { get; set; }
    public int TotalSessions { get; set; }
    public decimal BaseSalary { get; set; }
    public decimal Bonus { get; set; }
    public decimal Deduction { get; set; }
    public decimal NetSalary { get; set; }
    public string Status { get; set; } = "Pending"; // Pending | Paid
    public int Month { get; set; }
    public int Year { get; set; }
    public DateTime? PaidDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
