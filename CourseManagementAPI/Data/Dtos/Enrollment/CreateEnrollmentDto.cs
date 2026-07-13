namespace CourseManagementAPI.Data.Dtos;

public class CreateEnrollmentDto
{ 
    public Guid StudentId { get; set; }
    public Guid CourseId { get; set; }
}
