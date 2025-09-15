using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using OnlineSchool.Teachers.Models;
using OnlineSchool.Courses.Models;
using Xunit;

namespace Teste.Integration;

public class TeachersControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions JsonOpts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    public TeachersControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateAuthenticatedClient();
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithSeededTeacher()
    {
        var resp = await _client.GetAsync("/api/v1/teachers/all");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

        var items = await resp.Content.ReadFromJsonAsync<List<Teacher>>(JsonOpts);
        Assert.NotNull(items);
        Assert.Contains(items!, t => t.Email == "prof1@example.com");
    }

    [Fact]
    public async Task GetById_ReturnsOk_ForSeeded()
    {
        var resp = await _client.GetAsync("/api/v1/teachers/findById?id=t1");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

        var model = await resp.Content.ReadFromJsonAsync<Teacher>(JsonOpts);
        Assert.NotNull(model);
        Assert.Equal("Prof One", model!.Name);
    }

    [Fact]
    public async Task GetByEmail_ReturnsOk_ForSeeded()
    {
        var resp = await _client.GetAsync("/api/v1/teachers/findByEmail?email=prof1@example.com");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

        var model = await resp.Content.ReadFromJsonAsync<Teacher>(JsonOpts);
        Assert.NotNull(model);
        Assert.Equal("t1", model!.Id);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_ForMissing()
    {
        var resp = await _client.GetAsync("/api/v1/teachers/findById?id=missing");
        Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
    }

    [Fact]
    public async Task Create_Update_Delete_Teacher_Flow()
    {
        // Create
        var newT = new Teacher { Id = "tmp", Name = "Prof Two", Email = "prof2@example.com", Subject = "Math", UpdateDate = DateTime.UtcNow, Courses = new List<Course>() };
        var cResp = await _client.PostAsJsonAsync("/api/v1/teachers/create", newT);
        Assert.Equal(HttpStatusCode.OK, cResp.StatusCode);
        var created = await cResp.Content.ReadFromJsonAsync<Teacher>(JsonOpts);
        Assert.NotNull(created);
        Assert.False(string.IsNullOrWhiteSpace(created!.Id));

        // Update
        var upd = new Teacher { Id = created.Id!, Name = "Prof II", Email = "prof2@example.com", Subject = "Algebra", UpdateDate = DateTime.UtcNow, Courses = new List<Course>() };
        var uResp = await _client.PutAsJsonAsync($"/api/v1/teachers/update?id={created.Id}", upd);
        Assert.Equal(HttpStatusCode.OK, uResp.StatusCode);
        var updated = await uResp.Content.ReadFromJsonAsync<Teacher>(JsonOpts);
        Assert.NotNull(updated);
        Assert.Equal("Prof II", updated!.Name);
        Assert.Equal("Algebra", updated!.Subject);

        // Delete
        var dResp = await _client.DeleteAsync($"/api/v1/teachers/delete?id={created.Id}");
        Assert.Equal(HttpStatusCode.OK, dResp.StatusCode);

        // Delete again should be 404
        var dResp2 = await _client.DeleteAsync($"/api/v1/teachers/delete?id={created.Id}");
        Assert.Equal(HttpStatusCode.NotFound, dResp2.StatusCode);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenInvalid()
    {
        var bad = new Teacher { Name = string.Empty, Email = "" };
        var resp = await _client.PostAsJsonAsync("/api/v1/teachers/create", bad);
        Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
    }
}
