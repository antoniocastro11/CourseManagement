using AutoMapper;
using CourseManagementAPI.Data;
using CourseManagementAPI.Data.Dtos.Enrollment;
using CourseManagementAPI.Exceptions;
using CourseManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CourseManagementAPI.Services;

public class EnrollmentService(IMapper mapper, CourseDbContext context)
{
    public async Task<ReadEnrollmentDto?> GetEnrollmentAuxAsync(Guid id)
    {
        Enrollment? enrollment = await context.Enrollments.FindAsync(id);
        
        if (enrollment is null) return null;

        ReadEnrollmentDto dto = mapper.Map<ReadEnrollmentDto>(enrollment);
        
        return dto;
    }

    public async Task<ReadEnrollmentDto> CreateEnrollment(CreateEnrollmentDto dto, ClaimsPrincipal user)
    {
        Guid studentId = ResolveStudentId(dto, user);
        await ValidateEnrollmentAsync(studentId, dto.CourseId);

        var enrollment = new Enrollment { StudentId = studentId, CourseId = dto.CourseId };
        context.Enrollments.Add(enrollment);
        await context.SaveChangesAsync();

        return mapper.Map<ReadEnrollmentDto>(enrollment);
    }

    private static Guid ResolveStudentId(CreateEnrollmentDto dto, ClaimsPrincipal user)
    {
        if (user.IsInRole("Student"))
            return Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        if (user.IsInRole("Admin") || user.IsInRole("Instructor"))
            return dto.StudentId ?? throw new EnrollmentException("O Id de estudante é obrigatório");
        throw new UnauthorizedAccessException();
    }

    private async Task ValidateEnrollmentAsync(Guid studentId, Guid courseId)
    {
        var student = await context.Students.FirstOrDefaultAsync(s => s.UserId == studentId);
        if (student is null || !student.IsActive)
            throw new EnrollmentException("Estudante inexistente ou inativo.");

        if (await context.Enrollments.AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId))
            throw new EnrollmentException("Estudante já matriculado neste curso.");
    }
}
