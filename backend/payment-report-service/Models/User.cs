namespace PaymentService.Models;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty; // hashed
    public string Role { get; set; } = string.Empty; // admin | teacher | student
    public string? Phone { get; set; }
    public string Status { get; set; } = "Active"; // Active | Inactive
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
