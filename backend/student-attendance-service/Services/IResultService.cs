using StudentAttendanceService.Models;

namespace StudentAttendanceService.Services;

public interface IResultService
{
    Task<List<Result>> GetAllAsync();
    Task<Result?> GetByIdAsync(int id);
    Task<Result> CreateAsync(Result result);
    Task<bool> UpdateAsync(int id, Result result);
    Task<bool> DeleteAsync(int id);
}
