using System.ComponentModel.DataAnnotations;

namespace PaymentService.DTOs;

// === Auth ===
public record LoginRequest([Required] string Email, [Required] string Password, string? Role = null);

public record LoginResponse(string Token, string Role, UserDto User);

// === User ===
public record UserDto(
    int Id,
    string FullName,
    string Email,
    string Role,
    string? Phone,
    string Status,
    DateTime CreatedAt
);

public record CreateUserRequest(
    [Required] string FullName,
    [Required][EmailAddress] string Email,
    [Required] string Password,
    [Required] string Role,
    string? Phone
);

public record UpdateUserRequest(
    string? FullName,
    string? Email,
    string? Password,
    string? Role,
    string? Phone,
    string? Status
);

// === Payment ===
public record PaymentDto(
    int Id,
    int StudentId,
    int CourseId,
    string? StudentName,
    string? CourseName,
    decimal Amount,
    decimal PaidAmount,
    string Status,
    string? PaymentMethod,
    string? TransactionCode,
    DateTime? PaymentDate,
    DateTime? DueDate,
    string? Note,
    DateTime CreatedAt
);

public record CreatePaymentRequest(
    [Required] int StudentId,
    [Required] int CourseId,
    [Required] decimal Amount,
    DateTime? DueDate,
    string? Note
);

public record UpdatePaymentRequest(
    decimal? Amount,
    decimal? PaidAmount,
    string? Status,
    string? PaymentMethod,
    string? TransactionCode,
    DateTime? PaymentDate,
    DateTime? DueDate,
    string? Note
);

public record PayPaymentRequest(
    [Required] decimal PaidAmount,
    [Required] string PaymentMethod, // Cash | BankTransfer | VNPay | MoMo
    string? TransactionCode,
    string? Note
);

// === Debt ===
public record DebtDto(
    int Id,
    int StudentId,
    string? StudentName,
    string? Email,
    int CourseId,
    string? CourseName,
    decimal TotalFee,
    decimal RemainingAmount,
    int DaysOverdue,
    string Status,
    DateTime? DueDate,
    DateTime CreatedAt
);

// === Report ===
public record DashboardDto(
    int TotalStudents,
    decimal TotalRevenue,
    decimal TotalDebt,
    int ActiveCourses,
    int PaidPayments,
    int UnpaidPayments,
    List<RevenueByMonthDto> RevenueByMonth,
    List<RevenueByCategoryDto> RevenueByCategory
);

public record RevenueByMonthDto(string Month, decimal Revenue, decimal Target);
public record RevenueByCategoryDto(string Category, decimal Revenue);

// === Payroll ===
public record PayrollDto(
    int Id,
    int TeacherId,
    string? TeacherName,
    string? Specialty,
    int TotalClasses,
    int TotalSessions,
    decimal BaseSalary,
    decimal Bonus,
    decimal Deduction,
    decimal NetSalary,
    string Status,
    int Month,
    int Year,
    DateTime? PaidDate,
    DateTime CreatedAt
);

public record PayPayrollRequest([Required] decimal Bonus, decimal Deduction);
