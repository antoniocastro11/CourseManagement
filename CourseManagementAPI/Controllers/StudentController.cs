using AutoMapper;
using CourseManagementAPI.Data;
using CourseManagementAPI.Data.Dtos.Student;
using CourseManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
}