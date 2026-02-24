using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace AlAshmar.Infrastructure.Authorization.Handlers;

/// <summary>
/// Requirement for resource ownership authorization.
/// Usage: [Authorize(Policy = "OwnsResource:students")]
/// </summary>
public class ResourceOwnershipRequirement : IAuthorizationRequirement
{
    public string ResourceType { get; }

    public ResourceOwnershipRequirement(string resourceType)
    {
        ResourceType = resourceType;
    }
}

/// <summary>
/// Handler for resource ownership authorization.
/// This is a base handler - actual ownership check should be done in the service layer
/// where you have access to the resource being accessed.
/// This handler just marks that ownership check is required.
/// </summary>
public class ResourceOwnershipHandler : AuthorizationHandler<ResourceOwnershipRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ResourceOwnershipRequirement requirement)
    {
        // Note: Actual ownership verification happens in the service/use case layer
        // This handler just ensures the user is authenticated
        if (context.User.Identity?.IsAuthenticated == true)
        {
            // Mark as succeeded - actual check happens in service layer
            // The service layer should inject IAuthorizationService and call
            // the appropriate ownership check method
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
