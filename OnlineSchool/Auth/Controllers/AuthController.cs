using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineSchool.Auth.Models;
using OnlineSchool.Auth.Services;
using System.ComponentModel.DataAnnotations;

namespace OnlineSchool.Auth.Controllers
{
    public record LoginResponse(string Token, DateTime ExpiresAt, string Role);

    public record LoginRequest
    {
        [Required]
        public string Username { get; init; } = string.Empty;

        [Required]
        public string Password { get; init; } = string.Empty;
    }

    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserAuthenticator _authenticator;
        private readonly IJwtTokenGenerator _tokenGenerator;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserAuthenticator authenticator, IJwtTokenGenerator tokenGenerator, ILogger<AuthController> logger)
        {
            _authenticator = authenticator ?? throw new ArgumentNullException(nameof(authenticator));
            _tokenGenerator = tokenGenerator ?? throw new ArgumentNullException(nameof(tokenGenerator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                return Unauthorized("Invalid credentials");
            }

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var user = await _authenticator.AuthenticateAsync(request.Username, request.Password, cancellationToken);
            if (user is null)
            {
                _logger.LogInformation("Invalid login attempt for {Username}", request.Username);
                return Unauthorized("Invalid credentials");
            }

            var generated = _tokenGenerator.GenerateToken(user);
            return Ok(new LoginResponse(generated.Token, generated.ExpiresAt, user.Role));
        }
    }
}
