using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using OnlineSchool.Students.Dto;
using Xunit;

namespace Teste.Integration;

public class StudentsControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions JsonOpts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    public StudentsControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateAuthenticatedClient();
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithSeededStudent()
    {
        var resp = await _client.GetAsync("/api/v1/ControllerStudent/all");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

        var items = await resp.Content.ReadFromJsonAsync<List<DtoStudentView>>(JsonOpts);
        Assert.NotNull(items);
        Assert.Contains(items!, s => s.Name == "Alice" && s.Email == "alice@example.com");
    }

    [Fact]
    public async Task GetById_ReturnsOk_ForSeeded()
    {
        var resp = await _client.GetAsync("/api/v1/ControllerStudent/findById?id=stu1");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

        var dto = await resp.Content.ReadFromJsonAsync<DtoStudentView>(JsonOpts);
        Assert.NotNull(dto);
        Assert.Equal("Alice", dto!.Name);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_ForMissing()
    {
        var resp = await _client.GetAsync("/api/v1/ControllerStudent/findById?id=ghost");
        Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
    }

    [Fact]
    public async Task GetByName_ReturnsNotFound_WhenMissing()
    {
        var resp = await _client.GetAsync("/api/v1/ControllerStudent/findByName?name=Ghost");
        Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
    }
}
