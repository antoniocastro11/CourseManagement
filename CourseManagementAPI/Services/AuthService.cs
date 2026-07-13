using CourseManagementAPI.Model;
using System.Security.Claims;

namespace CourseManagementAPI.Services;
public class AuthService
{
    public string GenerateToken(ApplicationUser user)
    {
        Claim[] claims =
        {
            new("username", user.UserName),
            new("id", user.Id.ToString()),

        };

        return "teste";
    }
}
