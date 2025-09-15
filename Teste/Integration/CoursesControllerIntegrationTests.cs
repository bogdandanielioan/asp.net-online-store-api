using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using OnlineSchool.Courses.Dto;
using OnlineSchool.Courses.Models;
using Xunit;

namespace Teste.Integration;

public class CoursesControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions JsonOpts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    public CoursesControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithSeededCourse()
    {
        var resp = await _client.GetAsync("/api/v1/courses/all");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

        var items = await resp.Content.ReadFromJsonAsync<List<DtoCourseView>>(JsonOpts);
        Assert.NotNull(items);
        Assert.Contains(items!, c => c.Name == "Math" && c.Department == "Science");
    }

    [Fact]
    public async Task GetById_ReturnsOk_ForSeeded()
    {
        var resp = await _client.GetAsync("/api/v1/courses/findById?id=c1");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

        var dto = await resp.Content.ReadFromJsonAsync<DtoCourseView>(JsonOpts);
        Assert.NotNull(dto);
        Assert.Equal("Math", dto!.Name);
    }

    [Fact]
    public async Task GetByName_ReturnsOk_ForSeeded()
    {
        var resp = await _client.GetAsync("/api/v1/courses/findByName?name=Math");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

        var dto = await resp.Content.ReadFromJsonAsync<DtoCourseView>(JsonOpts);
        Assert.NotNull(dto);
        Assert.Equal("Science", dto!.Department);
    }

    [Fact]
    public async Task CreateCourse_ReturnsOk_WhenValid()
    {
        var req = new CreateRequestCourse { Name = "Physics", Department = "Science", TeacherId = "t1" };
        var resp = await _client.PostAsJsonAsync("/api/v1/courses/createCourse", req);
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

        var model = await resp.Content.ReadFromJsonAsync<Course>(JsonOpts);
        Assert.NotNull(model);
        Assert.Equal("Physics", model!.Name);
        Assert.False(string.IsNullOrWhiteSpace(model.Id));
    }

    [Fact]
    public async Task CreateCourse_ReturnsBadRequest_OnInvalidName()
    {
        var req = new CreateRequestCourse { Name = string.Empty, Department = "X" };
        var resp = await _client.PostAsJsonAsync("/api/v1/courses/createCourse", req);
        Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
    }

    [Fact]
    public async Task UpdateCourse_ReturnsOk_WhenValid()
    {
        // create first
        var create = new CreateRequestCourse { Name = "Chem", Department = "Sci", TeacherId = "t1" };
        var cResp = await _client.PostAsJsonAsync("/api/v1/courses/createCourse", create);
        var created = await cResp.Content.ReadFromJsonAsync<Course>(JsonOpts);
        Assert.NotNull(created);

        var update = new UpdateRequestCourse { Name = "Chemistry" };
        var uResp = await _client.PutAsJsonAsync($"/api/v1/courses/updateCourse?id={created!.Id}", update);
        Assert.Equal(HttpStatusCode.OK, uResp.StatusCode);

        var updated = await uResp.Content.ReadFromJsonAsync<Course>(JsonOpts);
        Assert.NotNull(updated);
        Assert.Equal("Chemistry", updated!.Name);
    }

    [Fact]
    public async Task UpdateCourse_ReturnsBadRequest_OnInvalidName()
    {
        // create first
        var create = new CreateRequestCourse { Name = "Bio", Department = "Sci", TeacherId = "t1" };
        var cResp = await _client.PostAsJsonAsync("/api/v1/courses/createCourse", create);
        var created = await cResp.Content.ReadFromJsonAsync<Course>(JsonOpts);
        Assert.NotNull(created);

        var update = new UpdateRequestCourse { Name = string.Empty };
        var uResp = await _client.PutAsJsonAsync($"/api/v1/courses/updateCourse?id={created!.Id}", update);
        Assert.Equal(HttpStatusCode.BadRequest, uResp.StatusCode);
    }

    [Fact]
    public async Task UpdateCourse_ReturnsNotFound_WhenMissing()
    {
        var update = new UpdateRequestCourse { Name = "X" };
        var uResp = await _client.PutAsJsonAsync($"/api/v1/courses/updateCourse?id=missing", update);
        Assert.Equal(HttpStatusCode.NotFound, uResp.StatusCode);
    }

    [Fact]
    public async Task DeleteCourse_ReturnsOk_ThenNotFoundOnSecondDelete()
    {
        var create = new CreateRequestCourse { Name = "Hist", Department = "Art", TeacherId = "t1" };
        var cResp = await _client.PostAsJsonAsync("/api/v1/courses/createCourse", create);
        var created = await cResp.Content.ReadFromJsonAsync<Course>(JsonOpts);
        Assert.NotNull(created);

        var dResp = await _client.DeleteAsync($"/api/v1/courses/deleteCourse?id={created!.Id}");
        Assert.Equal(HttpStatusCode.OK, dResp.StatusCode);

        var dResp2 = await _client.DeleteAsync($"/api/v1/courses/deleteCourse?id={created!.Id}");
        Assert.Equal(HttpStatusCode.NotFound, dResp2.StatusCode);
    }

    [Fact]
    public async Task CreateCourse_ReturnsBadRequest_WhenTeacherIdMissing()
    {
        var req = new CreateRequestCourse { Name = "Geo", Department = "Sci" /* TeacherId missing */ };
        var resp = await _client.PostAsJsonAsync("/api/v1/courses/createCourse", req);
        Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
    }

    [Fact]
    public async Task CreateCourse_ReturnsNotFound_WhenTeacherDoesNotExist()
    {
        var req = new CreateRequestCourse { Name = "Geo2", Department = "Sci", TeacherId = "no-such" };
        var resp = await _client.PostAsJsonAsync("/api/v1/courses/createCourse", req);
        Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
    }
}
