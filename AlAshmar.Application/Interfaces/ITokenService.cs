namespace AlAshmar.Application.Interfaces;

/// <summary>
/// Service for generating and validating JWT tokens.
/// </summary>
public interface ITokenService
{
    Task<string> GenerateTokenAsync(string username, Guid userId, Guid? roleId, CancellationToken cancellationToken = default);
    string GenerateRefreshToken();
    Task<bool> ValidateTokenAsync(string token, CancellationToken cancellationToken = default);
}
