using CourseManagementAPI.Data.Dtos;
using CourseManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementAPI.Controllers;

[ApiController]
public class AuthController : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpPost]
    //pensasr qual vai ser esse metodo aqui, pq tem que ver do cadastro de outros tipos de usuário
    public async Task<IActionResult> RegisterStudent(CreateStudentDto createDto)
    {
       // await AuthService.Register(createDto);
        return NoContent();
    }

    //[HttpPost]
    //preciso criar o login dto
    //public async Task <IActionResult> LoginRequest(loginDto){
      //  }
}
