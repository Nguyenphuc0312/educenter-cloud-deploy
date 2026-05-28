using Microsoft.AspNetCore.Mvc;
using StudentAttendanceService.Models;
using StudentAttendanceService.Services;
using StudentAttendanceService.Shared;

namespace StudentAttendanceService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttendancesController : ControllerBase
{
    private readonly IAttendanceService _attendanceService;

    public AttendancesController(IAttendanceService attendanceService)
    {
        _attendanceService = attendanceService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var attendances = await _attendanceService.GetAllAsync();
        return Ok(new ApiResponse<List<Attendance>>
        {
            Success = true,
            Message = "Get attendances successfully.",
            Data = attendances
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var attendance = await _attendanceService.GetByIdAsync(id);
        if (attendance == null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Attendance not found."
            });
        }

        return Ok(new ApiResponse<Attendance>
        {
            Success = true,
            Message = "Get attendance successfully.",
            Data = attendance
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Attendance attendance)
    {
        try
        {
            var created = await _attendanceService.CreateAsync(attendance);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, new ApiResponse<Attendance>
            {
                Success = true,
                Message = "Create attendance successfully.",
                Data = created
            });
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Attendance attendance)
    {
        try
        {
            var updated = await _attendanceService.UpdateAsync(id, attendance);
            if (!updated)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Attendance not found."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Update attendance successfully."
            });
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _attendanceService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Attendance not found."
            });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Delete attendance successfully."
        });
    }
}
