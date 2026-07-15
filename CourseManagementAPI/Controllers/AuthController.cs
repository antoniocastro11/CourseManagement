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
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        await authService.Register(registerDto);
        return NoContent();
    } // precisa pensar como salvar os estudantes na tabela correta

    [HttpPost("login")]
    public async Task<IActionResult> LoginRequest(LoginDto loginDto)
    {
        var token = await authService.Login(loginDto);
        return Ok(token);
    }
}
