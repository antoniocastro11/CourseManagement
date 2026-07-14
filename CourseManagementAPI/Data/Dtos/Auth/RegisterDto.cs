using CourseManagementAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace CourseManagementAPI.Data.Dtos.Auth;

public class RegisterDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public UserRole Role { get; set; }
}