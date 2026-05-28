using CourseAndScheduleService.DTOs;

namespace CourseAndScheduleService.Services.Interfaces
{
    /// <summary>
    /// Interface ??nh ngh?a các operation cho Schedule Service
    /// </summary>
    public interface IScheduleService
    {
        /// <summary>
        /// L?y t?t c? schedules
        /// </summary>
        Task<IEnumerable<ScheduleDto>> GetAllSchedulesAsync();

        /// <summary>
        /// L?y schedule theo ID
        /// </summary>
        Task<ScheduleDto?> GetScheduleByIdAsync(int id);

        /// <summary>
        /// L?y t?t c? schedules c?a m?t l?p
        /// </summary>
        Task<IEnumerable<ScheduleDto>> GetSchedulesByClassIdAsync(int classId);

        /// <summary>
        /// L?y schedules theo ngày (ClassDate)
        /// </summary>
        Task<IEnumerable<ScheduleDto>> GetSchedulesByDateAsync(DateTime classDate);

        /// <summary>
        /// T?o schedule m?i
        /// </summary>
        Task<ScheduleDto> CreateScheduleAsync(CreateScheduleDto createScheduleDto);

        /// <summary>
        /// C?p nh?t schedule
        /// </summary>
        Task<ScheduleDto> UpdateScheduleAsync(UpdateScheduleDto updateScheduleDto);

        /// <summary>
        /// Xóa schedule theo ID
        /// </summary>
        Task<bool> DeleteScheduleAsync(int id);
    }
}
