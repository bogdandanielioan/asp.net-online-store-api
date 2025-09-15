using System.Net;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using OnlineSchool.Auth.Models;
using OnlineSchool.Auth.Services;
using OnlineSchool.Courses.Dto;
using OnlineSchool.Courses.Models;

namespace Teste.Integration;

public class AuthAuthorizationIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;
    private static readonly JsonSerializerOptions JsonOpts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    public AuthAuthorizationIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    private string CreateToken(string role, params string[] permissions)
    {
        using var scope = _factory.Services.CreateScope();
        var generator = scope.ServiceProvider.GetRequiredService<IJwtTokenGenerator>();
        var user = new AuthenticatedUser($"{role.ToLowerInvariant()}@example.com", $"{role} User", role);
        var claims = permissions.Select(p => new Claim("permission", p));
        return generator.GenerateToken(user, claims).Token;
    }

    private static void Authorize(HttpRequestMessage request, string token)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    [Fact]
    public async Task Unauthenticated_Request_Returns401()
    {
        Assert.Null(_client.DefaultRequestHeaders.Authorization);
        using var req = new HttpRequestMessage(HttpMethod.Get, "/api/v1/ControllerCourse/all");
        var resp = await _client.SendAsync(req);
        Assert.Equal(HttpStatusCode.Unauthorized, resp.StatusCode);
    }

    [Fact]
    public async Task ReadOnly_User_CanRead_But_ForbiddenToWrite()
    {
        var token = CreateToken("ReadOnly", "read");
        // GET should be OK with read permission
        using (var getReq = new HttpRequestMessage(HttpMethod.Get, "/api/v1/ControllerCourse/all"))
        {
            Authorize(getReq, token);
            var getResp = await _client.SendAsync(getReq);
            Assert.Equal(HttpStatusCode.OK, getResp.StatusCode);
        }

        // POST should be Forbidden without write permission
        var create = new CreateRequestCourse { Name = "AuthzTest1", Department = "Dept", TeacherId = "t1" };
        using (var postReq = new HttpRequestMessage(HttpMethod.Post, "/api/v1/ControllerCourse/createCourse"))
        {
            Authorize(postReq, token);
            postReq.Content = JsonContent.Create(create);
            var postResp = await _client.SendAsync(postReq);
            Assert.Equal(HttpStatusCode.Forbidden, postResp.StatusCode);
        }
    }

    [Fact]
    public async Task WriteOnly_User_CanWrite_But_ForbiddenToRead()
    {
        var token = CreateToken("WriteOnly", "write");
        // GET should be Forbidden without read permission
        using (var getReq = new HttpRequestMessage(HttpMethod.Get, "/api/v1/ControllerCourse/all"))
        {
            Authorize(getReq, token);
            var getResp = await _client.SendAsync(getReq);
            Assert.Equal(HttpStatusCode.Forbidden, getResp.StatusCode);
        }

        // POST should be OK with write permission
        var create = new CreateRequestCourse { Name = "AuthzTest2", Department = "Dept", TeacherId = "t1" };
        using (var postReq = new HttpRequestMessage(HttpMethod.Post, "/api/v1/ControllerCourse/createCourse"))
        {
            Authorize(postReq, token);
            postReq.Content = JsonContent.Create(create);
            var postResp = await _client.SendAsync(postReq);
            Assert.Equal(HttpStatusCode.OK, postResp.StatusCode);
            var model = await postResp.Content.ReadFromJsonAsync<Course>(JsonOpts);
            Assert.NotNull(model);
            Assert.Equal("AuthzTest2", model!.Name);
        }
    }

    [Fact]
    public async Task Default_Admin_CanRead_And_Write()
    {
        var adminToken = CreateToken(SystemRoles.Admin, "read", "write");
        // No override headers => default admin with read+write
        var create = new CreateRequestCourse { Name = "AuthzDefaultAdmin", Department = "Dept", TeacherId = "t1" };
        using var postReq = new HttpRequestMessage(HttpMethod.Post, "/api/v1/ControllerCourse/createCourse")
        {
            Content = JsonContent.Create(create)
        };
        Authorize(postReq, adminToken);
        var post = await _client.SendAsync(postReq);
        Assert.Equal(HttpStatusCode.OK, post.StatusCode);

        using var getReq = new HttpRequestMessage(HttpMethod.Get, "/api/v1/ControllerCourse/all");
        Authorize(getReq, adminToken);
        var get = await _client.SendAsync(getReq);
        Assert.Equal(HttpStatusCode.OK, get.StatusCode);
    }

    [Fact]
    public async Task CourseController_GetCourses_Unauthenticated_StatusIs401()
    {
        using var factory = new CustomWebApplicationFactory();
        var client = factory.CreateClient();
        var resp = await client.GetAsync("/api/v1/ControllerCourse/all");
        Assert.Equal(HttpStatusCode.Unauthorized, resp.StatusCode);
    }
}
