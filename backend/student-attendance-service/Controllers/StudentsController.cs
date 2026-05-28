using Microsoft.AspNetCore.Mvc;
using StudentAttendanceService.Models;
using StudentAttendanceService.Services;
using StudentAttendanceService.Shared;

namespace StudentAttendanceService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var students = await _studentService.GetAllAsync();
        return Ok(new ApiResponse<List<Student>>
        {
            Success = true,
            Message = "Get students successfully.",
            Data = students
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var student = await _studentService.GetByIdAsync(id);
        if (student == null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Student not found."
            });
        }

        return Ok(new ApiResponse<Student>
        {
            Success = true,
            Message = "Get student successfully.",
            Data = student
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Student student)
    {
        try
        {
            var created = await _studentService.CreateAsync(student);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, new ApiResponse<Student>
            {
                Success = true,
                Message = "Create student successfully.",
                Data = created
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Student student)
    {
        try
        {
            var updated = await _studentService.UpdateAsync(id, student);
            if (!updated)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Student not found."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Update student successfully."
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _studentService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Student not found."
            });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Delete student successfully."
        });
    }
}
