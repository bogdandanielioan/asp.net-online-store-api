using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using OnlineSchool.Auth.Controllers;
using OnlineSchool.Auth.Features.Commands;
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

    private static AuthController CreateController(Mock<IMediator> mediator)
    {
        return new AuthController(mediator.Object, NullLogger<AuthController>.Instance);
    }

    [Fact]
    public async Task Login_Admin_ContainsAdminRole_And_ReadWritePermissions()
    {
        var generator = BuildTokenGenerator();
        var mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.Is<LoginCommand>(cmd => cmd.Username == "admin" && cmd.Password == "admin"), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() =>
            {
                var user = new AuthenticatedUser("admin", "admin", SystemRoles.Admin);
                var generated = generator.GenerateToken(user);
                return new LoginResult(generated.Token, generated.ExpiresAt, user.Role);
            });

        var controller = CreateController(mediator);
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
        var generator = BuildTokenGenerator();
        var mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.Is<LoginCommand>(cmd => cmd.Username == "user" && cmd.Password == "user"), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() =>
            {
                var user = new AuthenticatedUser("user", "user", SystemRoles.User);
                var generated = generator.GenerateToken(user);
                return new LoginResult(generated.Token, generated.ExpiresAt, user.Role);
            });

        var controller = CreateController(mediator);
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
        var mediator = new Mock<IMediator>();
        mediator.Setup(m => m.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((LoginResult?)null);

        var controller = CreateController(mediator);
        var result = await controller.Login(new LoginRequest { Username = "admin", Password = "wrong" }, CancellationToken.None);

        Assert.IsType<Microsoft.AspNetCore.Mvc.UnauthorizedObjectResult>(result.Result);
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
