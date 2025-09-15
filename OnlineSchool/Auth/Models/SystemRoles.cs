namespace OnlineSchool.Auth.Models
{
    public static class SystemRoles
    {
        public const string Admin = "Admin";
        public const string User = "User";

        public static string Normalize(string? role)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                return string.Empty;
            }

            var trimmed = role.Trim();
            if (string.Equals(trimmed, Admin, StringComparison.OrdinalIgnoreCase))
            {
                return Admin;
            }

            if (string.Equals(trimmed, User, StringComparison.OrdinalIgnoreCase))
            {
                return User;
            }

            return trimmed;
        }
    }
}
