using System.Security.Claims;
using CourseManagementAPI.Data.Dtos.Enrollment;
using CourseManagementAPI.Exceptions;
using CourseManagementAPI.Models;
using CourseManagementAPI.Services;

namespace CourseManagementAPI.UnitTests;

public class EnrollmentServiceTests : IDisposable
{
    private readonly TestDb _t = new();
    private readonly EnrollmentService _sut;
    public EnrollmentServiceTests() => _sut = new EnrollmentService(_t.Mapper, _t.Db);
    public void Dispose() => _t.Dispose();

    private static readonly Guid Ana = new("00000000-0000-0000-0000-000000000110");
    private static readonly Guid Course = new("00000000-0000-0000-0000-000000000200");

    private static ClaimsPrincipal User(string role, Guid? id = null)
    {
        var claims = new List<Claim> { new(ClaimTypes.Role, role) };
        if (id is not null) claims.Add(new(ClaimTypes.NameIdentifier, id.ToString()!));
        return new ClaimsPrincipal(new ClaimsIdentity(claims, "test"));
    }

    [Fact]
    public async Task Student_MatriculaASiMesmo()
    {
        await _sut.CreateEnrollment(new CreateEnrollmentDto { CourseId = Course }, User("Student", Ana));
        Assert.Single(_t.Db.Enrollments, e => e.StudentId == Ana && e.CourseId == Course);
    }

    [Fact]
    public async Task Admin_MatriculaTerceiro()
    {
        var dto = await _sut.CreateEnrollment(new CreateEnrollmentDto { CourseId = Course, StudentId = Ana }, User("Admin"));
        Assert.Equal(Ana, dto.StudentId);
    }

    [Fact]
    public async Task Admin_SemStudentId_Lanca()
        => await Assert.ThrowsAsync<EnrollmentException>(
            () => _sut.CreateEnrollment(new CreateEnrollmentDto { CourseId = Course }, User("Admin")));

    [Fact]
    public async Task SemRoleValida_LancaUnauthorized()
        => await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _sut.CreateEnrollment(new CreateEnrollmentDto { CourseId = Course, StudentId = Ana }, User("Outro")));

    [Fact]
    public async Task Duplicada_Lanca()
    {
        _t.Db.Enrollments.Add(new Enrollment { StudentId = Ana, CourseId = Course, IsActive = true });
        await _t.Db.SaveChangesAsync();

        await Assert.ThrowsAsync<EnrollmentException>(
            () => _sut.CreateEnrollment(new CreateEnrollmentDto { CourseId = Course }, User("Student", Ana)));
    }

    [Fact]
    public async Task GetEnrollmentAuxAsync_Inexistente_RetornaNull()
        => Assert.Null(await _sut.GetEnrollmentAuxAsync(Guid.NewGuid()));
}
