using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace OnlineSchool.Auth
{
    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string SchemeName = "Test";

        public TestAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var headers = Request.Headers;

            if (headers.TryGetValue("X-Test-Auth", out var authMode) && string.Equals(authMode.ToString(), "none", StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            var role = headers.TryGetValue("X-Test-Role", out var roleHdr) && !string.IsNullOrWhiteSpace(roleHdr)
                ? roleHdr.ToString()
                : "Admin";

            var permissions = new List<string>();
            if (headers.TryGetValue("X-Test-Permissions", out var permsHdr) && !string.IsNullOrWhiteSpace(permsHdr))
            {
                permissions = permsHdr.ToString()
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(p => p.ToLowerInvariant())
                    .ToList();
            }
            else
            {
                permissions.AddRange(new[] { "read", "write", "read:student", "write:student" });
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user"),
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, role)
            };
            foreach (var p in permissions.Distinct())
            {
                claims.Add(new Claim("permission", p));
            }

            var identity = new ClaimsIdentity(claims, SchemeName);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, SchemeName);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
