namespace PaymentService.Models;

public class Debt
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string? StudentName { get; set; }
    public string? Email { get; set; }
    public int CourseId { get; set; }
    public string? CourseName { get; set; }
    public decimal TotalFee { get; set; }
    public decimal RemainingAmount { get; set; }
    public int DaysOverdue { get; set; }
    public string Status { get; set; } = "Overdue"; // Overdue | Warning | Cleared
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
