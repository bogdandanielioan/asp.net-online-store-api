using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace Teste.Integration;

public class AuthRegisterIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions JsonOpts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    public AuthRegisterIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    private class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    private class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public string Role { get; set; } = string.Empty;
    }

    private class CreateRequestStudent
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Age { get; set; }
        public string? Password { get; set; }
    }

    private class DtoStudentView
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Age { get; set; }
        public string? Role { get; set; }
    }

    private class TeacherCreateRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Subject { get; set; }
        public string? Password { get; set; }
    }

    private class DtoTeacherView
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Subject { get; set; }
        public DateTime UpdateDate { get; set; }
        public string? Role { get; set; }
    }

    [Fact]
    public async Task Student_Register_Is_Unprotected_And_Assigns_User_Role()
    {
        var create = new CreateRequestStudent { Name = "Reg User", Email = "reg.user@example.com", Age = 19, Password = "secret123" };
        using var req = new HttpRequestMessage(HttpMethod.Post, "/api/v1/students/createStudent")
        {
            Content = JsonContent.Create(create)
        };
        // Simulate no authentication
        req.Headers.Add("X-Test-Auth", "none");

        var resp = await _client.SendAsync(req);
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

        var dto = await resp.Content.ReadFromJsonAsync<DtoStudentView>(JsonOpts);
        Assert.NotNull(dto);
        Assert.False(string.IsNullOrWhiteSpace(dto!.Id));
        Assert.Equal("Reg User", dto.Name);
        Assert.Equal("reg.user@example.com", dto.Email);
        Assert.Equal("User", dto.Role);
    }

    [Fact]
    public async Task Teacher_Register_Is_Unprotected_And_Assigns_Admin_Role()
    {
        var create = new TeacherCreateRequest { Name = "Reg Teach", Email = "reg.teach@example.com", Subject = "Math", Password = "secret" };

        using var req = new HttpRequestMessage(HttpMethod.Post, "/api/v1/teachers/create")
        {
            Content = JsonContent.Create(create)
        };
        req.Headers.Add("X-Test-Auth", "none");

        var resp = await _client.SendAsync(req);
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

        var dto = await resp.Content.ReadFromJsonAsync<DtoTeacherView>(JsonOpts);
        Assert.NotNull(dto);
        Assert.False(string.IsNullOrWhiteSpace(dto!.Id));
        Assert.Equal("Reg Teach", dto.Name);
        Assert.Equal("reg.teach@example.com", dto.Email);
        Assert.Equal("Admin", dto.Role);
    }

    [Fact]
    public async Task Auth_Login_Admin_Succeeds_Returns_Token_And_Role()
    {
        var reqObj = new LoginRequest { Username = "admin", Password = "admin" };
        var resp = await _client.PostAsJsonAsync("/api/v1/Auth/login", reqObj);
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

        var login = await resp.Content.ReadFromJsonAsync<LoginResponse>(JsonOpts);
        Assert.NotNull(login);
        Assert.False(string.IsNullOrWhiteSpace(login!.Token));
        Assert.True(login.ExpiresAt > DateTime.UtcNow);
        Assert.Equal("Admin", login.Role);
    }

    [Fact]
    public async Task Auth_Login_User_Succeeds_With_User_Role()
    {
        var reqObj = new LoginRequest { Username = "user", Password = "user" };
        var resp = await _client.PostAsJsonAsync("/api/v1/Auth/login", reqObj);
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

        var login = await resp.Content.ReadFromJsonAsync<LoginResponse>(JsonOpts);
        Assert.NotNull(login);
        Assert.False(string.IsNullOrWhiteSpace(login!.Token));
        Assert.True(login.ExpiresAt > DateTime.UtcNow);
        Assert.Equal("User", login.Role);
    }

    [Fact]
    public async Task Auth_Login_Fails_With_Wrong_Password()
    {
        var reqObj = new LoginRequest { Username = "admin", Password = "wrong" };
        var resp = await _client.PostAsJsonAsync("/api/v1/Auth/login", reqObj);
        Assert.Equal(HttpStatusCode.Unauthorized, resp.StatusCode);
    }
}
