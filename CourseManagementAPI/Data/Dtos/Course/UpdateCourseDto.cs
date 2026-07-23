namespace CourseManagementAPI.Data.Dtos.Course;

public class UpdateCourseDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public int? WorkLoad { get; set; }
    public bool? IsActive { get; set; }
}
