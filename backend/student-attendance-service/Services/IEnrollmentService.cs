using StudentAttendanceService.Models;

namespace StudentAttendanceService.Services;

public interface IEnrollmentService
{
    Task<List<Enrollment>> GetAllAsync();
    Task<Enrollment?> GetByIdAsync(int id);
    Task<Enrollment> CreateAsync(Enrollment enrollment);
    Task<bool> UpdateAsync(int id, Enrollment enrollment);
    Task<bool> DeleteAsync(int id);
}
