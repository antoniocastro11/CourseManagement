using AutoMapper;
using CourseManagementAPI.Data;
using CourseManagementAPI.Data.Dtos.Enrollment;
using CourseManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

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
        return true; // preciso pensar sobre a regra de negócio: o estudante cria a própria matrícula ou ela deve ser criada por terceiros?
    }

    public async Task<bool> CreateEnrollment(CreateEnrollmentDto dto)
    {
        var canCreate = await CanCreateEnrollment(dto);
        if(canCreate)
        {
            Enrollment enrollment = mapper.Map<Enrollment>(dto);

            context.Enrollments.Add(enrollment);
            
            await context.SaveChangesAsync();
            return true;
        }
        return false;
    }
}