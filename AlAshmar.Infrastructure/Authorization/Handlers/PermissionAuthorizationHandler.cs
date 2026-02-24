using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace AlAshmar.Infrastructure.Authorization.Handlers;

/// <summary>
/// Requirement for permission-based authorization.
/// Usage: [Authorize(Policy = "permission:students.create")]
/// </summary>
public class PermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; }

    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }
}

/// <summary>
/// Handler for permission-based authorization.
/// Checks if user has the required permission in their JWT claims.
/// </summary>
public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var permissionClaims = context.User.FindAll("permission");

        foreach (var claim in permissionClaims)
        {
            if (claim.Value == requirement.Permission)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
        }

        // Check for wildcard permission (e.g., "students.*" grants all students permissions)
        var wildcardPermission = $"{requirement.Permission.Split('.')[0]}.*";
        foreach (var claim in permissionClaims)
        {
            if (claim.Value == wildcardPermission)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
        }

        return Task.CompletedTask;
    }
}
