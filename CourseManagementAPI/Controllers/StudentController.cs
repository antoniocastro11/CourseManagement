using AutoMapper;
using CourseManagementAPI.Data;
using CourseManagementAPI.Data.Dtos.Student;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentController(CourseDbContext context, IMapper mapper) : ControllerBase
{
    // TODO: atualizar estudantes, softdelete

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IEnumerable<ReadStudentDto> GetAllStudents([FromQuery] int take = 10, [FromQuery] int skip = 0)
    {
        return mapper.Map<List<ReadStudentDto>>(context.Students.Take(take).Skip(skip).ToList());
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    public IActionResult GetStudentById(Guid id)
    {
        var student = context.Students.Find(id);

        if (student is null) return NotFound();
        
        var dto = mapper.Map<ReadStudentDto>(student);
        
        return Ok(dto);
    }
}