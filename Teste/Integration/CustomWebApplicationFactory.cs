using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnlineSchool.Auth.Models;
using OnlineSchool.Auth.Services;
using OnlineSchool.Data;
using OnlineSchool.Students.Models;

namespace Teste.Integration;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _dbName = $"OnlineSchool_TestDb_{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // Replace AppDbContext with InMemory provider
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase(_dbName)
            );

            // Build the service provider and seed database
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
            Seed(db);
        });
    }

    public HttpClient CreateAuthenticatedClient(params string[] permissions)
    {
        var client = CreateClient();
        using var scope = Services.CreateScope();
        var generator = scope.ServiceProvider.GetRequiredService<IJwtTokenGenerator>();
        var effectivePermissions = (permissions is { Length: > 0 }
            ? permissions
            : new[] { "read", "write", "read:student", "write:student", "read:course", "write:course" });

        var claims = effectivePermissions.Select(p => new Claim("permission", p));
        var user = new AuthenticatedUser("tester@example.com", "Test User", SystemRoles.Admin);
        var token = generator.GenerateToken(user, claims).Token;
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    private static void Seed(AppDbContext db)
    {
        // Seed a student
        db.Students.Add(new Student
        {
            Id = "stu1",
            Name = "Alice",
            Email = "alice@example.com",
            Age = 20,
            UpdateDate = DateTime.UtcNow,
            Role = "User",
            StudentBooks = new List<OnlineSchool.Books.Models.Book>(),
            MyCourses = new List<OnlineSchool.Enrolments.Models.Enrolment>()
        });

        // Seed a course
        db.Courses.Add(new OnlineSchool.Courses.Models.Course
        {
            Id = "c1",
            Name = "Math",
            Department = "Science"
        });

        // Seed a student card for Alice
        db.Studentscard.Add(new OnlineSchool.StudentCards.Models.StudentCard
        {
            Id = "sc1",
            IdStudent = "stu1",
            Namecard = "AliceCard"
        });

        // Seed a book for Alice
        db.Books.Add(new OnlineSchool.Books.Models.Book
        {
            Id = "b1",
            IdStudent = "stu1",
            Name = "Algebra Basics",
            Created = DateTime.UtcNow
        });

        // Seed an enrolment linking Alice to Math
        db.Enrolments.Add(new OnlineSchool.Enrolments.Models.Enrolment
        {
            Id = "e1",
            IdStudent = "stu1",
            IdCourse = "c1",
            Created = DateTime.UtcNow
        });

        // Seed a teacher required by Courses creation
        db.Teachers.Add(new OnlineSchool.Teachers.Models.Teacher
        {
            Id = "t1",
            Name = "Prof One",
            Email = "prof1@example.com",
            UpdateDate = DateTime.UtcNow,
            Role = "Admin"
        });

        db.SaveChanges();
    }
}
