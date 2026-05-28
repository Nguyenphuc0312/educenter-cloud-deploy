using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.DTOs;
using PaymentService.Models;

namespace PaymentService.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly AppDbContext _db;

    public PaymentsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<PaymentDto>>>> GetAll(
        [FromQuery] int? studentId,
        [FromQuery] int? courseId,
        [FromQuery] string? status)
    {
        var query = _db.Payments.AsQueryable();
        if (studentId.HasValue) query = query.Where(p => p.StudentId == studentId.Value);
        if (courseId.HasValue) query = query.Where(p => p.CourseId == courseId.Value);
        if (!string.IsNullOrEmpty(status)) query = query.Where(p => p.Status == status);

        var payments = await query
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new PaymentDto(
                p.Id, p.StudentId, p.CourseId, p.StudentName, p.CourseName,
                p.Amount, p.PaidAmount, p.Status, p.PaymentMethod,
                p.TransactionCode, p.PaymentDate, p.DueDate, p.Note, p.CreatedAt
            ))
            .ToListAsync();

        return Ok(ApiResponse<List<PaymentDto>>.Ok(payments));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<PaymentDto>>> GetById(int id)
    {
        var p = await _db.Payments.FindAsync(id);
        if (p == null)
            return NotFound(ApiResponse<PaymentDto>.NotFound("Payment not found."));

        return Ok(ApiResponse<PaymentDto>.Ok(new PaymentDto(
            p.Id, p.StudentId, p.CourseId, p.StudentName, p.CourseName,
            p.Amount, p.PaidAmount, p.Status, p.PaymentMethod,
            p.TransactionCode, p.PaymentDate, p.DueDate, p.Note, p.CreatedAt
        )));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<PaymentDto>>> Create([FromBody] CreatePaymentRequest req)
    {
        var payment = new Payment
        {
            StudentId = req.StudentId,
            CourseId = req.CourseId,
            Amount = req.Amount,
            PaidAmount = 0,
            Status = "Unpaid",
            DueDate = req.DueDate,
            Note = req.Note,
            CreatedAt = DateTime.UtcNow
        };

        _db.Payments.Add(payment);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = payment.Id },
            ApiResponse<PaymentDto>.Created(new PaymentDto(
                payment.Id, payment.StudentId, payment.CourseId, payment.StudentName, payment.CourseName,
                payment.Amount, payment.PaidAmount, payment.Status, payment.PaymentMethod,
                payment.TransactionCode, payment.PaymentDate, payment.DueDate, payment.Note, payment.CreatedAt
            ), "Payment created successfully."));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<PaymentDto>>> Update(int id, [FromBody] UpdatePaymentRequest req)
    {
        var p = await _db.Payments.FindAsync(id);
        if (p == null)
            return NotFound(ApiResponse<PaymentDto>.NotFound("Payment not found."));

        if (req.Amount.HasValue) p.Amount = req.Amount.Value;
        if (req.PaidAmount.HasValue) p.PaidAmount = req.PaidAmount.Value;
        if (!string.IsNullOrEmpty(req.Status)) p.Status = req.Status;
        if (req.PaymentMethod != null) p.PaymentMethod = req.PaymentMethod;
        if (req.TransactionCode != null) p.TransactionCode = req.TransactionCode;
        if (req.PaymentDate.HasValue) p.PaymentDate = req.PaymentDate;
        if (req.DueDate.HasValue) p.DueDate = req.DueDate;
        if (req.Note != null) p.Note = req.Note;
        p.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Ok(ApiResponse<PaymentDto>.Ok(new PaymentDto(
            p.Id, p.StudentId, p.CourseId, p.StudentName, p.CourseName,
            p.Amount, p.PaidAmount, p.Status, p.PaymentMethod,
            p.TransactionCode, p.PaymentDate, p.DueDate, p.Note, p.CreatedAt
        ), "Payment updated successfully."));
    }

    [HttpPost("{id}/pay")]
    public async Task<ActionResult<ApiResponse<PaymentDto>>> Pay(int id, [FromBody] PayPaymentRequest req)
    {
        var p = await _db.Payments.FindAsync(id);
        if (p == null)
            return NotFound(ApiResponse<PaymentDto>.NotFound("Payment not found."));

        p.PaidAmount += req.PaidAmount;
        p.PaymentMethod = req.PaymentMethod;
        p.TransactionCode = req.TransactionCode ?? $"TXN{DateTime.UtcNow:yyyyMMddHHmmss}";
        p.PaymentDate = DateTime.UtcNow;
        p.Note = req.Note;
        p.UpdatedAt = DateTime.UtcNow;

        if (p.PaidAmount >= p.Amount)
        {
            p.Status = "Paid";
            p.PaidAmount = p.Amount;
        }
        else
        {
            p.Status = "Partial";
        }

        await _db.SaveChangesAsync();

        return Ok(ApiResponse<PaymentDto>.Ok(new PaymentDto(
            p.Id, p.StudentId, p.CourseId, p.StudentName, p.CourseName,
            p.Amount, p.PaidAmount, p.Status, p.PaymentMethod,
            p.TransactionCode, p.PaymentDate, p.DueDate, p.Note, p.CreatedAt
        ), "Payment successful."));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        var p = await _db.Payments.FindAsync(id);
        if (p == null)
            return NotFound(ApiResponse<object>.NotFound("Payment not found."));

        _db.Payments.Remove(p);
        await _db.SaveChangesAsync();

        return Ok(ApiResponse.Ok("Payment deleted successfully."));
    }
}
