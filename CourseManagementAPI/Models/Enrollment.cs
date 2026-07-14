using System.ComponentModel.DataAnnotations;

namespace CourseManagementAPI.Models;

public class Enrollment
{
    [Key]
    [Required]
    public Guid EnrollmentId { get; set; }

    [Required]
    public Guid StudentId { get; set; }
    public virtual Student Student { get; set; }

    [Required]
    public Guid CourseId { get; set; }
    public virtual Course Course { get; set; }
    
    [Required]
    public bool IsActive { get; set; }
    public DateTime EnrollmentDate { get; set; }
}
