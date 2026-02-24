using AlAshmar.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlAshmar.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResult>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _authenticationService.LoginAsync(request.Username, request.Password, cancellationToken);
        return result.IsError ? Unauthorized(result.Errors) : Ok(result.Value);
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResult>> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await _authenticationService.RegisterAsync(request.Username, request.Password, request.RoleId, cancellationToken);
        return result.IsError ? BadRequest(result.Errors) : Ok(result.Value);
    }
}

public record LoginRequest(string Username, string Password);

public record RegisterRequest(string Username, string Password, Guid RoleId);
