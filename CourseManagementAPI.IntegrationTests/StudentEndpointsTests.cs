using System.Net;

namespace CourseManagementAPI.IntegrationTests;

[Collection("api")]
public class StudentEndpointsTests(ApiFactory f) : ApiTestBase(f)
{
    [Fact]
    public async Task GetAll_SemToken_Retorna401()
        => Assert.Equal(HttpStatusCode.Unauthorized, (await Anonymous().GetAsync("/Student")).StatusCode);

    [Fact]
    public async Task GetAll_ComoStudent_Retorna403()
        => Assert.Equal(HttpStatusCode.Forbidden, (await (await LoginAs("ana@course.com")).GetAsync("/Student")).StatusCode);

    [Fact]
    public async Task GetAll_ComoAdmin_Retorna200()
        => (await (await LoginAs(Admin)).GetAsync("/Student")).EnsureSuccessStatusCode();

    [Fact]
    public async Task GetById_Inexistente_Retorna404()
        => Assert.Equal(HttpStatusCode.NotFound, (await (await LoginAs(Admin)).GetAsync($"/Student/{Guid.NewGuid()}")).StatusCode);

    [Fact]
    public async Task GetMe_ComoStudent_RetornaProprioPerfil()
    {
        var res = await (await LoginAs("ana@course.com")).GetAsync("/Student/me");
        res.EnsureSuccessStatusCode();
        Assert.Contains("Ana Souza", await res.Content.ReadAsStringAsync());
    }
}
