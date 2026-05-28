using Microsoft.EntityFrameworkCore;
using StudentAttendanceService.Data;
using StudentAttendanceService.Models;

namespace StudentAttendanceService.Services;

public class AttendanceService : IAttendanceService
{
    private readonly SchoolDbContext _context;

    public AttendanceService(SchoolDbContext context)
    {
        _context = context;
    }

    public async Task<List<Attendance>> GetAllAsync()
    {
        return await _context.Attendances.AsNoTracking().ToListAsync();
    }

    public async Task<Attendance?> GetByIdAsync(int id)
    {
        return await _context.Attendances.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Attendance> CreateAsync(Attendance attendance)
    {
        var studentExists = await _context.Students.AnyAsync(x => x.Id == attendance.StudentId);
        if (!studentExists)
        {
            throw new KeyNotFoundException("Student not found.");
        }

        if (attendance.Date == default)
        {
            attendance.Date = DateTime.Now;
        }

        _context.Attendances.Add(attendance);
        await _context.SaveChangesAsync();
        return attendance;
    }

    public async Task<bool> UpdateAsync(int id, Attendance attendance)
    {
        var existing = await _context.Attendances.FirstOrDefaultAsync(x => x.Id == id);
        if (existing == null)
        {
            return false;
        }

        var studentExists = await _context.Students.AnyAsync(x => x.Id == attendance.StudentId);
        if (!studentExists)
        {
            throw new KeyNotFoundException("Student not found.");
        }

        existing.StudentId = attendance.StudentId;
        existing.ScheduleId = attendance.ScheduleId;
        existing.Date = attendance.Date == default ? existing.Date : attendance.Date;
        existing.IsPresent = attendance.IsPresent;
        existing.Note = attendance.Note;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.Attendances.FirstOrDefaultAsync(x => x.Id == id);
        if (existing == null)
        {
            return false;
        }

        _context.Attendances.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
