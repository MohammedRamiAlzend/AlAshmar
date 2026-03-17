namespace AlAshmar.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthenticationService authenticationService, ITokenService tokenService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<AuthResult>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await authenticationService.LoginAsync(request.Username, request.Password, cancellationToken);
        return result.IsError ? Unauthorized(result.Errors) : Ok(result.Value);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<RefreshTokenResponse>> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var newAccessToken = await tokenService.RefreshAccessTokenAsync(request.RefreshToken, cancellationToken);
        if (newAccessToken == null)
            return Unauthorized(new { message = "Invalid or expired refresh token." });

        return Ok(new RefreshTokenResponse(newAccessToken));
    }

    //[HttpPost("register-student")]
    //public async Task<ActionResult<AuthResult>> RegisterStudent([FromBody] CreateStudentCommand request, CancellationToken cancellationToken)
    //{
    //    var result = await authenticationService.RegisterStudentAsync(request, cancellationToken);
    //    return result.IsError ? BadRequest(result.Errors) : Ok(result.Value);
    //}
    //[HttpPost("register-manager")]
    //public async Task<ActionResult<AuthResult>> RegisterManeger([FromBody] CreateManagerCommand request, CancellationToken cancellationToken)
    //{
    //    var result = await authenticationService.RegisterManagerAsync(request, cancellationToken);
    //    return result.IsError ? BadRequest(result.Errors) : Ok(result.Value);
    //}
    //[HttpPost("register-teacher")]
    //public async Task<ActionResult<AuthResult>> Register([FromBody] CreateTeacherCommand request, CancellationToken cancellationToken)
    //{
    //    var result = await authenticationService.RegisterTeacherAsync(request, cancellationToken);
    //    return result.IsError ? BadRequest(result.Errors) : Ok(result.Value);
    //}
}

public record LoginRequest(string Username, string Password);
public record RefreshTokenRequest(string RefreshToken);
public record RefreshTokenResponse(string AccessToken);
