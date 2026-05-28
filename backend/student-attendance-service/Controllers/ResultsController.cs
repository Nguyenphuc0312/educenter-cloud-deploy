using Microsoft.AspNetCore.Mvc;
using StudentAttendanceService.Models;
using StudentAttendanceService.Services;
using StudentAttendanceService.Shared;

namespace StudentAttendanceService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResultsController : ControllerBase
{
    private readonly IResultService _resultService;

    public ResultsController(IResultService resultService)
    {
        _resultService = resultService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var results = await _resultService.GetAllAsync();
        return Ok(new ApiResponse<List<Result>>
        {
            Success = true,
            Message = "Get results successfully.",
            Data = results
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _resultService.GetByIdAsync(id);
        if (result == null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Result not found."
            });
        }

        return Ok(new ApiResponse<Result>
        {
            Success = true,
            Message = "Get result successfully.",
            Data = result
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Result result)
    {
        try
        {
            var created = await _resultService.CreateAsync(result);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, new ApiResponse<Result>
            {
                Success = true,
                Message = "Create result successfully.",
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
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Result result)
    {
        try
        {
            var updated = await _resultService.UpdateAsync(id, result);
            if (!updated)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Result not found."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Update result successfully."
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
        catch (InvalidOperationException ex)
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
        var deleted = await _resultService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Result not found."
            });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Delete result successfully."
        });
    }
}
