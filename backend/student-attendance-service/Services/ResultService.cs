using Microsoft.EntityFrameworkCore;
using StudentAttendanceService.Data;
using StudentAttendanceService.Models;

namespace StudentAttendanceService.Services;

public class ResultService : IResultService
{
    private readonly SchoolDbContext _context;

    public ResultService(SchoolDbContext context)
    {
        _context = context;
    }

    public async Task<List<Result>> GetAllAsync()
    {
        return await _context.Results.AsNoTracking().ToListAsync();
    }

    public async Task<Result?> GetByIdAsync(int id)
    {
        return await _context.Results.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Result> CreateAsync(Result result)
    {
        var studentExists = await _context.Students.AnyAsync(x => x.Id == result.StudentId);
        if (!studentExists)
        {
            throw new KeyNotFoundException("Student not found.");
        }

        if (result.Score < 0 || result.Score > 10)
        {
            throw new InvalidOperationException("Score must be between 0 and 10.");
        }

        if (result.ResultDate == default)
        {
            result.ResultDate = DateTime.Now;
        }

        _context.Results.Add(result);
        await _context.SaveChangesAsync();
        return result;
    }

    public async Task<bool> UpdateAsync(int id, Result result)
    {
        var existing = await _context.Results.FirstOrDefaultAsync(x => x.Id == id);
        if (existing == null)
        {
            return false;
        }

        var studentExists = await _context.Students.AnyAsync(x => x.Id == result.StudentId);
        if (!studentExists)
        {
            throw new KeyNotFoundException("Student not found.");
        }

        if (result.Score < 0 || result.Score > 10)
        {
            throw new InvalidOperationException("Score must be between 0 and 10.");
        }

        existing.StudentId = result.StudentId;
        existing.CourseId = result.CourseId;
        existing.Score = result.Score;
        existing.ResultDate = result.ResultDate == default ? existing.ResultDate : result.ResultDate;
        existing.Note = result.Note;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.Results.FirstOrDefaultAsync(x => x.Id == id);
        if (existing == null)
        {
            return false;
        }

        _context.Results.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
