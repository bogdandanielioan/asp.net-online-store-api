using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using OnlineSchool.Auth.Controllers;
using OnlineSchool.Auth.Models;
using OnlineSchool.Auth.Services;

namespace Teste.Unit;

public class AuthControllerUnitTests
{
    private static IJwtTokenGenerator BuildTokenGenerator()
    {
        var options = Options.Create(new JwtOptions
        {
            Issuer = "OnlineSchool",
            Audience = "OnlineSchoolClients",
            Key = "super_secret_dev_key_change_in_prod_1234567890"
        });

        return new JwtTokenGenerator(options, new StaticRolePermissionResolver());
    }

    private static AuthController BuildController(Dictionary<string, (string Password, string Role)>? defaults = null)
    {
        var authenticator = new FakeAuthenticator(defaults);
        return new AuthController(authenticator, BuildTokenGenerator(), NullLogger<AuthController>.Instance);
    }

    [Fact]
    public async Task Login_Admin_ContainsAdminRole_And_ReadWritePermissions()
    {
        var controller = BuildController();
        var result = await controller.Login(new LoginRequest { Username = "admin", Password = "admin" }, CancellationToken.None);
        var ok = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result.Result);
        var payload = Assert.IsType<LoginResponse>(ok.Value);

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(payload.Token);
        var claims = token.Claims.ToList();
        Assert.Contains(claims, c => c.Type == ClaimTypes.Role && c.Value == SystemRoles.Admin);
        Assert.Contains(claims, c => c.Type == "permission" && c.Value == "read");
        Assert.Contains(claims, c => c.Type == "permission" && c.Value == "write");
    }

    [Fact]
    public async Task Login_User_ContainsUserRole_And_ReadPermissionOnly()
    {
        var controller = BuildController();
        var result = await controller.Login(new LoginRequest { Username = "user", Password = "user" }, CancellationToken.None);
        var ok = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result.Result);
        var payload = Assert.IsType<LoginResponse>(ok.Value);

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(payload.Token);
        var claims = token.Claims.ToList();
        Assert.Contains(claims, c => c.Type == ClaimTypes.Role && c.Value == SystemRoles.User);
        Assert.Contains(claims, c => c.Type == "permission" && c.Value == "read");
        Assert.DoesNotContain(claims, c => c.Type == "permission" && c.Value == "write");
    }

    [Fact]
    public async Task Login_InvalidCredentials_ReturnsUnauthorized()
    {
        var controller = BuildController();
        var result = await controller.Login(new LoginRequest { Username = "admin", Password = "wrong" }, CancellationToken.None);
        Assert.IsType<Microsoft.AspNetCore.Mvc.UnauthorizedObjectResult>(result.Result);
    }

    private sealed class FakeAuthenticator : IUserAuthenticator
    {
        private readonly Dictionary<string, (string Password, string Role)> _users;

        public FakeAuthenticator(Dictionary<string, (string Password, string Role)>? defaults)
        {
            _users = defaults ?? new Dictionary<string, (string Password, string Role)>(StringComparer.OrdinalIgnoreCase)
            {
                ["admin"] = ("admin", SystemRoles.Admin),
                ["user"] = ("user", SystemRoles.User)
            };
        }

        public Task<AuthenticatedUser?> AuthenticateAsync(string username, string password, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return Task.FromResult<AuthenticatedUser?>(null);
            }

            if (_users.TryGetValue(username, out var info) && info.Password == password)
            {
                return Task.FromResult<AuthenticatedUser?>(new AuthenticatedUser(username, username, info.Role));
            }

            return Task.FromResult<AuthenticatedUser?>(null);
        }
    }

    private sealed class StaticRolePermissionResolver : IRolePermissionResolver
    {
        public IReadOnlyCollection<string> GetPermissions(string role)
        {
            if (string.Equals(role, SystemRoles.Admin, StringComparison.OrdinalIgnoreCase))
            {
                return new[] { "read", "write", "read:student", "write:student", "read:course", "write:course" };
            }

            if (string.Equals(role, SystemRoles.User, StringComparison.OrdinalIgnoreCase))
            {
                return new[] { "read", "read:student", "write:student" };
            }

            return Array.Empty<string>();
        }
    }
}
