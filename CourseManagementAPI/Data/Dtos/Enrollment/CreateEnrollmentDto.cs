namespace CourseManagementAPI.Data.Dtos.Enrollment;

public class CreateEnrollmentDto
{ 
    public Guid? StudentId { get; set; } // pensar que ele pode se inscrever e o admin pode inscrever ele
    public Guid CourseId { get; set; }
}
