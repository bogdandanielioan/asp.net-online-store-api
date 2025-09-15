using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using OnlineSchool.Students.Dto;
using OnlineSchool.Students.Models;
using OnlineSchool.Teachers.Models;
using OnlineSchool.Courses.Models;

namespace Teste.Integration;

public class RolesIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions JsonOpts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    public RolesIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Seeded_Teacher_Has_Admin_Role()
    {
        var resp = await _client.GetAsync("/api/v1/ControllerTeacher/findById?id=t1");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
        var model = await resp.Content.ReadFromJsonAsync<Teacher>(JsonOpts);
        Assert.NotNull(model);
        Assert.Equal("Admin", model!.Role);
    }

    [Fact]
    public async Task Creating_Student_Assigns_User_Role()
    {
        var create = new CreateRequestStudent { Name = "Role Stu", Email = "role.stu@example.com", Age = 18 };
        var cResp = await _client.PostAsJsonAsync("/api/v1/ControllerStudent/createStudent", create);
        Assert.Equal(HttpStatusCode.OK, cResp.StatusCode);
        var student = await cResp.Content.ReadFromJsonAsync<Student>(JsonOpts);
        Assert.NotNull(student);
        Assert.Equal("User", student!.Role);
    }

    [Fact]
    public async Task Creating_Teacher_Assigns_Admin_Role()
    {
        var create = new Teacher { Id = "tmp", Name = "Role Teach", Email = "rt@example.com", UpdateDate = DateTime.UtcNow, Courses = new List<Course>() };
        var cResp = await _client.PostAsJsonAsync("/api/v1/ControllerTeacher/create", create);
        Assert.Equal(HttpStatusCode.OK, cResp.StatusCode);
        var teacher = await cResp.Content.ReadFromJsonAsync<Teacher>(JsonOpts);
        Assert.NotNull(teacher);
        Assert.Equal("Admin", teacher!.Role);
    }
}
