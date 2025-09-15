using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using OnlineSchool.Auth.Controllers;

namespace Teste.Unit;

public class AuthControllerUnitTests
{
    private static IConfiguration BuildConfig()
    {
        var dict = new Dictionary<string, string?>
        {
            ["Jwt:Issuer"] = "OnlineSchool",
            ["Jwt:Audience"] = "OnlineSchoolClients",
            ["Jwt:Key"] = "super_secret_dev_key_change_in_prod_1234567890"
        };
        return new ConfigurationBuilder().AddInMemoryCollection(dict!).Build();
    }

    [Fact]
    public void Login_Admin_ContainsAdminRole_And_ReadWritePermissions()
    {
        var controller = new AuthController(BuildConfig());
        var result = controller.Login(new LoginRequest("admin", "admin"));
        var ok = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result.Result);
        var payload = Assert.IsType<LoginResponse>(ok.Value);

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(payload.Token);
        var claims = token.Claims.ToList();
        Assert.Contains(claims, c => c.Type == ClaimTypes.Role && c.Value == "Admin");
        Assert.Contains(claims, c => c.Type == "permission" && c.Value == "read");
        Assert.Contains(claims, c => c.Type == "permission" && c.Value == "write");
    }

    [Fact]
    public void Login_User_ContainsUserRole_And_ReadPermissionOnly()
    {
        var controller = new AuthController(BuildConfig());
        var result = controller.Login(new LoginRequest("user", "user"));
        var ok = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result.Result);
        var payload = Assert.IsType<LoginResponse>(ok.Value);

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(payload.Token);
        var claims = token.Claims.ToList();
        Assert.Contains(claims, c => c.Type == ClaimTypes.Role && c.Value == "User");
        Assert.Contains(claims, c => c.Type == "permission" && c.Value == "read");
        Assert.DoesNotContain(claims, c => c.Type == "permission" && c.Value == "write");
    }

    [Fact]
    public void Login_InvalidCredentials_ReturnsUnauthorized()
    {
        var controller = new AuthController(BuildConfig());
        var result = controller.Login(new LoginRequest("admin", "wrong"));
        Assert.IsType<Microsoft.AspNetCore.Mvc.UnauthorizedObjectResult>(result.Result);
    }
}
