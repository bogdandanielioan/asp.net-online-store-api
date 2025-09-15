using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using OnlineSchool.Enrolments.Models;
using Xunit;

namespace Teste.Integration;

public class EnrolmentsControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions JsonOpts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    public EnrolmentsControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithSeededEnrolment()
    {
        var resp = await _client.GetAsync("/api/v1/ControllerEnrolment/all");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

        var items = await resp.Content.ReadFromJsonAsync<List<Enrolment>>(JsonOpts);
        Assert.NotNull(items);
        Assert.Contains(items!, e => e.Id == "e1" && e.IdStudent == "stu1" && e.IdCourse == "c1");
    }

    [Fact]
    public async Task GetById_ReturnsOk_ForSeeded()
    {
        var resp = await _client.GetAsync("/api/v1/ControllerEnrolment/findById?id=e1");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

        var model = await resp.Content.ReadFromJsonAsync<Enrolment>(JsonOpts);
        Assert.NotNull(model);
        Assert.Equal("stu1", model!.IdStudent);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenMissing()
    {
        var resp = await _client.GetAsync("/api/v1/ControllerEnrolment/findById?id=missing");
        Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
    }
}
