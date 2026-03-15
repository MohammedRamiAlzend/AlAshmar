namespace AlAshmar.Application.DTOs.Authorization;

public record PermissionDto(
    Guid Id,
    string Name,
    string Description,
    string Resource,
    string Action
);

public record RoleDto(
    Guid Id,
    string Type,
    List<PermissionDto> Permissions
);

public record CreatePermissionDto(
    string Name,
    string Description,
    string Resource,
    string Action
);

public record AssignPermissionsToRoleDto(
    Guid RoleId,
    List<Guid> PermissionIds
);

public record AssignRoleToUserDto(
    Guid UserId,
    Guid RoleId
);

public record AuthorizationResultDto(
    bool IsAuthorized,
    string? Reason = null
);
