using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using OnlineSchool.Auth.Services;
using OnlineSchool.Data;

namespace OnlineSchool.Auth.Controllers
{
    public record LoginRequest(string Username, string Password);
    public record LoginResponse(string Token, DateTime ExpiresAt, string Role);

    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext? _db;
        public AuthController(IConfiguration config, AppDbContext? db = null)
        {
            _config = config;
            _db = db;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult<LoginResponse> Login([FromBody] LoginRequest request)
        {
            if (request is null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return Unauthorized("Invalid credentials");
            }

            string role = string.Empty;
            string displayName = request.Username!;

            if (_db != null)
            {
                var student = _db.Students.AsNoTracking().FirstOrDefault(s => s.Email == request.Username);
                if (student != null && !string.IsNullOrEmpty(student.PasswordHash) && !string.IsNullOrEmpty(student.PasswordSalt))
                {
                    if (PasswordHasher.Verify(request.Password!, student.PasswordHash!, student.PasswordSalt!))
                    {
                        role = string.IsNullOrWhiteSpace(student.Role) ? "User" : student.Role!;
                        displayName = student.Name;
                    }
                    else
                    {
                        return Unauthorized("Invalid credentials");
                    }
                }

                if (string.IsNullOrEmpty(role))
                {
                    var teacher = _db.Teachers.AsNoTracking().FirstOrDefault(t => t.Email == request.Username);
                    if (teacher != null && !string.IsNullOrEmpty(teacher.PasswordHash) && !string.IsNullOrEmpty(teacher.PasswordSalt))
                    {
                        if (PasswordHasher.Verify(request.Password!, teacher.PasswordHash!, teacher.PasswordSalt!))
                        {
                            role = string.IsNullOrWhiteSpace(teacher.Role) ? "Admin" : teacher.Role!;
                            displayName = teacher.Name;
                        }
                        else
                        {
                            return Unauthorized("Invalid credentials");
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(role))
            {
                var users = new Dictionary<string, (string Password, string Role)>(StringComparer.OrdinalIgnoreCase)
                {
                    ["admin"] = ("admin", "Admin"),
                    ["user"] = ("user", "User")
                };

                if (users.TryGetValue(request.Username!, out var u) && u.Password == request.Password)
                {
                    role = u.Role;
                }
                else
                {
                    return Unauthorized("Invalid credentials");
                }
            }

            var jwtSection = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"] ?? string.Empty));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, displayName),
                new Claim(ClaimTypes.Role, role)
            };

            if (string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                claims.Add(new Claim("permission", "read"));
                claims.Add(new Claim("permission", "write"));

                claims.Add(new Claim("permission", "read:student"));
                claims.Add(new Claim("permission", "write:student"));
                claims.Add(new Claim("permission", "read:course"));
                claims.Add(new Claim("permission", "write:course"));
            }
            else
            {
                claims.Add(new Claim("permission", "read"));

                claims.Add(new Claim("permission", "read:student"));
                claims.Add(new Claim("permission", "write:student"));
            }

            var expires = DateTime.UtcNow.AddHours(1);
            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new LoginResponse(tokenString, expires, role));
        }
    }
}
