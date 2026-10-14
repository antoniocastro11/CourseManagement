using CourseManagementAPI.Data.Dtos;
using CourseManagementAPI.Data.Dtos.Course;
using CourseManagementAPI.Services;

namespace CourseManagementAPI.UnitTests;

public class CourseServiceTests : IDisposable
{
    private readonly TestDb _t = new();
    private readonly CourseService _sut;
    public CourseServiceTests() => _sut = new CourseService(_t.Db, _t.Mapper);
    public void Dispose() => _t.Dispose();

    private static readonly Guid CSharp = new("00000000-0000-0000-0000-000000000200");

    [Fact]
    public async Task GetCourseAuxAsync_Existente_RetornaDto()
    {
        var dto = await _sut.GetCourseAuxAsync(CSharp);
        Assert.Equal("C# Básico", dto.Title);
    }

    [Fact]
    public async Task GetCourseAuxAsync_Inexistente_RetornaNull()
        => Assert.Null(await _sut.GetCourseAuxAsync(Guid.NewGuid()));

    [Theory]
    [InlineData(null, 3)]
    [InlineData("Programação", 2)]
    [InlineData("Banco de Dados", 1)]
    [InlineData("Inexistente", 0)]
    public async Task GetAllCoursesAsync_FiltraPorCategoria(string? categoria, int esperado)
        => Assert.Equal(esperado, (await _sut.GetAllCoursesAsync(10, 0, categoria)).Count);

    [Fact]
    public async Task GetAllCoursesAsync_RespeitaTakeESkip()
    {
        Assert.Single(await _sut.GetAllCoursesAsync(take: 1, skip: 0));
        Assert.Equal(2, (await _sut.GetAllCoursesAsync(take: 10, skip: 1)).Count);
    }

    [Fact]
    public async Task CreateCourse_Persiste()
    {
        await _sut.CreateCourse(new CreateCourseDto { Title = "Novo", Description = "d", Category = "X", WorkLoad = 10 });
        Assert.Contains(_t.Db.Courses, c => c.Title == "Novo");
    }

    [Fact]
    public async Task UpdateCourse_AtualizaApenasCamposInformados()
    {
        await _sut.UpdateCourse(CSharp, new UpdateCourseDto { Title = "Renomeado" });
        var c = await _t.Db.Courses.FindAsync(CSharp);
        Assert.Equal("Renomeado", c!.Title);
        Assert.Equal(40, c.WorkLoad);
    }

    [Fact]
    public async Task UpdateCourse_Inexistente_NaoLancaNullReference()
    {
        var ex = await Record.ExceptionAsync(() => _sut.UpdateCourse(Guid.NewGuid(), new UpdateCourseDto { Title = "x" }));
        Assert.IsNotType<NullReferenceException>(ex);
        Assert.IsNotType<AutoMapper.AutoMapperMappingException>(ex);
    }
}
