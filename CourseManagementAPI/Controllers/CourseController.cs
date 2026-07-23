using AutoMapper;
using CourseManagementAPI.Data;
using CourseManagementAPI.Data.Dtos;
using CourseManagementAPI.Data.Dtos.Course;
using CourseManagementAPI.Models;
using CourseManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CourseController(CourseService service) : ControllerBase
{
    // TODO: atualizar (adm/instrutor), remover(adm)
    // pensar aqui como faz, pq tem q ser com patch (problema de pacote)

    [Authorize(Roles ="Admin, Instructor")]
    [HttpPost]
    public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDto createDto)
    {
        await service.CreateCourse(createDto);

        return Created();
    }

    [HttpGet]
    public async Task<IEnumerable<ReadCourseDto>> GetAllCourses([FromQuery] int take = 10, [FromQuery] int skip = 0, [FromQuery] string? category = null)
    {
        return await service.GetAllCoursesAsync(take, skip, category);

    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCourseById(Guid id)
    {
        var search = await service.GetCourseAuxAsync(id);

        if (search is null) return NotFound();
        
        return Ok(search);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateCourse(Guid id, UpdateCourseDto dto)
    {
        await service.UpdateCourse(id, dto);
        return NoContent();
    }
}
