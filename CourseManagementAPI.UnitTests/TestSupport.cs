using AutoMapper;
using CourseManagementAPI.Data;
using CourseManagementAPI.Profiles;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace CourseManagementAPI.UnitTests;

public sealed class TestDb : IDisposable
{
    private readonly SqliteConnection _conn = new("DataSource=:memory:");
    public CourseDbContext Db { get; }
    public IMapper Mapper { get; } = CreateMapper();

    public TestDb()
    {
        _conn.Open();
        Db = new CourseDbContext(new DbContextOptionsBuilder<CourseDbContext>().UseSqlite(_conn).Options);
        Db.Database.EnsureCreated(); // aplica o HasData do SeedData: 3 cursos, 7 estudantes
    }

    public static MapperConfiguration CreateConfig() => new(c =>
    {
        c.AddProfile<ApplicationUserProfile>();
        c.AddProfile<CourseProfile>();
        c.AddProfile<StudentProfile>();
        c.AddProfile<EnrollmentProfile>();
    }, NullLoggerFactory.Instance);

    public static IMapper CreateMapper() => CreateConfig().CreateMapper();

    public void Dispose() { Db.Dispose(); _conn.Dispose(); }
}
