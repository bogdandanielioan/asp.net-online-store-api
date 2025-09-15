using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OnlineSchool.Auth.Models;
using OnlineSchool.Data;

namespace OnlineSchool.Auth.Services
{
    public class UserAuthenticator : IUserAuthenticator
    {
        private readonly AppDbContext? _dbContext;
        private readonly IReadOnlyDictionary<string, DefaultUser> _defaults;
        private readonly ILogger<UserAuthenticator> _logger;

        public UserAuthenticator(AppDbContext? dbContext, IOptions<AuthDefaults> defaults, ILogger<UserAuthenticator> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
            _defaults = BuildDefaults(defaults?.Value);
        }

        public async Task<AuthenticatedUser?> AuthenticateAsync(string username, string password, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            var normalizedUsername = username.Trim();

            if (_dbContext != null)
            {
                var student = await TryAuthenticateStudent(normalizedUsername, password, cancellationToken);
                if (student != null)
                {
                    return student;
                }

                var teacher = await TryAuthenticateTeacher(normalizedUsername, password, cancellationToken);
                if (teacher != null)
                {
                    return teacher;
                }
            }

            if (_defaults.TryGetValue(normalizedUsername, out var fallback) && string.Equals(fallback.Password, password, StringComparison.Ordinal))
            {
                var displayName = string.IsNullOrWhiteSpace(fallback.DisplayName) ? fallback.Username : fallback.DisplayName!;
                var role = string.IsNullOrWhiteSpace(fallback.Role) ? SystemRoles.User : SystemRoles.Normalize(fallback.Role);
                return new AuthenticatedUser(fallback.Username, displayName, role);
            }

            return null;
        }

        private async Task<AuthenticatedUser?> TryAuthenticateStudent(string username, string password, CancellationToken cancellationToken)
        {
            if (_dbContext == null)
            {
                return null;
            }

            var student = await _dbContext.Students.AsNoTracking()
                .Where(s => s.Email == username)
                .Select(s => new
                {
                    s.Email,
                    s.Name,
                    s.Role,
                    s.PasswordHash,
                    s.PasswordSalt
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (student != null && !string.IsNullOrEmpty(student.PasswordHash) && !string.IsNullOrEmpty(student.PasswordSalt))
            {
                if (PasswordHasher.Verify(password, student.PasswordHash, student.PasswordSalt))
                {
                    var role = string.IsNullOrWhiteSpace(student.Role) ? SystemRoles.User : SystemRoles.Normalize(student.Role);
                    return new AuthenticatedUser(student.Email!, student.Name!, role);
                }

                _logger.LogInformation("Failed password verification for student {Username}", username);
            }

            return null;
        }

        private async Task<AuthenticatedUser?> TryAuthenticateTeacher(string username, string password, CancellationToken cancellationToken)
        {
            if (_dbContext == null)
            {
                return null;
            }

            var teacher = await _dbContext.Teachers.AsNoTracking()
                .Where(t => t.Email == username)
                .Select(t => new
                {
                    t.Email,
                    t.Name,
                    t.Role,
                    t.PasswordHash,
                    t.PasswordSalt
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (teacher != null && !string.IsNullOrEmpty(teacher.PasswordHash) && !string.IsNullOrEmpty(teacher.PasswordSalt))
            {
                if (PasswordHasher.Verify(password, teacher.PasswordHash, teacher.PasswordSalt))
                {
                    var role = string.IsNullOrWhiteSpace(teacher.Role) ? SystemRoles.Admin : SystemRoles.Normalize(teacher.Role);
                    return new AuthenticatedUser(teacher.Email!, teacher.Name!, role);
                }

                _logger.LogInformation("Failed password verification for teacher {Username}", username);
            }

            return null;
        }

        private static IReadOnlyDictionary<string, DefaultUser> BuildDefaults(AuthDefaults? defaults)
        {
            if (defaults?.Users != null && defaults.Users.Count > 0)
            {
                return defaults.Users
                    .Where(u => !string.IsNullOrWhiteSpace(u.Username) && !string.IsNullOrWhiteSpace(u.Password))
                    .ToDictionary(u => u.Username, u => u, StringComparer.OrdinalIgnoreCase);
            }

            return new Dictionary<string, DefaultUser>(StringComparer.OrdinalIgnoreCase)
            {
                ["admin"] = new DefaultUser { Username = "admin", Password = "admin", Role = SystemRoles.Admin },
                ["user"] = new DefaultUser { Username = "user", Password = "user", Role = SystemRoles.User }
            };
        }
    }
}
