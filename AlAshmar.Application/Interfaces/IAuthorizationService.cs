
using AlAshmar.Domain.DTOs.Domain;

namespace AlAshmar.Application.Interfaces;

public interface IAuthorizationService
{

    Task<Result<PermissionDto>> CreatePermissionAsync(CreatePermissionDto dto, CancellationToken cancellationToken = default);
    Task<Result<PermissionDto>> GetPermissionByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<List<PermissionDto>>> GetAllPermissionsAsync(CancellationToken cancellationToken = default);
    Task<Result<Success>> DeletePermissionAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<RoleDto>> GetRoleByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<List<RoleDto>>> GetAllRolesAsync(CancellationToken cancellationToken = default);
    Task<Result<RoleDto>> GetRoleByTypeAsync(string type, CancellationToken cancellationToken = default);

    Task<Result<RoleDto>> AssignPermissionsToRoleAsync(AssignPermissionsToRoleDto dto, CancellationToken cancellationToken = default);
    Task<Result<Success>> RemovePermissionFromRoleAsync(Guid roleId, Guid permissionId, CancellationToken cancellationToken = default);

    Task<Result<Success>> AssignRoleToUserAsync(AssignRoleToUserDto dto, CancellationToken cancellationToken = default);

    Task<bool> UserHasPermissionAsync(Guid userId, string permissionString, CancellationToken cancellationToken = default);
    Task<List<string>> GetUserPermissionsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> UserHasRoleAsync(Guid userId, string roleType, CancellationToken cancellationToken = default);
}
