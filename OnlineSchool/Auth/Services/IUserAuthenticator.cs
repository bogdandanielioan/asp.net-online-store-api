using OnlineSchool.Auth.Models;

namespace OnlineSchool.Auth.Services
{
    public interface IUserAuthenticator
    {
        Task<AuthenticatedUser?> AuthenticateAsync(string username, string password, CancellationToken cancellationToken = default);
    }
}
