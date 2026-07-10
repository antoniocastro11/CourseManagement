using System.ComponentModel.DataAnnotations;

namespace CourseManagementAPI.Model;

public class Course
{
    [Key]
    [Required]
    public Guid CourseID { get; set; }

    [MaxLength(100, ErrorMessage = "O título não deve ultrapassar 100 caracteres") ]
    [Required]
    public string Title { get; set; }

    [MaxLength(200, ErrorMessage = "A descrição não deve ultrapassar 200 caracteres")]
    public string Description { get; set; }

    [Required]
    [MaxLength(50, ErrorMessage = "A categoria não deve ultrapassar 50 caracteres")]
    public string Category { get; set; }

    [Required]
    public int WorkLoad { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public Boolean isActive { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; set; }
    public virtual ICollection<Student> Students { get; set; }


}
