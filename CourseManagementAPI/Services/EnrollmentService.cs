using AutoMapper;
using CourseManagementAPI.Data;
using CourseManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementAPI.Services;

public class EnrollmentService (IMapper mapper, CourseDbContext context)
{
    public async Task<Enrollment> GetEnrollmentAuxAsync(Guid id)
    {
        Enrollment? enrollment = await context.Enrollments.FindAsync(id);
        return enrollment;
    }

    public async Task<bool> IsCourseActive(Guid id)
    {
        var result = await context.Courses.Where(c => c.IsActive == true).FirstOrDefaultAsync(c => c.CourseId == id);
        
        if (result is null)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> IsStudentActive(Guid id)
    {
        return await context.Students.AnyAsync(s => s.IsActive && s.UserId == id);
    }

    public async Task<bool> IsEnrollmentActive(Guid id)
    {
        return await context.Enrollments.AnyAsync(e => e.EnrollmentId == id && e.IsActive);
    }
}