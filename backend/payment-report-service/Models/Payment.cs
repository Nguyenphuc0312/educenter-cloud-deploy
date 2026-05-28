namespace PaymentService.Models;

public class Payment
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public string? StudentName { get; set; }
    public string? CourseName { get; set; }
    public decimal Amount { get; set; }
    public decimal PaidAmount { get; set; }
    public string Status { get; set; } = "Unpaid"; // Unpaid | Partial | Paid
    public string? PaymentMethod { get; set; } // Cash | BankTransfer | VNPay | MoMo
    public string? TransactionCode { get; set; }
    public DateTime? PaymentDate { get; set; }
    public DateTime? DueDate { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
