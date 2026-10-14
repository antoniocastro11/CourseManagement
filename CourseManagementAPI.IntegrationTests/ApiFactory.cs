using CourseManagementAPI.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace CourseManagementAPI.IntegrationTests;

public class ApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _pg = new PostgreSqlBuilder().WithImage("postgres:17").Build();

    public async Task InitializeAsync()
    {
        await _pg.StartAsync();
        // Program.cs lê Jwt:Key e a connection string antes do host de teste, então vão por env var
        Environment.SetEnvironmentVariable("Jwt__Key", "chave-de-teste-com-pelo-menos-32-bytes!!");
        Environment.SetEnvironmentVariable("ConnectionStrings__DatabaseConnection", _pg.GetConnectionString());

        // migra antes do host subir: o startup já consulta as roles
        var opts = new DbContextOptionsBuilder<CourseDbContext>().UseNpgsql(_pg.GetConnectionString()).Options;
        await using var db = new CourseDbContext(opts);
        await db.Database.MigrateAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder) => builder.UseEnvironment("Development");

    async Task IAsyncLifetime.DisposeAsync() => await _pg.DisposeAsync();
}

[CollectionDefinition("api")]
public class ApiCollection : ICollectionFixture<ApiFactory> { }
