namespace CourseManagementAPI.Data.Dtos;

public class CreateCourseDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public int WorkLoad {  get; set; }
}
