using AutoMapper;
using CourseManagementAPI.Data;
using CourseManagementAPI.Data.Dtos.Enrollment;
using CourseManagementAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class EnrollmentController(CourseDbContext context, IMapper mapper) : ControllerBase
{
    // TODO: cadastrar estudantes em um curso(com atutenticacao), getenrollmentbystudentId, cancelar matrícula(opicional), paginação e filtro por status

    [HttpPost]
    [Authorize]
    public IActionResult CreateEnrollment([FromBody] CreateEnrollmentDto createDto)
    {
        Enrollment enrollment = mapper.Map<Enrollment>(createDto);
        context.Enrollments.Add(enrollment);
        context.SaveChanges();
        return Ok(); //corrigir esse tipo de retorno
    }
}