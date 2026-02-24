using AlAshmar.Application.Interfaces;
using AlAshmar.Domain.Commons;
using AlAshmar.Domain.Entities.Users;
using AlAshmar.Application.Repos;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Infrastructure.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IRepositoryBase<User, Guid> _userRepository;
    private readonly ITokenService _tokenService;

    public AuthenticationService(
        IRepositoryBase<User, Guid> userRepository,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<Result<AuthResult>> LoginAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetAsync(
            u => u.UserName == username,
            transform: q => q.Include(u => u.Role)
        );

        if (user.IsError || user.Value == null)
            return new Error("401", "Invalid username or password", ErrorKind.Unauthorized);

        // TODO: Use proper password hashing (BCrypt/Argon2)
        if (user.Value.Password != password)
            return new Error("401", "Invalid username or password", ErrorKind.Unauthorized);

        var token = await _tokenService.GenerateTokenAsync(user.Value.UserName, user.Value.Id, user.Value.RoleId, cancellationToken);
        var refreshToken = _tokenService.GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddHours(24);

        return new AuthResult(token, refreshToken, expiresAt);
    }

    public async Task<Result<AuthResult>> RegisterAsync(string username, string password, Guid roleId, CancellationToken cancellationToken = default)
    {
        // Check if user already exists
        var existingUser = await _userRepository.GetAsync(u => u.UserName == username);
        if (!existingUser.IsError && existingUser.Value != null)
            return new Error("400", "Username already exists", ErrorKind.Validation);

        var user = new User
        {
            UserName = username,
            Password = password, // TODO: Hash password
            RoleId = roleId
        };

        var result = await _userRepository.AddAsync(user);
        if (result.IsError)
            return result.Errors;

        var token = await _tokenService.GenerateTokenAsync(user.UserName, user.Id, user.RoleId, cancellationToken);
        var refreshToken = _tokenService.GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddHours(24);

        return new AuthResult(token, refreshToken, expiresAt);
    }
}
