using CourseManagementAPI.Data.Dtos.Student;
using CourseManagementAPI.Services;

namespace CourseManagementAPI.UnitTests;

public class StudentServiceTests : IDisposable
{
    private readonly TestDb _t = new();
    private readonly StudentService _sut;
    public StudentServiceTests() => _sut = new StudentService(_t.Db, _t.Mapper);
    public void Dispose() => _t.Dispose();

    private static readonly Guid Ana = new("00000000-0000-0000-0000-000000000110");

    [Fact]
    public async Task GetAllStudentsAsync_RespeitaTake()
        => Assert.Equal(3, (await _sut.GetAllStudentsAsync(take: 3)).Count);

    [Fact]
    public async Task GetStudentByIdAsync_Existente()
        => Assert.Equal("Ana Souza", (await _sut.GetStudentByIdAsync(Ana)).FullName);

    [Fact]
    public async Task GetStudentByIdAsync_Inexistente_RetornaNull()
        => Assert.Null(await _sut.GetStudentByIdAsync(Guid.NewGuid()));

    [Fact]
    public async Task UpdateStudentAsync_AtualizaNome()
    {
        await _sut.UpdateStudentAsync(Ana, new UpdateStudentDto { FullName = "Ana Maria" });
        Assert.Equal("Ana Maria", (await _t.Db.Students.FindAsync(Ana))!.FullName);
    }
}
