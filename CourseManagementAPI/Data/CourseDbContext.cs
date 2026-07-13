using Microsoft.EntityFrameworkCore;
using CourseManagementAPI.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CourseManagementAPI.Models;
namespace CourseManagementAPI.Data;

public class CourseDbContext(DbContextOptions<CourseDbContext> opts) : IdentityDbContext<ApplicationUser, AccessRole, Guid>(opts)
{    public DbSet<Student>Students => Set<Student>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Enrollment> Enrollments => Set <Enrollment>();
}