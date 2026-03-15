using AlAshmar.Domain.Entities.Users;

namespace AlAshmar.Infrastructure.Services;

public class AuthorizationService : Application.Interfaces.IAuthorizationService
{
    private readonly AppDbContext _context;

    public AuthorizationService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<PermissionDto>> CreatePermissionAsync(CreatePermissionDto dto, CancellationToken cancellationToken = default)
    {
        var permission = new Permission
        {
            Name = dto.Name,
            Description = dto.Description,
            Resource = dto.Resource,
            Action = dto.Action
        };

        _context.Permissions.Add(permission);
        await _context.SaveChangesAsync(cancellationToken);

        return new PermissionDto(permission.Id, permission.Name, permission.Description, permission.Resource, permission.Action);
    }

    public async Task<Result<PermissionDto>> GetPermissionByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var permission = await _context.Permissions.FindAsync([id], cancellationToken);
        if (permission == null)
            return ApplicationErrors.PermissionNotFound;

        return new PermissionDto(permission.Id, permission.Name, permission.Description, permission.Resource, permission.Action);
    }

    public async Task<Result<List<PermissionDto>>> GetAllPermissionsAsync(CancellationToken cancellationToken = default)
    {
        var permissions = await _context.Permissions.ToListAsync(cancellationToken);
        return permissions.Select(p => new PermissionDto(p.Id, p.Name, p.Description, p.Resource, p.Action)).ToList();
    }

    public async Task<Result<Success>> DeletePermissionAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var permission = await _context.Permissions.FindAsync([id], cancellationToken);
        if (permission == null)
            return ApplicationErrors.PermissionNotFound;

        _context.Permissions.Remove(permission);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }

    public async Task<Result<RoleDto>> GetRoleByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var role = await _context.Roles
            .Include(r => r.Permissions)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        if (role == null)
            return ApplicationErrors.RoleNotFound;

        return MapToRoleDto(role);
    }

    public async Task<Result<List<RoleDto>>> GetAllRolesAsync(CancellationToken cancellationToken = default)
    {
        var roles = await _context.Roles
            .Include(r => r.Permissions)
            .ToListAsync(cancellationToken);

        return roles.Select(MapToRoleDto).ToList();
    }

    public async Task<Result<RoleDto>> GetRoleByTypeAsync(string type, CancellationToken cancellationToken = default)
    {
        var role = await _context.Roles
            .Include(r => r.Permissions)
            .FirstOrDefaultAsync(r => r.Type == type, cancellationToken);

        if (role == null)
            return ApplicationErrors.RoleNotFound;

        return MapToRoleDto(role);
    }

    public async Task<Result<RoleDto>> AssignPermissionsToRoleAsync(AssignPermissionsToRoleDto dto, CancellationToken cancellationToken = default)
    {
        var role = await _context.Roles
            .Include(r => r.Permissions)
            .FirstOrDefaultAsync(r => r.Id == dto.RoleId, cancellationToken);

        if (role == null)
            return ApplicationErrors.RoleNotFound;

        var permissions = await _context.Permissions
            .Where(p => dto.PermissionIds.Contains(p.Id))
            .ToListAsync(cancellationToken);

        if (permissions.Count != dto.PermissionIds.Count)
            return ApplicationErrors.SomePermissionsNotFound;

        role.Permissions.Clear();
        foreach (var permission in permissions)
        {
            role.Permissions.Add(permission);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return MapToRoleDto(role);
    }

    public async Task<Result<Success>> RemovePermissionFromRoleAsync(Guid roleId, Guid permissionId, CancellationToken cancellationToken = default)
    {
        var role = await _context.Roles
            .Include(r => r.Permissions)
            .FirstOrDefaultAsync(r => r.Id == roleId, cancellationToken);

        if (role == null)
            return ApplicationErrors.RoleNotFound;

        var permission = role.Permissions.FirstOrDefault(p => p.Id == permissionId);
        if (permission == null)
            return ApplicationErrors.PermissionNotAssignedToRole;

        role.Permissions.Remove(permission);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }

    public async Task<Result<Success>> AssignRoleToUserAsync(AssignRoleToUserDto dto, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.FindAsync([dto.UserId], cancellationToken);
        if (user == null)
            return ApplicationErrors.UserNotFound;

        var role = await _context.Roles.FindAsync([dto.RoleId], cancellationToken);
        if (role == null)
            return ApplicationErrors.RoleNotFound;

        user.RoleId = dto.RoleId;
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }

    public async Task<bool> UserHasPermissionAsync(Guid userId, string permissionString, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .ThenInclude(r => r!.Permissions)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user?.Role == null)
            return false;

        return user.Role.Permissions.Any(p => p.ToPermissionString() == permissionString);
    }

    public async Task<List<string>> GetUserPermissionsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .ThenInclude(r => r!.Permissions)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user?.Role == null)
            return [];

        return user.Role.Permissions.Select(p => p.ToPermissionString()).ToList();
    }

    public async Task<bool> UserHasRoleAsync(Guid userId, string roleType, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        return user?.Role?.Type == roleType;
    }

    private static RoleDto MapToRoleDto(Role role) => new(
        role.Id,
        role.Type,
        role.Permissions.Select(p => new PermissionDto(p.Id, p.Name, p.Description, p.Resource, p.Action)).ToList()
    );
}
