using AlAshmar.Application.DTOs.Authorization;
using AlAshmar.Domain.Commons;
using AlAshmar.Domain.Entities.Users;

namespace AlAshmar.Application.Interfaces;

/// <summary>
/// Service for managing authorization operations.
/// </summary>
public interface IAuthorizationService
{
    // Permission operations
    Task<Result<PermissionDto>> CreatePermissionAsync(CreatePermissionDto dto, CancellationToken cancellationToken = default);
    Task<Result<PermissionDto>> GetPermissionByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<List<PermissionDto>>> GetAllPermissionsAsync(CancellationToken cancellationToken = default);
    Task<Result<Success>> DeletePermissionAsync(Guid id, CancellationToken cancellationToken = default);

    // Role operations
    Task<Result<RoleDto>> GetRoleByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<List<RoleDto>>> GetAllRolesAsync(CancellationToken cancellationToken = default);
    Task<Result<RoleDto>> GetRoleByTypeAsync(string type, CancellationToken cancellationToken = default);

    // Role-Permission operations
    Task<Result<RoleDto>> AssignPermissionsToRoleAsync(AssignPermissionsToRoleDto dto, CancellationToken cancellationToken = default);
    Task<Result<Success>> RemovePermissionFromRoleAsync(Guid roleId, Guid permissionId, CancellationToken cancellationToken = default);

    // User-Role operations
    Task<Result<Success>> AssignRoleToUserAsync(AssignRoleToUserDto dto, CancellationToken cancellationToken = default);

    // Permission checks
    Task<bool> UserHasPermissionAsync(Guid userId, string permissionString, CancellationToken cancellationToken = default);
    Task<List<string>> GetUserPermissionsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> UserHasRoleAsync(Guid userId, string roleType, CancellationToken cancellationToken = default);
}
