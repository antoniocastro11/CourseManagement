using AutoMapper;
using CourseManagementAPI.Data;
using CourseManagementAPI.Data.Dtos;
using CourseManagementAPI.Data.Dtos.Course;
using CourseManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CourseController(CourseDbContext context, IMapper mapper) : ControllerBase
{
    // TODO: atualizar (adm/instrutor), remover(adm)
    // pensar aqui como faz, pq tem q ser com patch (problema de pacote)

    [HttpPost]
    public IActionResult CreateCourse([FromBody] CreateCourseDto createDto)
    {
        Course course = mapper.Map<Course>(createDto);
        course.CourseId = Guid.NewGuid();
        context.Courses.Add(course);
        context.SaveChanges();
        return Ok(); // corrigir esse tipo de retorno
    }

    [HttpGet]
    public IEnumerable<ReadCourseDto> GetAllCourses([FromQuery] int take = 10, [FromQuery] int skip = 0, [FromQuery] string? category = null)
    {
        if (category is null)
        {
            return mapper.Map<List<ReadCourseDto>>(context.Courses.Take(take).Skip(skip).ToList());
        }

        return mapper.Map<List<ReadCourseDto>>(context.Courses.Where(c => c.Category == category).Skip(skip).Take(take).ToList());
    }

    [HttpGet("{id}")]
    public IActionResult GetCourseById(Guid id)
    {
        var course = context.Courses.Find(id);
        var dto = mapper.Map<ReadCourseDto>(course);
        return Ok(dto);
    }
}
