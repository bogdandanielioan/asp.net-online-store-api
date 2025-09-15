using System.Security.Claims;
using OnlineSchool.Auth.Models;

namespace OnlineSchool.Auth.Services
{
    public interface IJwtTokenGenerator
    {
        GeneratedJwt GenerateToken(AuthenticatedUser user, IEnumerable<Claim>? customClaims = null);
    }
}
