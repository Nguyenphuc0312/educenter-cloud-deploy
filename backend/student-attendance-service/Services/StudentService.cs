using Microsoft.EntityFrameworkCore;
using StudentAttendanceService.Data;
using StudentAttendanceService.Models;

namespace StudentAttendanceService.Services;

public class StudentService : IStudentService
{
    private readonly SchoolDbContext _context;

    public StudentService(SchoolDbContext context)
    {
        _context = context;
    }

    public async Task<List<Student>> GetAllAsync()
    {
        return await _context.Students.AsNoTracking().ToListAsync();
    }

    public async Task<Student?> GetByIdAsync(int id)
    {
        return await _context.Students.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Student> CreateAsync(Student student)
    {
        var emailExists = await _context.Students.AnyAsync(x => x.Email == student.Email);
        if (emailExists)
        {
            throw new InvalidOperationException("Email already exists.");
        }

        if (student.EnrolledAt == default)
        {
            student.EnrolledAt = DateTime.Now;
        }

        _context.Students.Add(student);
        await _context.SaveChangesAsync();
        return student;
    }

    public async Task<bool> UpdateAsync(int id, Student student)
    {
        var existing = await _context.Students.FirstOrDefaultAsync(x => x.Id == id);
        if (existing == null)
        {
            return false;
        }

        var emailExists = await _context.Students.AnyAsync(x => x.Email == student.Email && x.Id != id);
        if (emailExists)
        {
            throw new InvalidOperationException("Email already exists.");
        }

        existing.FullName = student.FullName;
        existing.Email = student.Email;
        existing.Phone = student.Phone;
        existing.DateOfBirth = student.DateOfBirth;
        existing.Address = student.Address;
        existing.EnrolledAt = student.EnrolledAt == default ? existing.EnrolledAt : student.EnrolledAt;
        existing.Status = student.Status;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.Students.FirstOrDefaultAsync(x => x.Id == id);
        if (existing == null)
        {
            return false;
        }

        _context.Students.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
