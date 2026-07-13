namespace CourseManagementAPI.Data.Dtos.Student;
public class ReadStudentDto
{
    public Guid UserId { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
}
