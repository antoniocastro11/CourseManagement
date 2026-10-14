using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace CourseManagementAPI.IntegrationTests;

public abstract class ApiTestBase(ApiFactory factory)
{
    protected const string Password = "Senha@123"; // senha de todos os usuários do seed

    // seed: admin.instrutor = Admin+Instructor, admin.estudante = Admin+Student, ana..elisa = Student
    protected const string Admin = "admin.instrutor@course.com";
    protected static readonly Guid Ana = new("00000000-0000-0000-0000-000000000110");
    protected static readonly Guid Bruno = new("00000000-0000-0000-0000-000000000111");
    protected static readonly Guid Carla = new("00000000-0000-0000-0000-000000000112");
    protected static readonly Guid CSharp = new("00000000-0000-0000-0000-000000000200");
    protected static readonly Guid AspNet = new("00000000-0000-0000-0000-000000000201");
    protected static readonly Guid Postgres = new("00000000-0000-0000-0000-000000000202");

    protected HttpClient Anonymous() => factory.CreateClient();

    protected async Task<HttpClient> LoginAs(string email)
    {
        var client = factory.CreateClient();
        var res = await client.PostAsJsonAsync("/Auth/login", new { email, password = Password });
        res.EnsureSuccessStatusCode();
        var token = (await res.Content.ReadAsStringAsync()).Trim('"');
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }
}
