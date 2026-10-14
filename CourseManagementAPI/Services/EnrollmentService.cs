using AutoMapper;
using CourseManagementAPI.Data;
using CourseManagementAPI.Data.Dtos.Enrollment;
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

    public async Task<bool> CanCreateEnrollment(CreateEnrollmentDto dto)
    {
        if (await context.Students.AnyAsync(s => s.IsActive && s.UserId == dto.StudentId && s.Enrollments.Any(ent => ent.CourseId == dto.CourseId)) )
        {
            return false;
        }

        if (await context.Enrollments.AnyAsync(e => e.StudentId == dto.StudentId && e.CourseId == dto.CourseId)) return false;

        return true;
    }

    public async Task<bool> CreateEnrollment(CreateEnrollmentDto dto, ClaimsPrincipal user)
    {
        var canCreate = await CanCreateEnrollment(dto);
        if(!canCreate) return false;

        Guid studentId;

        if (user.IsInRole("Student"))
        {
            studentId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        } else if(user.IsInRole("Admin") || user.IsInRole("Instructor"))
        {
            if (dto.StudentId is null) throw new ApplicationException("O Id de estudante é obrigatório");

            studentId = dto.StudentId.Value;

        } else
        {
            throw new UnauthorizedAccessException();
        }

        Enrollment enrollment = new Enrollment
        {
            StudentId = studentId,
            CourseId = dto.CourseId
        };

        context.Enrollments.Add(enrollment);

        await context.SaveChangesAsync();
        return true;

    }
}