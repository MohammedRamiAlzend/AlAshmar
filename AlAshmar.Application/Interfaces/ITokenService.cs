namespace AlAshmar.Application.Interfaces;

public interface ITokenService
{
    Task<string> GenerateTokenAsync(string username, Guid userId, Guid? roleId, CancellationToken cancellationToken = default);
    Task<bool> ValidateTokenAsync(string token, CancellationToken cancellationToken = default);
    Task<string> GenerateRefreshTokenAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<string?> RefreshAccessTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
}
