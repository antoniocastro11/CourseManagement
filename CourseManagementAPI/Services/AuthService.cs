using AutoMapper;
using CourseManagementAPI.Data.Dtos.Auth;
using CourseManagementAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CourseManagementAPI.Services;
public class AuthService (UserManager<ApplicationUser> userManager, IMapper mapper, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
{
    private readonly string jwtKey = configuration["Jwt:Key"];

    public async Task<string> GenerateToken(ApplicationUser user)
    {
        var roles = await signInManager.UserManager.GetRolesAsync(user);

        var claims = new List<Claim> // como tenho que adicionar os cargos, claim vira uma lista e não um array (array tem tamanho fixo)
        {
            new("username", user.UserName),
            new("id", user.Id.ToString()),
            new("email", user.Email)
        };

        foreach (var role in roles)
        {
            claims.Add(new(ClaimTypes.Role, role)); // adiciona os cargos
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(claims: claims, signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task Register(RegisterDto registerDto)
    {
        ApplicationUser user = mapper.Map<ApplicationUser>(registerDto);
        
        IdentityResult result = await userManager.CreateAsync(user, registerDto.Password);
        
        if (!result.Succeeded) throw new ApplicationException("Falha ao cadastrar usuário");

        await userManager.AddToRoleAsync(user, registerDto.Role.ToString());
    }

    public async Task<string> Login(LoginDto loginDto)
    {
        var result = await signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, false);
        
        if (!result.Succeeded) throw new ApplicationException("Credenciais inválidas");
        
        var user = await signInManager.UserManager.FindByEmailAsync(loginDto.Email);
        
        if (user is null) throw new ApplicationException("Usuário não encontrado");
        
        return await GenerateToken(user);
    }

}
