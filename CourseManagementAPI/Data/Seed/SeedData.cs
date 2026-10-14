using CourseManagementAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementAPI.Data.Seed;

public static class SeedData
{
    // hash fixo (HasData precisa de valores determinísticos) da senha "Senha@123"
    private const string PasswordHash = "AQAAAAIAAYagAAAAEAABAgMEBQYHCAkKCwwNDg8JMGEZ72VCQlkfQvfwV+ZmYH91F/ZREdZhyfoeVx6lNw==";
    private static readonly DateTime Now = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    private static Guid Id(int n) => new($"00000000-0000-0000-0000-{n:D12}");

    public static void Seed(ModelBuilder mb)
    {
        var (admin, instructor, student) = (Id(1), Id(2), Id(3));
        mb.Entity<AccessRole>().HasData(
            new AccessRole { Id = admin, Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = "role-admin" },
            new AccessRole { Id = instructor, Name = "Instructor", NormalizedName = "INSTRUCTOR", ConcurrencyStamp = "role-instructor" },
            new AccessRole { Id = student, Name = "Student", NormalizedName = "STUDENT", ConcurrencyStamp = "role-student" });

        // users: 100-101 admins, 110-114 estudantes
        var users = new List<ApplicationUser>();
        void AddUser(int n, string name, string email) => users.Add(new ApplicationUser
        {
            Id = Id(n), UserName = email, NormalizedUserName = email.ToUpperInvariant(),
            Email = email, NormalizedEmail = email.ToUpperInvariant(), EmailConfirmed = true,
            PasswordHash = PasswordHash, SecurityStamp = $"stamp-{n}", ConcurrencyStamp = $"concurrency-{n}"
        });

        AddUser(100, "Admin Instrutor", "admin.instrutor@course.com");
        AddUser(101, "Admin Estudante", "admin.estudante@course.com");
        string[] studentNames = ["Ana Souza", "Bruno Lima", "Carla Dias", "Diego Alves", "Elisa Rocha"];
        for (var i = 0; i < 5; i++)
            AddUser(110 + i, studentNames[i], $"{studentNames[i].Split(' ')[0].ToLowerInvariant()}@course.com");
        mb.Entity<ApplicationUser>().HasData(users);

        // cada admin tem o cargo Admin + o cargo da sua classe
        mb.Entity<IdentityUserRole<Guid>>().HasData(
            new IdentityUserRole<Guid> { UserId = Id(100), RoleId = admin },
            new IdentityUserRole<Guid> { UserId = Id(100), RoleId = instructor },
            new IdentityUserRole<Guid> { UserId = Id(101), RoleId = admin },
            new IdentityUserRole<Guid> { UserId = Id(101), RoleId = student });
        mb.Entity<IdentityUserRole<Guid>>().HasData(
            Enumerable.Range(110, 5).Select(n => new IdentityUserRole<Guid> { UserId = Id(n), RoleId = student }));

        mb.Entity<Instructor>().HasData(new Instructor { UserId = Id(100), FullName = "Admin Instrutor", Subject = "Programação" });

        mb.Entity<Student>().HasData(
            new Student { UserId = Id(101), FullName = "Admin Estudante", RegisterDate = Now, IsActive = true });
        mb.Entity<Student>().HasData(
            Enumerable.Range(0, 5).Select(i => new Student { UserId = Id(110 + i), FullName = studentNames[i], RegisterDate = Now, IsActive = true }));

        mb.Entity<Course>().HasData(
            new Course { CourseId = Id(200), Title = "C# Básico", Description = "Fundamentos da linguagem C#", Category = "Programação", WorkLoad = 40, CreatedAt = Now, IsActive = true },
            new Course { CourseId = Id(201), Title = "ASP.NET Core", Description = "Construindo APIs REST", Category = "Programação", WorkLoad = 60, CreatedAt = Now, IsActive = true },
            new Course { CourseId = Id(202), Title = "PostgreSQL", Description = "Modelagem e consultas SQL", Category = "Banco de Dados", WorkLoad = 30, CreatedAt = Now, IsActive = true });
    }
}
