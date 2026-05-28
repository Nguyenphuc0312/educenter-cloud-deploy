using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.DTOs;
using PaymentService.Models;

namespace PaymentService.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _db;

    public UsersController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<UserDto>>>> GetAll(
        [FromQuery] string? role,
        [FromQuery] string? status)
    {
        var query = _db.Users.AsQueryable();
        if (!string.IsNullOrEmpty(role)) query = query.Where(u => u.Role == role);
        if (!string.IsNullOrEmpty(status)) query = query.Where(u => u.Status == status);

        var users = await query
            .OrderBy(u => u.Role)
            .ThenBy(u => u.FullName)
            .Select(u => new UserDto(u.Id, u.FullName, u.Email, u.Role, u.Phone, u.Status, u.CreatedAt))
            .ToListAsync();

        return Ok(ApiResponse<List<UserDto>>.Ok(users));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetById(int id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null)
            return NotFound(ApiResponse<UserDto>.NotFound("User not found."));

        return Ok(ApiResponse<UserDto>.Ok(new UserDto(
            user.Id, user.FullName, user.Email, user.Role, user.Phone, user.Status, user.CreatedAt
        )));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<UserDto>>> Create([FromBody] CreateUserRequest req)
    {
        if (await _db.Users.AnyAsync(u => u.Email == req.Email))
            return Conflict(ApiResponse<UserDto>.Conflict("Email đã tồn tại."));

        var user = new User
        {
            FullName = req.FullName,
            Email = req.Email,
            Password = req.Password,
            Role = req.Role,
            Phone = req.Phone,
            Status = "Active",
            CreatedAt = DateTime.UtcNow
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = user.Id },
            ApiResponse<UserDto>.Created(new UserDto(
                user.Id, user.FullName, user.Email, user.Role, user.Phone, user.Status, user.CreatedAt
            ), "User created successfully."));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<UserDto>>> Update(int id, [FromBody] UpdateUserRequest req)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null)
            return NotFound(ApiResponse<UserDto>.NotFound("User not found."));

        if (!string.IsNullOrEmpty(req.FullName)) user.FullName = req.FullName;
        if (!string.IsNullOrEmpty(req.Email)) user.Email = req.Email;
        if (!string.IsNullOrEmpty(req.Password)) user.Password = req.Password;
        if (!string.IsNullOrEmpty(req.Role)) user.Role = req.Role;
        if (req.Phone != null) user.Phone = req.Phone;
        if (!string.IsNullOrEmpty(req.Status)) user.Status = req.Status;
        user.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Ok(ApiResponse<UserDto>.Ok(new UserDto(
            user.Id, user.FullName, user.Email, user.Role, user.Phone, user.Status, user.CreatedAt
        ), "User updated successfully."));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null)
            return NotFound(ApiResponse<object>.NotFound("User not found."));

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();

        return Ok(ApiResponse.Ok("User deleted successfully."));
    }
}
