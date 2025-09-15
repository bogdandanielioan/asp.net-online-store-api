using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using OnlineSchool.Courses.Dto;
using OnlineSchool.Courses.Models;

namespace Teste.Integration;

public class AuthAuthorizationIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions JsonOpts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    public AuthAuthorizationIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Unauthenticated_Request_Returns401()
    {
        using var req = new HttpRequestMessage(HttpMethod.Get, "/api/v1/ControllerCourse/all");
        req.Headers.Add("X-Test-Auth", "none");
        var resp = await _client.SendAsync(req);
        Assert.Equal(HttpStatusCode.Unauthorized, resp.StatusCode);
    }

    [Fact]
    public async Task ReadOnly_User_CanRead_But_ForbiddenToWrite()
    {
        // GET should be OK with read permission
        using (var getReq = new HttpRequestMessage(HttpMethod.Get, "/api/v1/ControllerCourse/all"))
        {
            getReq.Headers.Add("X-Test-Role", "User");
            getReq.Headers.Add("X-Test-Permissions", "read");
            var getResp = await _client.SendAsync(getReq);
            Assert.Equal(HttpStatusCode.OK, getResp.StatusCode);
        }

        // POST should be Forbidden without write permission
        var create = new CreateRequestCourse { Name = "AuthzTest1", Department = "Dept", TeacherId = "t1" };
        using (var postReq = new HttpRequestMessage(HttpMethod.Post, "/api/v1/ControllerCourse/createCourse"))
        {
            postReq.Headers.Add("X-Test-Role", "User");
            postReq.Headers.Add("X-Test-Permissions", "read");
            postReq.Content = JsonContent.Create(create);
            var postResp = await _client.SendAsync(postReq);
            Assert.Equal(HttpStatusCode.Forbidden, postResp.StatusCode);
        }
    }

    [Fact]
    public async Task WriteOnly_User_CanWrite_But_ForbiddenToRead()
    {
        // GET should be Forbidden without read permission
        using (var getReq = new HttpRequestMessage(HttpMethod.Get, "/api/v1/ControllerCourse/all"))
        {
            getReq.Headers.Add("X-Test-Role", "Admin");
            getReq.Headers.Add("X-Test-Permissions", "write");
            var getResp = await _client.SendAsync(getReq);
            Assert.Equal(HttpStatusCode.Forbidden, getResp.StatusCode);
        }

        // POST should be OK with write permission
        var create = new CreateRequestCourse { Name = "AuthzTest2", Department = "Dept", TeacherId = "t1" };
        using (var postReq = new HttpRequestMessage(HttpMethod.Post, "/api/v1/ControllerCourse/createCourse"))
        {
            postReq.Headers.Add("X-Test-Role", "Admin");
            postReq.Headers.Add("X-Test-Permissions", "write");
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
        // No override headers => default admin with read+write
        var create = new CreateRequestCourse { Name = "AuthzDefaultAdmin", Department = "Dept", TeacherId = "t1" };
        var post = await _client.PostAsJsonAsync("/api/v1/ControllerCourse/createCourse", create);
        Assert.Equal(HttpStatusCode.OK, post.StatusCode);

        var get = await _client.GetAsync("/api/v1/ControllerCourse/all");
        Assert.Equal(HttpStatusCode.OK, get.StatusCode);
    }
}
