using AutoMapper;
using CourseManagementAPI.Data;
using CourseManagementAPI.Data.Dtos.Student;
using CourseManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseManagementAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentController(CourseDbContext context, IMapper mapper, StudentService service) : ControllerBase
{
    // TODO: atualizar estudantes, softdelete

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IEnumerable<ReadStudentDto>> GetAllStudents([FromQuery] int take = 10, [FromQuery] int skip = 0)
    {
        return await service.GetAllStudentsAsync(skip, take);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudentById(Guid id)
    {
        var student = await service.GetStudentByIdAsync(id);

        if (student is null) return NotFound();

        return Ok(student);
    }

    [Authorize(Roles = "Student")]
    [HttpGet("me")]
    public async Task<ActionResult<ReadStudentDto>> GetMe()
    {
        Guid studentId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        ReadStudentDto student = await service.GetStudentByIdAsync(studentId);
        return Ok(student);
    }

    [Authorize(Roles = "Student")]
    [HttpPatch("me")]
    public async Task<IActionResult> UpdateProfileAsync(UpdateStudentDto dto)
    {
        await service.UpdateProfileAsync(User, dto);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateStudentAsync(Guid id, UpdateStudentDto dto)
    {
        await service.UpdateStudentAsync(id, dto);
        return NoContent();
    }
}