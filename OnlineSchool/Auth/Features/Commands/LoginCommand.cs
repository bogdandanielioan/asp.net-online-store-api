using MediatR;
using OnlineSchool.Auth.Models;
using OnlineSchool.Auth.Services;

namespace OnlineSchool.Auth.Features.Commands;

public record LoginResult(string Token, DateTime ExpiresAt, string Role);

public record LoginCommand(string Username, string Password) : IRequest<LoginResult?>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult?>
{
    private readonly IUserAuthenticator _authenticator;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public LoginCommandHandler(IUserAuthenticator authenticator, IJwtTokenGenerator tokenGenerator)
    {
        _authenticator = authenticator;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<LoginResult?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _authenticator.AuthenticateAsync(request.Username, request.Password, cancellationToken);
        if (user is null)
        {
            return null;
        }

        var token = _tokenGenerator.GenerateToken(user);
        return new LoginResult(token.Token, token.ExpiresAt, user.Role);
    }
}
