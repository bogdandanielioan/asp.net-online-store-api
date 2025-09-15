using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using OnlineSchool.Auth.Controllers;

namespace Teste.Integration;

public class AuthControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions JsonOpts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    public AuthControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_Admin_ReturnsToken()
    {
        var payload = new LoginRequest { Username = "admin", Password = "admin" };
        var resp = await _client.PostAsJsonAsync("/api/v1/Auth/login", payload);
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
        var body = await resp.Content.ReadFromJsonAsync<LoginResponse>(JsonOpts);
        Assert.NotNull(body);
        Assert.False(string.IsNullOrWhiteSpace(body!.Token));
        Assert.Equal("Admin", body.Role);
    }

    [Fact]
    public async Task Login_Invalid_ReturnsUnauthorized()
    {
        var payload = new LoginRequest { Username = "admin", Password = "wrong" };
        var resp = await _client.PostAsJsonAsync("/api/v1/Auth/login", payload);
        Assert.Equal(HttpStatusCode.Unauthorized, resp.StatusCode);
    }
}
