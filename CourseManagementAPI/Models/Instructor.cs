using System.ComponentModel.DataAnnotations;
using CourseManagementAPI.Model;

namespace CourseManagementAPI.Model;

public class Instructor
{
    [Key]
    public Guid UserId { get; set;}
    public ApplicationUser User {get; set;}

    [Required]
    [MaxLength(70, ErrorMessage = "O nome não deve ultrapassar 70 caracteres")]
    public string FullName { get; set;}

    

}