using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineSchool.Auth.Features.Commands;
using System.ComponentModel.DataAnnotations;

namespace OnlineSchool.Auth.Controllers;

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
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IMediator mediator, ILogger<AuthController> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
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

        var result = await _mediator.Send(new LoginCommand(request.Username, request.Password), cancellationToken);
        if (result is null)
        {
            _logger.LogInformation("Invalid login attempt for {Username}", request.Username);
            return Unauthorized("Invalid credentials");
        }

        return Ok(new LoginResponse(result.Token, result.ExpiresAt, result.Role));
    }
}
