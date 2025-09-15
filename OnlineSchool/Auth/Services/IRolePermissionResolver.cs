namespace OnlineSchool.Auth.Services
{
    public interface IRolePermissionResolver
    {
        IReadOnlyCollection<string> GetPermissions(string role);
    }
}
