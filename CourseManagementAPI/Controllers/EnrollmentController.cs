using CourseManagementAPI.Data.Dtos.Enrollment;
using CourseManagementAPI.Models;
using CourseManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class EnrollmentController(EnrollmentService service) : ControllerBase
{
    // TODO: cadastrar estudantes em um curso(com atutenticacao), getenrollmentbystudentId, cancelar matrícula(opicional), paginação e filtro por status

    [HttpPost]
    [Authorize (Roles="Student, Admin, Instructor")]
    public async Task<IActionResult> CreateEnrollment([FromBody] CreateEnrollmentDto createDto)
    {

        var canCreate = await service.CreateEnrollment(createDto, User);
        
        if (!canCreate) return BadRequest();

        return Created();
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Student")]
    public async Task<IActionResult> GetEnrollmentById(Guid id)
    {
        var search = await service.GetEnrollmentAuxAsync(id);

        if (search is null) return NotFound();
        
        return Ok(search);
        
    }
}