using System.Net;
using System.Net.Http.Json;

namespace CourseManagementAPI.IntegrationTests;

[Collection("api")]
public class EnrollmentEndpointsTests(ApiFactory f) : ApiTestBase(f)
{
    [Fact]
    public async Task Create_SemToken_Retorna401()
        => Assert.Equal(HttpStatusCode.Unauthorized, (await Anonymous().PostAsJsonAsync("/Enrollment", new { courseId = CSharp })).StatusCode);

    [Fact]
    public async Task Create_ComoAdmin_Retorna201_EDuplicadaRetorna400()
    {
        var client = await LoginAs(Admin);
        var body = new { studentId = Bruno, courseId = CSharp };

        Assert.Equal(HttpStatusCode.Created, (await client.PostAsJsonAsync("/Enrollment", body)).StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, (await client.PostAsJsonAsync("/Enrollment", body)).StatusCode);
    }

    [Fact]
    public async Task Create_ComoStudent_MatriculaASiMesmo()
    {
        var res = await (await LoginAs("carla@course.com")).PostAsJsonAsync("/Enrollment", new { courseId = AspNet });
        Assert.Equal(HttpStatusCode.Created, res.StatusCode);
    }

    [Fact]
    public async Task GetMe_ComoStudent_Retorna200()
        => (await (await LoginAs("diego@course.com")).GetAsync("/Enrollment/me")).EnsureSuccessStatusCode();

    [Fact]
    public async Task GetByStudent_ComoStudent_Retorna403()
        => Assert.Equal(HttpStatusCode.Forbidden, (await (await LoginAs("ana@course.com")).GetAsync($"/Enrollment/student/{Ana}")).StatusCode);

    [Fact]
    public async Task GetByStudent_ComoAdmin_Retorna200()
        => (await (await LoginAs(Admin)).GetAsync($"/Enrollment/student/{Ana}")).EnsureSuccessStatusCode();

    [Fact]
    public async Task GetById_Inexistente_Retorna404()
        => Assert.Equal(HttpStatusCode.NotFound, (await (await LoginAs(Admin)).GetAsync($"/Enrollment/{Guid.NewGuid()}")).StatusCode);
}
