using AlAshmar.Application.Repos;
using AlAshmar.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Infrastructure.Persistence.Repos;

public class TokenRepository(AppDbContext dbContext) : ITokenRepository
{
    public async Task<Role?> GetRoleWithPermissionsAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Roles
            .Include(r => r.Permissions)
            .FirstOrDefaultAsync(r => r.Id == roleId, cancellationToken);
    }

    public async Task AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        await dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<RefreshToken?> GetValidRefreshTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await dbContext.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == token && !rt.IsRevoked && rt.ExpiresAt > DateTime.UtcNow, cancellationToken);
    }

    public async Task RevokeRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        refreshToken.IsRevoked = true;
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users.FindAsync([userId], cancellationToken);
    }
}
