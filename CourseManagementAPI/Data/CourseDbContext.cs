using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CourseManagementAPI.Models;
using CourseManagementAPI.Data.Seed;
namespace CourseManagementAPI.Data;

public class CourseDbContext(DbContextOptions<CourseDbContext> opts) : IdentityDbContext<ApplicationUser, AccessRole, Guid>(opts)
{   
    public DbSet<Student>Students => Set<Student>();
    public DbSet<Instructor> Instructors => Set<Instructor>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Enrollment> Enrollments => Set <Enrollment>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Student>().HasOne(s=>s.User)
            .WithOne()
            .HasForeignKey<Student>(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Instructor>().HasOne(i => i.User)
            .WithOne()
            .HasForeignKey<Instructor>(i => i.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Enrollment>().HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Enrollment>().HasOne(e => e.Course)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        SeedData.Seed(builder);
    }
}