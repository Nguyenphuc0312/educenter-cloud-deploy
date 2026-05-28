using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.DTOs;

namespace PaymentService.Controllers;

[ApiController]
[Route("api/reports")]
public class ReportsController : ControllerBase
{
    private readonly AppDbContext _db;

    public ReportsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<DashboardDto>>> GetDashboard()
    {
        var totalStudents = await _db.Users.CountAsync(u => u.Role == "student");
        var totalRevenue = await _db.Payments
            .Where(p => p.Status == "Paid")
            .SumAsync(p => p.PaidAmount);
        var totalDebt = await _db.Payments
            .Where(p => p.Status != "Paid")
            .SumAsync(p => p.Amount - p.PaidAmount);
        var paidCount = await _db.Payments.CountAsync(p => p.Status == "Paid");
        var unpaidCount = await _db.Payments.CountAsync(p => p.Status != "Paid");
        var activeCourses = await _db.Payments.Select(p => p.CourseId).Distinct().CountAsync();

        // Revenue by month (mock last 6 months)
        var revenueByMonth = new List<RevenueByMonthDto>
        {
            new("T12/2025", 185000000, 180000000),
            new("T1/2026", 245000000, 220000000),
            new("T2/2026", 198000000, 200000000),
            new("T3/2026", 312000000, 280000000),
            new("T4/2026", 278000000, 300000000),
            new("T5/2026", 295000000, 290000000),
        };

        // Revenue by category
        var revenueByCategory = await _db.Payments
            .Where(p => p.Status == "Paid" && p.CourseName != null)
            .ToListAsync();
        
        var categoryGroups = revenueByCategory
            .GroupBy(p => p.CourseName!.Split(' ')[0])
            .Select(g => new RevenueByCategoryDto(g.Key, g.Sum(p => p.PaidAmount)))
            .ToList();

        var dashboard = new DashboardDto(
            totalStudents, totalRevenue, totalDebt, activeCourses,
            paidCount, unpaidCount, revenueByMonth, categoryGroups
        );

        return Ok(ApiResponse<DashboardDto>.Ok(dashboard, "Dashboard loaded successfully."));
    }

    [HttpGet("revenue")]
    public async Task<ActionResult<ApiResponse<object>>> GetRevenueReport(
        [FromQuery] int? year,
        [FromQuery] int? courseId)
    {
        var query = _db.Payments.Where(p => p.Status == "Paid");
        if (year.HasValue) query = query.Where(p => p.PaymentDate!.Value.Year == year.Value);
        if (courseId.HasValue) query = query.Where(p => p.CourseId == courseId.Value);

        var total = await query.SumAsync(p => p.PaidAmount);
        var payments = await query.ToListAsync();
        var byMonth = payments
            .GroupBy(p => new { Year = p.PaymentDate!.Value.Year, Month = p.PaymentDate!.Value.Month })
            .Select(g => new { MonthLabel = $"T{g.Key.Month}/{g.Key.Year}", Revenue = g.Sum(p => p.PaidAmount), g.Key.Year, g.Key.Month })
            .OrderBy(g => g.Year).ThenBy(g => g.Month)
            .ToList();

        return Ok(ApiResponse<object>.Ok(new { totalRevenue = total, byMonth }));
    }

    [HttpGet("by-course")]
    public async Task<ActionResult<ApiResponse<object>>> GetByCourse()
    {
        var byCourse = await _db.Payments
            .GroupBy(p => p.CourseName ?? "Unknown")
            .Select(g => new
            {
                courseName = g.Key,
                totalAmount = g.Sum(p => p.Amount),
                paidAmount = g.Where(p => p.Status == "Paid").Sum(p => p.PaidAmount),
                unpaidAmount = g.Sum(p => p.Amount - p.PaidAmount),
                studentCount = g.Select(p => p.StudentId).Distinct().Count()
            })
            .OrderByDescending(g => g.totalAmount)
            .ToListAsync();

        return Ok(ApiResponse<object>.Ok(byCourse));
    }

    [HttpGet("by-class")]
    public async Task<ActionResult<ApiResponse<object>>> GetByClass()
    {
        var byClass = await _db.Payments
            .Where(p => p.CourseName != null)
            .GroupBy(p => p.CourseName!)
            .Select(g => new
            {
                className = g.Key,
                totalRevenue = g.Where(p => p.Status == "Paid").Sum(p => p.PaidAmount),
                totalDebt = g.Sum(p => p.Amount - p.PaidAmount),
                paymentCount = g.Count(),
                paidCount = g.Count(p => p.Status == "Paid")
            })
            .OrderByDescending(g => g.totalRevenue)
            .ToListAsync();

        return Ok(ApiResponse<object>.Ok(byClass));
    }
}
