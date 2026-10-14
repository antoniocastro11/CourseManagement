using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace CourseManagementAPI.IntegrationTests;

[Collection("api")]
public class CourseEndpointsTests(ApiFactory f) : ApiTestBase(f)
{
    private static object NewCourse(string title) => new { title, description = "d", category = "Teste", workLoad = 10 };

    [Fact]
    public async Task GetCourses_Anonimo_Retorna200ComSeed()
    {
        var res = await Anonymous().GetAsync("/Course");
        res.EnsureSuccessStatusCode();
        Assert.True(JsonDocument.Parse(await res.Content.ReadAsStringAsync()).RootElement.GetArrayLength() >= 3);
    }

    [Fact]
    public async Task GetCourses_FiltraPorCategoria()
    {
        var res = await Anonymous().GetAsync("/Course?category=Banco%20de%20Dados");
        res.EnsureSuccessStatusCode();
        Assert.Equal(1, JsonDocument.Parse(await res.Content.ReadAsStringAsync()).RootElement.GetArrayLength());
    }

    [Fact]
    public async Task GetCourseById_Existente_Retorna200()
        => (await Anonymous().GetAsync($"/Course/{CSharp}")).EnsureSuccessStatusCode();

    [Fact]
    public async Task GetCourseById_Inexistente_Retorna404()
        => Assert.Equal(HttpStatusCode.NotFound, (await Anonymous().GetAsync($"/Course/{Guid.NewGuid()}")).StatusCode);

    [Fact]
    public async Task CreateCourse_SemToken_Retorna401()
        => Assert.Equal(HttpStatusCode.Unauthorized, (await Anonymous().PostAsJsonAsync("/Course", NewCourse("x"))).StatusCode);

    [Fact]
    public async Task CreateCourse_ComoStudent_Retorna403()
        => Assert.Equal(HttpStatusCode.Forbidden, (await (await LoginAs("ana@course.com")).PostAsJsonAsync("/Course", NewCourse("x"))).StatusCode);

    [Fact]
    public async Task CreateCourse_ComoAdmin_Retorna201()
    {
        var client = await LoginAs(Admin);
        var res = await client.PostAsJsonAsync("/Course", NewCourse($"Curso {Guid.NewGuid():N}"));
        Assert.Equal(HttpStatusCode.Created, res.StatusCode);
    }

    [Fact]
    public async Task UpdateCourse_Retorna204_EPersiste()
    {
        var client = await LoginAs(Admin);
        var res = await client.PatchAsJsonAsync($"/Course/{Postgres}", new { description = "Atualizada" });
        Assert.Equal(HttpStatusCode.NoContent, res.StatusCode);

        var body = await client.GetStringAsync($"/Course/{Postgres}");
        Assert.Contains("Atualizada", body);
    }
}
