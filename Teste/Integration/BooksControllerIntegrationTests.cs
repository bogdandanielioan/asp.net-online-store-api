using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using OnlineSchool.Books.Models;
using Xunit;

namespace Teste.Integration;

public class BooksControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions JsonOpts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    public BooksControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateAuthenticatedClient();
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithSeededBook()
    {
        var resp = await _client.GetAsync("/api/v1/ControllerBook/all");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

        var items = await resp.Content.ReadFromJsonAsync<List<Book>>(JsonOpts);
        Assert.NotNull(items);
        Assert.Contains(items!, b => b.Name == "Algebra Basics");
    }

    [Fact]
    public async Task GetById_ReturnsOk_ForSeeded()
    {
        var resp = await _client.GetAsync("/api/v1/ControllerBook/findById?id=b1");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

        var model = await resp.Content.ReadFromJsonAsync<Book>(JsonOpts);
        Assert.NotNull(model);
        Assert.Equal("Algebra Basics", model!.Name);
    }

    [Fact]
    public async Task GetByName_ReturnsOk_ForSeeded()
    {
        var resp = await _client.GetAsync("/api/v1/ControllerBook/findByName?name=Algebra%20Basics");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

        var model = await resp.Content.ReadFromJsonAsync<Book>(JsonOpts);
        Assert.NotNull(model);
        Assert.Equal("b1", model!.Id);
    }

    [Fact]
    public async Task GetByName_ReturnsNotFound_WhenMissing()
    {
        var resp = await _client.GetAsync("/api/v1/ControllerBook/findByName?name=GhostBook");
        Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
    }
}
