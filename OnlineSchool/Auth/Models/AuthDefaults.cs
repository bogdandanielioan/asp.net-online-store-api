namespace OnlineSchool.Auth.Models
{
    public class AuthDefaults
    {
        public IList<DefaultUser> Users { get; set; } = new List<DefaultUser>();
    }

    public class DefaultUser
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = SystemRoles.User;
        public string? DisplayName { get; set; }
    }
}
