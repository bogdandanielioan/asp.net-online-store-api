using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OnlineSchool.Auth.Models;

namespace OnlineSchool.Auth.Services
{
    public class RolePermissionResolver : IRolePermissionResolver
    {
        private readonly IOptionsMonitor<RolePermissionsOptions> _optionsMonitor;
        private readonly ILogger<RolePermissionResolver> _logger;

        private static readonly IReadOnlyCollection<string> AdminDefaults =
            new[] { "read", "write", "read:student", "write:student", "read:course", "write:course" };

        private static readonly IReadOnlyCollection<string> UserDefaults =
            new[] { "read", "read:student", "write:student" };

        public RolePermissionResolver(IOptionsMonitor<RolePermissionsOptions> optionsMonitor, ILogger<RolePermissionResolver> logger)
        {
            _optionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IReadOnlyCollection<string> GetPermissions(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                return Array.Empty<string>();
            }

            var normalizedRole = SystemRoles.Normalize(role);

            var snapshot = _optionsMonitor.CurrentValue;
            if (snapshot?.Permissions != null && snapshot.Permissions.Count > 0)
            {
                foreach (var entry in snapshot.Permissions)
                {
                    if (!string.Equals(entry.Key, normalizedRole, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    var sanitized = Sanitize(entry.Value);
                    if (sanitized.Count > 0)
                    {
                        return sanitized;
                    }

                    _logger.LogWarning("Configured permissions for role {Role} are empty; falling back to defaults.", normalizedRole);
                    break;
                }
            }

            return ResolveDefaults(normalizedRole);
        }

        private static IReadOnlyCollection<string> ResolveDefaults(string role)
        {
            if (string.Equals(role, SystemRoles.Admin, StringComparison.OrdinalIgnoreCase))
            {
                return AdminDefaults;
            }

            if (string.Equals(role, SystemRoles.User, StringComparison.OrdinalIgnoreCase))
            {
                return UserDefaults;
            }

            return Array.Empty<string>();
        }

        private static IReadOnlyCollection<string> Sanitize(string[]? configured)
        {
            if (configured is null || configured.Length == 0)
            {
                return Array.Empty<string>();
            }

            var perms = configured
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Select(p => p.Trim().ToLowerInvariant())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            if (perms.Length == 0)
            {
                return Array.Empty<string>();
            }

            return perms;
        }
    }
}
