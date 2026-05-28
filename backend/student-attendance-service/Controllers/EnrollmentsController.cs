using Microsoft.AspNetCore.Mvc;
using StudentAttendanceService.Models;
using StudentAttendanceService.Services;
using StudentAttendanceService.Shared;

namespace StudentAttendanceService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentsController(IEnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var enrollments = await _enrollmentService.GetAllAsync();
        return Ok(new ApiResponse<List<Enrollment>>
        {
            Success = true,
            Message = "Get enrollments successfully.",
            Data = enrollments
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var enrollment = await _enrollmentService.GetByIdAsync(id);
        if (enrollment == null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Enrollment not found."
            });
        }

        return Ok(new ApiResponse<Enrollment>
        {
            Success = true,
            Message = "Get enrollment successfully.",
            Data = enrollment
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Enrollment enrollment)
    {
        try
        {
            var created = await _enrollmentService.CreateAsync(enrollment);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, new ApiResponse<Enrollment>
            {
                Success = true,
                Message = "Create enrollment successfully.",
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
    public async Task<IActionResult> Update(int id, [FromBody] Enrollment enrollment)
    {
        try
        {
            var updated = await _enrollmentService.UpdateAsync(id, enrollment);
            if (!updated)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Enrollment not found."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Update enrollment successfully."
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
        var deleted = await _enrollmentService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Enrollment not found."
            });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Delete enrollment successfully."
        });
    }
}
