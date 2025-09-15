using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnlineSchool.Auth.Models;

namespace OnlineSchool.Auth.Services
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions _options;
        private readonly SigningCredentials _credentials;
        private readonly IRolePermissionResolver _permissionResolver;

        public JwtTokenGenerator(IOptions<JwtOptions> options, IRolePermissionResolver permissionResolver)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _permissionResolver = permissionResolver ?? throw new ArgumentNullException(nameof(permissionResolver));

            if (string.IsNullOrWhiteSpace(_options.Key))
            {
                throw new InvalidOperationException("Jwt:Key must be configured.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
            _credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }

        public GeneratedJwt GenerateToken(AuthenticatedUser user, IEnumerable<Claim>? customClaims = null)
        {
            ArgumentNullException.ThrowIfNull(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Name, user.DisplayName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            foreach (var permission in _permissionResolver.GetPermissions(user.Role))
            {
                claims.Add(new Claim("permission", permission));
            }

            if (customClaims != null)
            {
                claims.AddRange(customClaims);
            }

            var expiresAt = DateTime.UtcNow.AddMinutes(_options.GetValidatedLifetime());

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: _credentials
            );

            var handler = new JwtSecurityTokenHandler();
            return new GeneratedJwt(handler.WriteToken(token), expiresAt);
        }

    }
}
