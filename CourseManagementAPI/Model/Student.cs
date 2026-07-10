using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CourseManagementAPI.Model;
public class Student : IdentityUser<Guid>
    // reforçar: preciso entender como cria as roles
{
    [Required]
    public string FullName { get; set; }

    [Required]
    public DateTime RegisterDate { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; set; }
    public virtual ICollection<Course> Courses { get; set; }

    public Student() : base() { }
}