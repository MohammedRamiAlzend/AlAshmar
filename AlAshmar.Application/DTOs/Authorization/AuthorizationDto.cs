namespace AlAshmar.Application.DTOs.Authorization;

/// <summary>
/// DTO for Permission entity.
/// </summary>
public record PermissionDto(
    Guid Id,
    string Name,
    string Description,
    string Resource,
    string Action
);

/// <summary>
/// DTO for Role entity with permissions.
/// </summary>
public record RoleDto(
    Guid Id,
    string Type,
    List<PermissionDto> Permissions
);

/// <summary>
/// DTO for creating a new permission.
/// </summary>
public record CreatePermissionDto(
    string Name,
    string Description,
    string Resource,
    string Action
);

/// <summary>
/// DTO for assigning permissions to a role.
/// </summary>
public record AssignPermissionsToRoleDto(
    Guid RoleId,
    List<Guid> PermissionIds
);

/// <summary>
/// DTO for assigning a role to a user.
/// </summary>
public record AssignRoleToUserDto(
    Guid UserId,
    Guid RoleId
);

/// <summary>
/// DTO for authorization result.
/// </summary>
public record AuthorizationResultDto(
    bool IsAuthorized,
    string? Reason = null
);
