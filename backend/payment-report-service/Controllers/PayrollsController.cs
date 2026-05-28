using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.DTOs;
using PaymentService.Models;

namespace PaymentService.Controllers;

[ApiController]
[Route("api/payrolls")]
public class PayrollsController : ControllerBase
{
    private readonly AppDbContext _db;

    public PayrollsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<PayrollDto>>>> GetAll(
        [FromQuery] int? teacherId,
        [FromQuery] int? month,
        [FromQuery] int? year,
        [FromQuery] string? status)
    {
        var query = _db.Payrolls.AsQueryable();
        if (teacherId.HasValue) query = query.Where(p => p.TeacherId == teacherId.Value);
        if (month.HasValue) query = query.Where(p => p.Month == month.Value);
        if (year.HasValue) query = query.Where(p => p.Year == year.Value);
        if (!string.IsNullOrEmpty(status)) query = query.Where(p => p.Status == status);

        var payrolls = await query
            .OrderByDescending(p => p.Year)
            .ThenByDescending(p => p.Month)
            .Select(p => new PayrollDto(
                p.Id, p.TeacherId, p.TeacherName, p.Specialty,
                p.TotalClasses, p.TotalSessions,
                p.BaseSalary, p.Bonus, p.Deduction, p.NetSalary,
                p.Status, p.Month, p.Year, p.PaidDate, p.CreatedAt
            ))
            .ToListAsync();

        return Ok(ApiResponse<List<PayrollDto>>.Ok(payrolls));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<PayrollDto>>> GetById(int id)
    {
        var p = await _db.Payrolls.FindAsync(id);
        if (p == null)
            return NotFound(ApiResponse<PayrollDto>.NotFound("Payroll not found."));

        return Ok(ApiResponse<PayrollDto>.Ok(new PayrollDto(
            p.Id, p.TeacherId, p.TeacherName, p.Specialty,
            p.TotalClasses, p.TotalSessions,
            p.BaseSalary, p.Bonus, p.Deduction, p.NetSalary,
            p.Status, p.Month, p.Year, p.PaidDate, p.CreatedAt
        )));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<PayrollDto>>> Create([FromBody] CreatePayrollRequest req)
    {
        var payroll = new Payroll
        {
            TeacherId = req.TeacherId,
            TeacherName = req.TeacherName,
            Specialty = req.Specialty,
            TotalClasses = req.TotalClasses,
            TotalSessions = req.TotalSessions,
            BaseSalary = req.BaseSalary,
            Bonus = 0,
            Deduction = 0,
            NetSalary = req.BaseSalary,
            Status = "Pending",
            Month = req.Month,
            Year = req.Year,
            CreatedAt = DateTime.UtcNow
        };

        _db.Payrolls.Add(payroll);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = payroll.Id },
            ApiResponse<PayrollDto>.Created(new PayrollDto(
                payroll.Id, payroll.TeacherId, payroll.TeacherName, payroll.Specialty,
                payroll.TotalClasses, payroll.TotalSessions,
                payroll.BaseSalary, payroll.Bonus, payroll.Deduction, payroll.NetSalary,
                payroll.Status, payroll.Month, payroll.Year, payroll.PaidDate, payroll.CreatedAt
            ), "Payroll created successfully."));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<PayrollDto>>> Update(int id, [FromBody] UpdatePayrollRequest req)
    {
        var p = await _db.Payrolls.FindAsync(id);
        if (p == null)
            return NotFound(ApiResponse<PayrollDto>.NotFound("Payroll not found."));

        if (req.TotalClasses.HasValue) p.TotalClasses = req.TotalClasses.Value;
        if (req.TotalSessions.HasValue) p.TotalSessions = req.TotalSessions.Value;
        if (req.Bonus.HasValue) p.Bonus = req.Bonus.Value;
        if (req.Deduction.HasValue) p.Deduction = req.Deduction.Value;
        p.NetSalary = p.BaseSalary + p.Bonus - p.Deduction;

        await _db.SaveChangesAsync();

        return Ok(ApiResponse<PayrollDto>.Ok(new PayrollDto(
            p.Id, p.TeacherId, p.TeacherName, p.Specialty,
            p.TotalClasses, p.TotalSessions,
            p.BaseSalary, p.Bonus, p.Deduction, p.NetSalary,
            p.Status, p.Month, p.Year, p.PaidDate, p.CreatedAt
        ), "Payroll updated successfully."));
    }

    [HttpPost("{id}/mark-paid")]
    public async Task<ActionResult<ApiResponse<PayrollDto>>> MarkPaid(int id)
    {
        var p = await _db.Payrolls.FindAsync(id);
        if (p == null)
            return NotFound(ApiResponse<PayrollDto>.NotFound("Payroll not found."));

        p.Status = "Paid";
        p.PaidDate = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Ok(ApiResponse<PayrollDto>.Ok(new PayrollDto(
            p.Id, p.TeacherId, p.TeacherName, p.Specialty,
            p.TotalClasses, p.TotalSessions,
            p.BaseSalary, p.Bonus, p.Deduction, p.NetSalary,
            p.Status, p.Month, p.Year, p.PaidDate, p.CreatedAt
        ), "Payroll marked as paid."));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        var p = await _db.Payrolls.FindAsync(id);
        if (p == null)
            return NotFound(ApiResponse<object>.NotFound("Payroll not found."));

        _db.Payrolls.Remove(p);
        await _db.SaveChangesAsync();

        return Ok(ApiResponse<object>.Ok("Payroll deleted successfully."));
    }
}

public record CreatePayrollRequest(
    int TeacherId,
    string TeacherName,
    string Specialty,
    int TotalClasses,
    int TotalSessions,
    decimal BaseSalary,
    int Month,
    int Year
);

public record UpdatePayrollRequest(
    int? TotalClasses,
    int? TotalSessions,
    decimal? Bonus,
    decimal? Deduction
);
