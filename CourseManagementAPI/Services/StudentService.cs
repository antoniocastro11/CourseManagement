using AutoMapper;
using CourseManagementAPI.Data;
using CourseManagementAPI.Data.Dtos.Enrollment;
using CourseManagementAPI.Data.Dtos.Student;
using CourseManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementAPI.Services;

public class StudentService(CourseDbContext context, IMapper mapper)
{

    public async Task<ReadStudentDto> GetStudentByIdAsync(Guid id)
    {
        Student? student = await context.Students.FindAsync(id);
        if (student is null) return null;

        ReadStudentDto response = mapper.Map<ReadStudentDto>(student);

        return response;
    }

    public async Task<List<ReadStudentDto>> GetAllStudentsAsync(int take = 10, int skip = 0)
    {
        return mapper.Map<List<ReadStudentDto>>(context.Students.Skip(skip).Take(take).ToListAsync());
    }

    public async Task<List<ReadEnrollmentDto>> GetStudentEnrollmentsAsync(Guid id)
    {
        List<Enrollment> enrollments = await context.Enrollments.Where(e => e.StudentId == id).ToListAsync();
        return mapper.Map<List<ReadEnrollmentDto>>(enrollments);
    }
}
