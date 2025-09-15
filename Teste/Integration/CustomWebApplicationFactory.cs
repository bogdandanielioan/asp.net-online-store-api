using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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

        db.SaveChanges();
    }
}