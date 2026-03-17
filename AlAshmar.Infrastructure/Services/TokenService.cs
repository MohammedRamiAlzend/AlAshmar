using AlAshmar.Application.Interfaces;
using AlAshmar.Application.Repos;
using AlAshmar.Domain.Entities.Users;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AlAshmar.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly ITokenRepository _tokenRepository;

    public TokenService(IConfiguration configuration, ITokenRepository tokenRepository)
    {
        _configuration = configuration;
        _tokenRepository = tokenRepository;
    }

    public async Task<string> GenerateTokenAsync(string username, Guid userId, Guid? roleId, CancellationToken cancellationToken = default)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured")));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, username),
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (roleId.HasValue)
        {
            claims.Add(new Claim("RoleId", roleId.Value.ToString()));

            var role = await _tokenRepository.GetRoleWithPermissionsAsync(roleId.Value, cancellationToken);

            if (role != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Type));

                foreach (var permission in role.Permissions)
                {
                    claims.Add(new Claim("permission", permission.ToPermissionString()));
                }
            }
        }

        var expiresAt = DateTime.UtcNow.AddHours(
            int.TryParse(_configuration["Jwt:ExpiresInHours"], out var hours) ? hours : 24);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> GenerateRefreshTokenAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var tokenBytes = RandomNumberGenerator.GetBytes(64);
        var tokenString = Convert.ToBase64String(tokenBytes);

        var refreshToken = new RefreshToken
        {
            Token = tokenString,
            UserId = userId,
            ExpiresAt = DateTime.UtcNow.AddDays(
                int.TryParse(_configuration["Jwt:RefreshTokenExpiresInDays"], out var days) ? days : 7),
            IsRevoked = false
        };

        await _tokenRepository.AddRefreshTokenAsync(refreshToken, cancellationToken);
        return tokenString;
    }

    public async Task<string?> RefreshAccessTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var storedToken = await _tokenRepository.GetValidRefreshTokenAsync(refreshToken, cancellationToken);
        if (storedToken == null)
            return null;

        await _tokenRepository.RevokeRefreshTokenAsync(storedToken, cancellationToken);

        var user = storedToken.User;
        return await GenerateTokenAsync(user.UserName, user.Id, user.RoleId, cancellationToken);
    }

    public async Task<bool> ValidateTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(token))
            return false;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured")));

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out _);
            return await Task.FromResult(true);
        }
        catch
        {
            return false;
        }
    }
}
