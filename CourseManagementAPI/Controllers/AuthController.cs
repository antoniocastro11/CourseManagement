using CourseManagementAPI.Data.Dtos.Auth;
using CourseManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementAPI.Controllers;

[ApiController]
public class AuthController(AuthService authService) : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterStudent(RegisterDto registerDto)
    {
        await authService.Register(registerDto);
        return NoContent();
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginRequest(LoginDto loginDto)
    {
        var token = await authService.Login(loginDto);
        return Ok(token);
    }
}
