using CourseManagementAPI.Data;
using CourseManagementAPI.Models;
using CourseManagementAPI.Services;
using CourseManagementAPI.Profiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var connstring = builder.Configuration.GetConnectionString("DatabaseConnection");
var jwtKey = builder.Configuration["Jwt:Key"];

// Add services to the container.

builder.Services.AddDbContext<CourseDbContext>(options => options.UseNpgsql(connstring));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
    ValidateAudience = false,
    ValidateIssuer = false,
    ClockSkew = TimeSpan.Zero
});



builder.Services.AddIdentity<ApplicationUser, AccessRole>(opts => {
    opts.User.RequireUniqueEmail = true;
    opts.Password.RequiredLength = 8;
})
        .AddRoles<AccessRole>()
        .AddEntityFrameworkStores<CourseDbContext>()
        .AddDefaultTokenProviders();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<ApplicationUserProfile>();
    cfg.AddProfile<CourseProfile>();
    cfg.AddProfile<StudentProfile>();
    cfg.AddProfile<EnrollmentProfile>();
});

// Dependency Injection
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<EnrollmentService>();

builder.Services.AddExceptionHandler<CourseManagementAPI.Exceptions.Handlers.EnrollmentExceptionHandler>();
builder.Services.AddExceptionHandler<CourseManagementAPI.Exceptions.Handlers.GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

using(var scope = app.Services.CreateScope()){
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AccessRole>>();
    string[] roles = ["Admin", "Instructor", "Student"];
    foreach(var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(
                new AccessRole
                {
                    Name = role
                });
        }
    }
}

// Configure the HTTP request pipeline.
app.UseExceptionHandler();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();



app.Run();
