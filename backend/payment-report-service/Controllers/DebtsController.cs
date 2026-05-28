using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.DTOs;
using PaymentService.Models;

namespace PaymentService.Controllers;

[ApiController]
[Route("api/debts")]
public class DebtsController : ControllerBase
{
    private readonly AppDbContext _db;

    public DebtsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<DebtDto>>>> GetAll(
        [FromQuery] int? studentId,
        [FromQuery] string? status)
    {
        var query = _db.Debts.AsQueryable();
        if (studentId.HasValue) query = query.Where(d => d.StudentId == studentId.Value);
        if (!string.IsNullOrEmpty(status)) query = query.Where(d => d.Status == status);

        var debts = await query
            .OrderByDescending(d => d.DaysOverdue)
            .Select(d => new DebtDto(
                d.Id, d.StudentId, d.StudentName, d.Email, d.CourseId, d.CourseName,
                d.TotalFee, d.RemainingAmount, d.DaysOverdue, d.Status, d.DueDate, d.CreatedAt
            ))
            .ToListAsync();

        return Ok(ApiResponse<List<DebtDto>>.Ok(debts));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<DebtDto>>> GetById(int id)
    {
        var d = await _db.Debts.FindAsync(id);
        if (d == null)
            return NotFound(ApiResponse<DebtDto>.NotFound("Debt not found."));

        return Ok(ApiResponse<DebtDto>.Ok(new DebtDto(
            d.Id, d.StudentId, d.StudentName, d.Email, d.CourseId, d.CourseName,
            d.TotalFee, d.RemainingAmount, d.DaysOverdue, d.Status, d.DueDate, d.CreatedAt
        )));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<DebtDto>>> Create([FromBody] CreateDebtRequest req)
    {
        var debt = new Debt
        {
            StudentId = req.StudentId,
            StudentName = req.StudentName,
            Email = req.Email,
            CourseId = req.CourseId,
            CourseName = req.CourseName,
            TotalFee = req.TotalFee,
            RemainingAmount = req.RemainingAmount,
            DaysOverdue = 0,
            Status = "Warning",
            DueDate = req.DueDate,
            CreatedAt = DateTime.UtcNow
        };

        _db.Debts.Add(debt);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = debt.Id },
            ApiResponse<DebtDto>.Created(new DebtDto(
                debt.Id, debt.StudentId, debt.StudentName, debt.Email, debt.CourseId, debt.CourseName,
                debt.TotalFee, debt.RemainingAmount, debt.DaysOverdue, debt.Status, debt.DueDate, debt.CreatedAt
            ), "Debt record created successfully."));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<DebtDto>>> Update(int id, [FromBody] UpdateDebtRequest req)
    {
        var d = await _db.Debts.FindAsync(id);
        if (d == null)
            return NotFound(ApiResponse<DebtDto>.NotFound("Debt not found."));

        if (req.RemainingAmount.HasValue) d.RemainingAmount = req.RemainingAmount.Value;
        if (req.DaysOverdue.HasValue) d.DaysOverdue = req.DaysOverdue.Value;
        if (!string.IsNullOrEmpty(req.Status)) d.Status = req.Status;

        await _db.SaveChangesAsync();

        return Ok(ApiResponse<DebtDto>.Ok(new DebtDto(
            d.Id, d.StudentId, d.StudentName, d.Email, d.CourseId, d.CourseName,
            d.TotalFee, d.RemainingAmount, d.DaysOverdue, d.Status, d.DueDate, d.CreatedAt
        ), "Debt updated successfully."));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        var d = await _db.Debts.FindAsync(id);
        if (d == null)
            return NotFound(ApiResponse<object>.NotFound("Debt not found."));

        _db.Debts.Remove(d);
        await _db.SaveChangesAsync();

        return Ok(ApiResponse.Ok("Debt deleted successfully."));
    }

    [HttpPost("{studentId}/reminder")]
    public ActionResult<ApiResponse<object>> SendReminder(int studentId)
    {
        // Trong thực tế sẽ gọi email service
        return Ok(ApiResponse.Ok($"Reminder sent to student ID {studentId}."));
    }
}

public record CreateDebtRequest(
    int StudentId,
    string StudentName,
    string Email,
    int CourseId,
    string CourseName,
    decimal TotalFee,
    decimal RemainingAmount,
    DateTime? DueDate
);

public record UpdateDebtRequest(decimal? RemainingAmount, int? DaysOverdue, string? Status);
