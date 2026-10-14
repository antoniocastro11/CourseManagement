using AutoMapper;
using CourseManagementAPI.Data;
using CourseManagementAPI.Data.Dtos.Enrollment;
using CourseManagementAPI.Data.Dtos.Student;
using CourseManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
        var students = await context.Students.Skip(skip).Take(take).ToListAsync();

        return mapper.Map<List<ReadStudentDto>>(students);
    }

    public async Task<List<ReadEnrollmentDto>> GetStudentEnrollmentsAsync(Guid id)
    {
        List<Enrollment> enrollments = await context.Enrollments.Where(e => e.StudentId == id).ToListAsync();
        return mapper.Map<List<ReadEnrollmentDto>>(enrollments);
    }

    public async Task UpdateProfileAsync(ClaimsPrincipal user, UpdateStudentDto dto)
    {
        var studentId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var student = await context.Students.FindAsync(studentId) ?? throw new KeyNotFoundException();
        mapper.Map(dto, student);
        await context.SaveChangesAsync();

    }

    public async Task UpdateStudentAsync(Guid id, UpdateStudentDto dto)
    {
        var student = await context.Students.FindAsync(id) ?? throw new KeyNotFoundException();
        mapper.Map(dto, student);
        await context.SaveChangesAsync();
    }
}
