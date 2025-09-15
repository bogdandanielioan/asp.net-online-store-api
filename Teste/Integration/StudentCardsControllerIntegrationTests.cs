using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using OnlineSchool.StudentCards.Models;
using Xunit;

namespace Teste.Integration;

public class StudentCardsControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions JsonOpts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    public StudentCardsControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateAuthenticatedClient();
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithSeededCard()
    {
        var resp = await _client.GetAsync("/api/v1/ControllerStudentCards/all");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

        var items = await resp.Content.ReadFromJsonAsync<List<StudentCard>>(JsonOpts);
        Assert.NotNull(items);
        Assert.Contains(items!, c => c.Namecard == "AliceCard");
    }

    [Fact]
    public async Task GetById_ReturnsOk_ForSeeded()
    {
        var resp = await _client.GetAsync("/api/v1/ControllerStudentCards/findById?id=sc1");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

        var model = await resp.Content.ReadFromJsonAsync<StudentCard>(JsonOpts);
        Assert.NotNull(model);
        Assert.Equal("AliceCard", model!.Namecard);
    }

    [Fact]
    public async Task GetByName_ReturnsOk_ForSeeded()
    {
        var resp = await _client.GetAsync("/api/v1/ControllerStudentCards/findByName?name=AliceCard");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

        var model = await resp.Content.ReadFromJsonAsync<StudentCard>(JsonOpts);
        Assert.NotNull(model);
        Assert.Equal("sc1", model!.Id);
    }

    [Fact]
    public async Task GetByName_ReturnsNotFound_WhenMissing()
    {
        var resp = await _client.GetAsync("/api/v1/ControllerStudentCards/findByName?name=GhostCard");
        Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
    }
}
