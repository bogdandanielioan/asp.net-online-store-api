using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using OnlineSchool.Books.Repository;
using OnlineSchool.Books.Repository.interfaces;
using OnlineSchool.Books.Services;
using OnlineSchool.Books.Services.interfaces;
using OnlineSchool.Courses.Repository;
using OnlineSchool.Courses.Repository.interfaces;
using OnlineSchool.Courses.Services;
using OnlineSchool.Courses.Services.interfaces;
using OnlineSchool.Data;
using OnlineSchool.Enrolments.Repository;
using OnlineSchool.Enrolments.Repository.interfaces;
using OnlineSchool.Enrolments.Services;
using OnlineSchool.Enrolments.Services.interfaces;
using OnlineSchool.StudentCards.Repository;
using OnlineSchool.StudentCards.Repository.interfaces;
using OnlineSchool.StudentCards.Services;
using OnlineSchool.StudentCards.Services.interfaces;
using OnlineSchool.Students.Repository;
using OnlineSchool.Students.Repository.interfaces;
using OnlineSchool.Students.Services;
using OnlineSchool.Students.Services.interfaces;
using OnlineSchool.StudentsCard.Repository;
using OnlineSchool.Teachers.Repository;
using OnlineSchool.Teachers.Repository.interfaces;
using OnlineSchool.Teachers.Services;
using OnlineSchool.Teachers.Services.interfaces;
using MySqlConnector;
using OnlineSchool.Auth;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "OnlineSchool API",
        Version = "v1",
        Description = "API documentation for OnlineSchool. Use the Authorize button to enter a Bearer token."
    });

    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Enter 'Bearer {your JWT token}'. Example: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

var jwtSection = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSection["Key"] ?? "");

if (builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = TestAuthHandler.SchemeName;
        options.DefaultChallengeScheme = TestAuthHandler.SchemeName;
    })
    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.SchemeName, _ => { });
}
else
{
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });
}

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Read", policy => policy.RequireClaim("permission", "read"));
    options.AddPolicy("Write", policy => policy.RequireClaim("permission", "write"));
    options.AddPolicy("read:student", policy => policy.RequireAssertion(ctx =>
        ctx.User.HasClaim("permission", "read:student") || ctx.User.HasClaim("permission", "read")));
    options.AddPolicy("write:student", policy => policy.RequireAssertion(ctx =>
        ctx.User.HasClaim("permission", "write:student") || ctx.User.HasClaim("permission", "write")));

    options.AddPolicy("read:course", policy => policy.RequireAssertion(ctx =>
        ctx.User.HasClaim("permission", "read:course") || ctx.User.HasClaim("permission", "read")));
    options.AddPolicy("write:course", policy => policy.RequireAssertion(ctx =>
        ctx.User.HasClaim("permission", "write:course") || ctx.User.HasClaim("permission", "write")));
});

builder.Services.AddScoped<IRepositoryStudentCard, RepositoryStudentCard>();
builder.Services.AddScoped<IQueryServiceStudentCard, QueryServiceStudentCards>();

builder.Services.AddScoped<IRepositoryStudent, RepositoryStudent>();
builder.Services.AddScoped<IQueryServiceStudent, QueryServiceStudents>();
builder.Services.AddScoped<ICommandServiceStudent, CommandServiceStudents>();

builder.Services.AddScoped<IRepositoryBook, RepositoryBook>();
builder.Services.AddScoped<IQueryServiceBook, QueryServiceBook>();

builder.Services.AddScoped<IRepositoryCourse, RepositoryCourse>();
builder.Services.AddScoped<IQueryServiceCourse, QueryServiceCourse>();
builder.Services.AddScoped<ICommandServiceCourse, CommandServiceCourse>();

builder.Services.AddScoped<IRepositoryTeacher, RepositoryTeacher>();
builder.Services.AddScoped<IQueryServiceTeacher, QueryServiceTeachers>();
builder.Services.AddScoped<ICommandServiceTeacher, CommandServiceTeachers>();

builder.Services.AddScoped<IRepositoryEnrolment, RepositoryEnrolment>();
builder.Services.AddScoped<IQueryServiceEnrolment, QueryServiceEnrolment>();

builder.Services.AddDbContext<AppDbContext>(op => op.UseMySql(builder.Configuration.GetConnectionString("Default")!,
    new MySqlServerVersion(new Version(8, 0, 21))));

builder.Services.AddFluentMigratorCore()
    .ConfigureRunner(rb => rb.AddMySql5().WithGlobalConnectionString(builder.Configuration.GetConnectionString("Default"))
    .ScanIn(typeof(Program).Assembly).For.Migrations())
    .AddLogging(lb => lb.AddFluentMigratorConsole());

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

if (!app.Environment.IsEnvironment("Testing"))
{
    using (var scope = app.Services.CreateScope())
    {
        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var connStr = config.GetConnectionString("Default");
        EnsureDatabaseExists(connStr);

        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }
}

app.Run();

static void EnsureDatabaseExists(string? connectionString)
{
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new InvalidOperationException("Connection string 'Default' is not configured.");
    }

    var builder = new MySqlConnectionStringBuilder(connectionString);
    var database = builder.Database;
    if (string.IsNullOrWhiteSpace(database))
    {
        return;
    }

    builder.Database = string.Empty;

    using var connection = new MySqlConnection(builder.ConnectionString);
    connection.Open();

    using var cmd = new MySqlCommand($"CREATE DATABASE IF NOT EXISTS `{database}`;", connection);
    cmd.ExecuteNonQuery();
}

public partial class Program { }