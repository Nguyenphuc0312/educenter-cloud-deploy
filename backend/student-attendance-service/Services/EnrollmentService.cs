using Microsoft.EntityFrameworkCore;
using StudentAttendanceService.Data;
using StudentAttendanceService.Models;

namespace StudentAttendanceService.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly SchoolDbContext _context;

    public EnrollmentService(SchoolDbContext context)
    {
        _context = context;
    }

    public async Task<List<Enrollment>> GetAllAsync()
    {
        return await _context.Enrollments.AsNoTracking().ToListAsync();
    }

    public async Task<Enrollment?> GetByIdAsync(int id)
    {
        return await _context.Enrollments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Enrollment> CreateAsync(Enrollment enrollment)
    {
        var studentExists = await _context.Students.AnyAsync(x => x.Id == enrollment.StudentId);
        if (!studentExists)
        {
            throw new KeyNotFoundException("Student not found.");
        }

        if (enrollment.EnrolledDate == default)
        {
            enrollment.EnrolledDate = DateTime.Now;
        }

        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();
        return enrollment;
    }

    public async Task<bool> UpdateAsync(int id, Enrollment enrollment)
    {
        var existing = await _context.Enrollments.FirstOrDefaultAsync(x => x.Id == id);
        if (existing == null)
        {
            return false;
        }

        var studentExists = await _context.Students.AnyAsync(x => x.Id == enrollment.StudentId);
        if (!studentExists)
        {
            throw new KeyNotFoundException("Student not found.");
        }

        existing.StudentId = enrollment.StudentId;
        existing.CourseId = enrollment.CourseId;
        existing.EnrolledDate = enrollment.EnrolledDate == default ? existing.EnrolledDate : enrollment.EnrolledDate;
        existing.Status = enrollment.Status;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.Enrollments.FirstOrDefaultAsync(x => x.Id == id);
        if (existing == null)
        {
            return false;
        }

        _context.Enrollments.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
