using System.ComponentModel.DataAnnotations;

namespace OnlineSchool.Auth.Models
{
    public class JwtOptions
    {
        private const int DefaultLifetimeMinutes = 60;

        [Required]
        public string Issuer { get; set; } = string.Empty;

        [Required]
        public string Audience { get; set; } = string.Empty;

        [Required]
        public string Key { get; set; } = string.Empty;

        public int AccessTokenLifetimeMinutes { get; set; } = DefaultLifetimeMinutes;

        public int GetValidatedLifetime()
        {
            return AccessTokenLifetimeMinutes > 0 ? AccessTokenLifetimeMinutes : DefaultLifetimeMinutes;
        }
    }
}
