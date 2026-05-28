using CourseAndScheduleService.DTOs;
using CourseAndScheduleService.Responses;
using CourseAndScheduleService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CourseAndScheduleService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;
        private readonly ILogger<SchedulesController> _logger;

        public SchedulesController(IScheduleService scheduleService, ILogger<SchedulesController> logger)
        {
            _scheduleService = scheduleService;
            _logger = logger;
        }

        /// <summary>
        /// L?y t?t c? l?ch h?c
        /// GET: api/schedules
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ScheduleDto>>>> GetAllSchedules()
        {
            try
            {
                _logger.LogInformation("GetAllSchedules called");
                var schedules = await _scheduleService.GetAllSchedulesAsync();
                return Ok(ApiResponse<IEnumerable<ScheduleDto>>.SuccessResponse(
                    "L?y danh sách l?ch h?c thành công", schedules));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllSchedules");
                return BadRequest(ApiResponse<IEnumerable<ScheduleDto>>.ErrorResponse($"L?i: {ex.Message}"));
            }
        }

        /// <summary>
        /// L?y l?ch h?c theo ID
        /// GET: api/schedules/{id}
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ScheduleDto>>> GetScheduleById(int id)
        {
            try
            {
                _logger.LogInformation($"GetScheduleById called with id: {id}");
                var schedule = await _scheduleService.GetScheduleByIdAsync(id);

                if (schedule == null)
                {
                    return NotFound(ApiResponse<ScheduleDto>.ErrorResponse("L?ch h?c không t?n t?i"));
                }

                return Ok(ApiResponse<ScheduleDto>.SuccessResponse(
                    "L?y thông tin l?ch h?c thành công", schedule));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in GetScheduleById with id: {id}");
                return BadRequest(ApiResponse<ScheduleDto>.ErrorResponse($"L?i: {ex.Message}"));
            }
        }

        /// <summary>
        /// L?y l?ch h?c theo ID l?p
        /// GET: api/schedules/class/{classId}
        /// </summary>
        [HttpGet("class/{classId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ScheduleDto>>>> GetSchedulesByClassId(int classId)
        {
            try
            {
                _logger.LogInformation($"GetSchedulesByClassId called with classId: {classId}");
                var schedules = await _scheduleService.GetSchedulesByClassIdAsync(classId);
                return Ok(ApiResponse<IEnumerable<ScheduleDto>>.SuccessResponse(
                    "L?y danh sách l?ch h?c theo l?p thành công", schedules));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning($"Invalid argument in GetSchedulesByClassId: {ex.Message}");
                return BadRequest(ApiResponse<IEnumerable<ScheduleDto>>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in GetSchedulesByClassId with classId: {classId}");
                return BadRequest(ApiResponse<IEnumerable<ScheduleDto>>.ErrorResponse($"L?i: {ex.Message}"));
            }
        }

        /// <summary>
        /// L?y l?ch h?c theo ngày
        /// GET: api/schedules/date/{date}
        /// </summary>
        [HttpGet("date/{date}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ScheduleDto>>>> GetSchedulesByDate(string date)
        {
            try
            {
                _logger.LogInformation($"GetSchedulesByDate called with date: {date}");

                if (!DateTime.TryParse(date, out var parsedDate))
                {
                    return BadRequest(ApiResponse<IEnumerable<ScheduleDto>>.ErrorResponse(
                        "??nh d?ng ngày không h?p l?. S? d?ng: yyyy-MM-dd"));
                }

                var schedules = await _scheduleService.GetSchedulesByDateAsync(parsedDate);
                return Ok(ApiResponse<IEnumerable<ScheduleDto>>.SuccessResponse(
                    "L?y danh sách l?ch h?c theo ngày thành công", schedules));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in GetSchedulesByDate with date: {date}");
                return BadRequest(ApiResponse<IEnumerable<ScheduleDto>>.ErrorResponse($"L?i: {ex.Message}"));
            }
        }

        /// <summary>
        /// T?o l?ch h?c m?i
        /// POST: api/schedules
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<ScheduleDto>>> CreateSchedule([FromBody] CreateScheduleDto createScheduleDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<ScheduleDto>.ErrorResponse("D? li?u không h?p l?"));
                }

                _logger.LogInformation($"CreateSchedule called for classId: {createScheduleDto.ClassID}");
                var schedule = await _scheduleService.CreateScheduleAsync(createScheduleDto);

                return CreatedAtAction(nameof(GetScheduleById), new { id = schedule.ScheduleID },
                    ApiResponse<ScheduleDto>.SuccessResponse("T?o l?ch h?c thành công", schedule));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning($"Invalid argument in CreateSchedule: {ex.Message}");
                return BadRequest(ApiResponse<ScheduleDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateSchedule");
                return BadRequest(ApiResponse<ScheduleDto>.ErrorResponse($"L?i: {ex.Message}"));
            }
        }

        /// <summary>
        /// C?p nh?t l?ch h?c
        /// PUT: api/schedules/{id}
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<ScheduleDto>>> UpdateSchedule(int id, [FromBody] UpdateScheduleDto updateScheduleDto)
        {
            try
            {
                if (id != updateScheduleDto.ScheduleID)
                {
                    return BadRequest(ApiResponse<ScheduleDto>.ErrorResponse("ID trong URL và body không kh?p"));
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<ScheduleDto>.ErrorResponse("D? li?u không h?p l?"));
                }

                _logger.LogInformation($"UpdateSchedule called with id: {id}");
                var schedule = await _scheduleService.UpdateScheduleAsync(updateScheduleDto);

                return Ok(ApiResponse<ScheduleDto>.SuccessResponse("C?p nh?t l?ch h?c thành công", schedule));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning($"Invalid argument in UpdateSchedule: {ex.Message}");
                return BadRequest(ApiResponse<ScheduleDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in UpdateSchedule with id: {id}");
                return BadRequest(ApiResponse<ScheduleDto>.ErrorResponse($"L?i: {ex.Message}"));
            }
        }

        /// <summary>
        /// Xóa l?ch h?c
        /// DELETE: api/schedules/{id}
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteSchedule(int id)
        {
            try
            {
                _logger.LogInformation($"DeleteSchedule called with id: {id}");
                var result = await _scheduleService.DeleteScheduleAsync(id);

                if (!result)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse("L?ch h?c không t?n t?i"));
                }

                return Ok(ApiResponse<object>.SuccessResponse("Xóa l?ch h?c thành công"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in DeleteSchedule with id: {id}");
                return BadRequest(ApiResponse<object>.ErrorResponse($"L?i: {ex.Message}"));
            }
        }
    }
}
