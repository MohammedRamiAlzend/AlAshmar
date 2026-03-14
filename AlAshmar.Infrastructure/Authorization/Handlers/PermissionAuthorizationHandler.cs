using System.Security.Claims;

namespace AlAshmar.Infrastructure.Authorization.Handlers;





public class PermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; }

    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }
}





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
