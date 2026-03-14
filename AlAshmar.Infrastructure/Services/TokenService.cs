using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AlAshmar.Application.Interfaces;
using AlAshmar.Domain.Entities.Users;
using Microsoft.IdentityModel.Tokens;

namespace AlAshmar.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;

    public TokenService(IConfiguration configuration, AppDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    public async Task<string> GenerateTokenAsync(string username, Guid userId, Guid? roleId, CancellationToken cancellationToken = default)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured")));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var audience = _configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience not configured");

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, username),
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (roleId.HasValue)
        {
            claims.Add(new Claim("RoleId", roleId.Value.ToString()));


            var role = await _context.Roles
                .Include(r => r.Permissions)
                .FirstOrDefaultAsync(r => r.Id == roleId.Value, cancellationToken);

            if (role != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Type));
                audience = role.Type;


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
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<bool> ValidateTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(token))
            return false;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured")));

        var tokenHandler = new JwtSecurityTokenHandler();
        var validAudiences = await GetValidAudiencesAsync(cancellationToken);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudiences = validAudiences,
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

    private async Task<IEnumerable<string>> GetValidAudiencesAsync(CancellationToken cancellationToken)
    {
        var audiences = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var defaultAudience = _configuration["Jwt:Audience"];

        if (!string.IsNullOrWhiteSpace(defaultAudience))
        {
            audiences.Add(defaultAudience);
        }

        var roleAudiences = await _context.Roles
            .AsNoTracking()
            .Where(r => !string.IsNullOrWhiteSpace(r.Type))
            .Select(r => r.Type)
            .Distinct()
            .ToListAsync(cancellationToken);

        foreach (var roleAudience in roleAudiences)
        {
            audiences.Add(roleAudience);
        }

        if (audiences.Count == 0)
            throw new InvalidOperationException("No JWT audiences are configured");

        return audiences;
    }
}
