using CourseAndScheduleService.Data;
using CourseAndScheduleService.DTOs;
using CourseAndScheduleService.Models;
using CourseAndScheduleService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseAndScheduleService.Services.Implementations
{
    /// <summary>
    /// Service x? lý logic CRUD cho Schedule
    /// </summary>
    public class ScheduleService : IScheduleService
    {
        private readonly EduCenterDbContext _context;
        private readonly ILogger<ScheduleService> _logger;

        public ScheduleService(EduCenterDbContext context, ILogger<ScheduleService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// L?y t?t c? schedules
        /// </summary>
        public async Task<IEnumerable<ScheduleDto>> GetAllSchedulesAsync()
        {
            try
            {
                _logger.LogInformation("Getting all schedules");

                var schedules = await _context.Schedules
                    .Include(s => s.Class)
                    .OrderBy(s => s.ClassDate)
                    .ThenBy(s => s.StartTime)
                    .ToListAsync();

                return schedules.Select(s => MapToDto(s)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all schedules");
                throw;
            }
        }

        /// <summary>
        /// L?y schedule theo ID
        /// </summary>
        public async Task<ScheduleDto?> GetScheduleByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Getting schedule with ID: {id}");

                var schedule = await _context.Schedules
                    .Include(s => s.Class)
                    .FirstOrDefaultAsync(s => s.ScheduleID == id);

                if (schedule == null)
                {
                    _logger.LogWarning($"Schedule with ID {id} not found");
                    return null;
                }

                return MapToDto(schedule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting schedule with ID {id}");
                throw;
            }
        }

        /// <summary>
        /// L?y t?t c? schedules c?a m?t l?p
        /// </summary>
        public async Task<IEnumerable<ScheduleDto>> GetSchedulesByClassIdAsync(int classId)
        {
            try
            {
                _logger.LogInformation($"Getting schedules for class ID: {classId}");

                // Ki?m tra l?p có t?n t?i không
                var classExists = await _context.Classes.AnyAsync(c => c.ClassID == classId);
                if (!classExists)
                {
                    _logger.LogWarning($"Class with ID {classId} not found");
                    throw new ArgumentException($"L?p v?i ID {classId} không t?n t?i");
                }

                var schedules = await _context.Schedules
                    .Where(s => s.ClassID == classId)
                    .Include(s => s.Class)
                    .OrderBy(s => s.ClassDate)
                    .ThenBy(s => s.StartTime)
                    .ToListAsync();

                return schedules.Select(s => MapToDto(s)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting schedules for class ID {classId}");
                throw;
            }
        }

        /// <summary>
        /// L?y schedules theo ngày (ClassDate)
        /// </summary>
        public async Task<IEnumerable<ScheduleDto>> GetSchedulesByDateAsync(DateTime classDate)
        {
            try
            {
                _logger.LogInformation($"Getting schedules for date: {classDate:yyyy-MM-dd}");

                var schedules = await _context.Schedules
                    .Where(s => s.ClassDate.Date == classDate.Date)
                    .Include(s => s.Class)
                    .OrderBy(s => s.StartTime)
                    .ToListAsync();

                return schedules.Select(s => MapToDto(s)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting schedules for date {classDate:yyyy-MM-dd}");
                throw;
            }
        }

        /// <summary>
        /// T?o schedule m?i
        /// </summary>
        public async Task<ScheduleDto> CreateScheduleAsync(CreateScheduleDto createScheduleDto)
        {
            try
            {
                _logger.LogInformation($"Creating new schedule for class ID: {createScheduleDto.ClassID}");

                // Validation
                ValidateScheduleData(createScheduleDto.StartTime, createScheduleDto.EndTime, createScheduleDto.ClassDate);

                // Ki?m tra l?p có t?n t?i không
                var classEntity = await _context.Classes.FindAsync(createScheduleDto.ClassID);
                if (classEntity == null)
                {
                    _logger.LogWarning($"Class with ID {createScheduleDto.ClassID} not found");
                    throw new ArgumentException($"L?p v?i ID {createScheduleDto.ClassID} không t?n t?i");
                }

                var schedule = new Schedule
                {
                    ClassID = createScheduleDto.ClassID,
                    Room = createScheduleDto.Room,
                    ClassDate = createScheduleDto.ClassDate,
                    StartTime = createScheduleDto.StartTime,
                    EndTime = createScheduleDto.EndTime,
                    Status = createScheduleDto.Status,
                    CreatedAt = DateTime.Now
                };

                _context.Schedules.Add(schedule);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Schedule created successfully with ID: {schedule.ScheduleID}");

                // Reload ?? l?y navigation properties
                await _context.Entry(schedule).Reference(s => s.Class).LoadAsync();

                return MapToDto(schedule);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating schedule");
                throw;
            }
        }

        /// <summary>
        /// C?p nh?t schedule
        /// </summary>
        public async Task<ScheduleDto> UpdateScheduleAsync(UpdateScheduleDto updateScheduleDto)
        {
            try
            {
                _logger.LogInformation($"Updating schedule with ID: {updateScheduleDto.ScheduleID}");

                // Validation
                ValidateScheduleData(updateScheduleDto.StartTime, updateScheduleDto.EndTime, updateScheduleDto.ClassDate);

                var schedule = await _context.Schedules.FindAsync(updateScheduleDto.ScheduleID);
                if (schedule == null)
                {
                    _logger.LogWarning($"Schedule with ID {updateScheduleDto.ScheduleID} not found");
                    throw new ArgumentException($"L?ch h?c v?i ID {updateScheduleDto.ScheduleID} không t?n t?i");
                }

                // Ki?m tra l?p có t?n t?i không n?u ClassID thay ??i
                if (schedule.ClassID != updateScheduleDto.ClassID)
                {
                    var classEntity = await _context.Classes.FindAsync(updateScheduleDto.ClassID);
                    if (classEntity == null)
                    {
                        _logger.LogWarning($"Class with ID {updateScheduleDto.ClassID} not found");
                        throw new ArgumentException($"L?p v?i ID {updateScheduleDto.ClassID} không t?n t?i");
                    }
                }

                schedule.ClassID = updateScheduleDto.ClassID;
                schedule.Room = updateScheduleDto.Room;
                schedule.ClassDate = updateScheduleDto.ClassDate;
                schedule.StartTime = updateScheduleDto.StartTime;
                schedule.EndTime = updateScheduleDto.EndTime;
                schedule.Status = updateScheduleDto.Status;
                schedule.UpdatedAt = DateTime.Now;

                _context.Schedules.Update(schedule);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Schedule with ID {updateScheduleDto.ScheduleID} updated successfully");

                // Reload ?? l?y navigation properties
                await _context.Entry(schedule).Reference(s => s.Class).LoadAsync();

                return MapToDto(schedule);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating schedule with ID {updateScheduleDto.ScheduleID}");
                throw;
            }
        }

        /// <summary>
        /// Xóa schedule theo ID
        /// </summary>
        public async Task<bool> DeleteScheduleAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting schedule with ID: {id}");

                var schedule = await _context.Schedules.FindAsync(id);
                if (schedule == null)
                {
                    _logger.LogWarning($"Schedule with ID {id} not found");
                    return false;
                }

                _context.Schedules.Remove(schedule);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Schedule with ID {id} deleted successfully");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting schedule with ID {id}");
                throw;
            }
        }

        /// <summary>
        /// Validate d? li?u schedule
        /// </summary>
        private void ValidateScheduleData(TimeSpan startTime, TimeSpan endTime, DateTime classDate)
        {
            if (startTime >= endTime)
            {
                throw new ArgumentException("Th?i gian b?t ??u ph?i tr??c th?i gian k?t thúc");
            }

            if (classDate < DateTime.Today)
            {
                throw new ArgumentException("Ngày h?c không th? là ngày quá kh?");
            }
        }

        /// <summary>
        /// Map Schedule entity to ScheduleDto
        /// </summary>
        private ScheduleDto MapToDto(Schedule schedule)
        {
            return new ScheduleDto
            {
                ScheduleID = schedule.ScheduleID,
                ClassID = schedule.ClassID,
                Room = schedule.Room,
                ClassDate = schedule.ClassDate,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                Status = schedule.Status,
                CreatedAt = schedule.CreatedAt,
                UpdatedAt = schedule.UpdatedAt,
                Class = schedule.Class != null ? new ClassDto
                {
                    ClassID = schedule.Class.ClassID,
                    ClassCode = schedule.Class.ClassCode,
                    CourseID = schedule.Class.CourseID,
                    Capacity = schedule.Class.Capacity,
                    EnrolledStudents = schedule.Class.EnrolledStudents,
                    Instructor = schedule.Class.Instructor,
                    Status = schedule.Class.Status
                } : null
            };
        }
    }
}
