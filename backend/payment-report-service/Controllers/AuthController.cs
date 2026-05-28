using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.DTOs;
using PaymentService.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace PaymentService.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginRequest request)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null)
            return Unauthorized(ApiResponse<LoginResponse>.Error("Email hoặc mật khẩu không đúng."));

        if (user.Password != request.Password)
            return Unauthorized(ApiResponse<LoginResponse>.Error("Email hoặc mật khẩu không đúng."));

        if (!string.IsNullOrEmpty(request.Role) && user.Role != request.Role)
            return Unauthorized(ApiResponse<LoginResponse>.Error("Vai trò không hợp lệ."));

        var token = GenerateJwt(user);
        var response = new LoginResponse(token, user.Role, new UserDto(
            user.Id, user.FullName, user.Email, user.Role, user.Phone, user.Status, user.CreatedAt
        ));

        return Ok(ApiResponse<LoginResponse>.Ok(response, "Login successfully."));
    }

    private string GenerateJwt(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _config["Jwt:Key"] ?? "EduCenterPaymentServiceSecretKey2026VeryLongKey!"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("fullName", user.FullName),
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"] ?? "PaymentService",
            audience: _config["Jwt:Audience"] ?? "EduCenter",
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
