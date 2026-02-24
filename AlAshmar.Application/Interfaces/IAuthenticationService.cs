using AlAshmar.Domain.Commons;

namespace AlAshmar.Application.Interfaces;

/// <summary>
/// Service for handling authentication operations.
/// </summary>
public interface IAuthenticationService
{
    Task<Result<AuthResult>> LoginAsync(string username, string password, CancellationToken cancellationToken = default);
    Task<Result<AuthResult>> RegisterAsync(string username, string password, Guid roleId, CancellationToken cancellationToken = default);
}

/// <summary>
/// Result of an authentication operation.
/// </summary>
public record AuthResult(string Token, string RefreshToken, DateTime ExpiresAt);
