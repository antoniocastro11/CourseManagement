using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentController : ControllerBase
{
    // TODO: cadastrar estudantes (via admin), listar estudantes(admin), atualizar estudantes,  getbyid, softdelete
    [Authorize]
    [HttpGet]
    [Route("{id}")]
    public IActionResult GetStudentById(Guid id)
    {
        return Ok("Em Implementação");
    }
}