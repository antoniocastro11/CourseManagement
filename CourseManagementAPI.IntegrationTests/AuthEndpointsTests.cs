using System.Net;
using System.Net.Http.Json;

namespace CourseManagementAPI.IntegrationTests;

[Collection("api")]
public class AuthEndpointsTests(ApiFactory f) : ApiTestBase(f)
{
    [Fact]
    public async Task Login_CredenciaisValidas_RetornaToken()
    {
        var res = await Anonymous().PostAsJsonAsync("/Auth/login", new { email = Admin, password = Password });
        res.EnsureSuccessStatusCode();
        Assert.Equal(3, (await res.Content.ReadAsStringAsync()).Trim('"').Split('.').Length); // JWT: header.payload.signature
    }

    [Fact]
    public async Task Login_SenhaErrada_NaoRetornaSucesso()
    {
        var res = await Anonymous().PostAsJsonAsync("/Auth/login", new { email = Admin, password = "errada" });
        Assert.False(res.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Register_SemToken_Retorna401()
    {
        var res = await Anonymous().PostAsJsonAsync("/Auth/register", new { name = "x", email = "x@x.com", password = "Senha@123", role = 2 });
        Assert.Equal(HttpStatusCode.Unauthorized, res.StatusCode);
    }

    [Fact]
    public async Task Register_ComoStudent_Retorna403()
    {
        var res = await (await LoginAs("ana@course.com")).PostAsJsonAsync("/Auth/register", new { name = "x", email = "x@x.com", password = "Senha@123", role = 2 });
        Assert.Equal(HttpStatusCode.Forbidden, res.StatusCode);
    }

    [Fact]
    public async Task Register_ComoAdmin_Retorna204_ENovoUsuarioConsegueLogar()
    {
        var email = $"novo{Guid.NewGuid():N}@course.com";
        var res = await (await LoginAs(Admin)).PostAsJsonAsync("/Auth/register", new { name = "Novo", email, password = "Senha@123", role = 2 });
        Assert.Equal(HttpStatusCode.NoContent, res.StatusCode);

        var login = await Anonymous().PostAsJsonAsync("/Auth/login", new { email, password = "Senha@123" });
        login.EnsureSuccessStatusCode();
    }
}
