using CourseManagementAPI.Data.Dtos.Enrollment;
using CourseManagementAPI.Models;
using CourseManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseManagementAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class EnrollmentController(EnrollmentService service, StudentService studentService) : ControllerBase
{

    [HttpPost]
    [Authorize (Roles="Student, Admin, Instructor")]
    public async Task<IActionResult> CreateEnrollment([FromBody] CreateEnrollmentDto createDto)
    {
        var enrollment = await service.CreateEnrollment(createDto, User);

        return Created(string.Empty, enrollment);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Student")]
    public async Task<IActionResult> GetEnrollmentById(Guid id)
    {
        var search = await service.GetEnrollmentAuxAsync(id);

        if (search is null) return NotFound();
        
        return Ok(search);
        
    }

    [HttpGet("student/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetEnrollmentsByStudentId(Guid id)
    {
        var search = await studentService.GetStudentEnrollmentsAsync(id);

        return Ok(search);
    }

    [HttpGet("me")]
    [Authorize(Roles="Student")]
    public async Task<IActionResult> GetStudentEnrollments()
    {
        Guid studentId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var search = await studentService.GetStudentEnrollmentsAsync(studentId);
        
        return Ok(search);
    }
}