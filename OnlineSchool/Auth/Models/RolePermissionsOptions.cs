namespace OnlineSchool.Auth.Models
{
    public class RolePermissionsOptions
    {
        public Dictionary<string, string[]> Permissions { get; set; } = new();
    }
}
