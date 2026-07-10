using CourseManagementAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementAPI.Data;

public class CourseDbContext : DbContext
{
    public CourseDbContext(DbContextOptions<CourseDbContext> opts) : base(opts) {}
    public DbSet<Student>Students => Set<Student>();
    public DbSet<Course> Courses => Set<Course>();
}
