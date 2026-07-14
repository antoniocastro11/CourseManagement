using System.ComponentModel.DataAnnotations;

namespace CourseManagementAPI.Models;
public class Student
{
    [Key]
    public Guid UserId {get; set;}
    public ApplicationUser User {get; set;}

    [Required]
    [MaxLength(70, ErrorMessage = "O nome não deve ultrapassar 70 caracteres")]
    public string FullName { get; set; }

    [Required]
    public DateTime RegisterDate { get; set; }
    public virtual ICollection<Enrollment> Enrollments { get; set; }
}