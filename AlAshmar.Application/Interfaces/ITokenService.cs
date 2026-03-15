namespace AlAshmar.Application.Interfaces;

public interface ITokenService
{
    Task<string> GenerateTokenAsync(string username, Guid userId, Guid? roleId, CancellationToken cancellationToken = default);
    Task<bool> ValidateTokenAsync(string token, CancellationToken cancellationToken = default);
}
