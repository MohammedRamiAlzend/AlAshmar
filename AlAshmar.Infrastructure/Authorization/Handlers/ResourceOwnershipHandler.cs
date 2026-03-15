namespace AlAshmar.Infrastructure.Authorization.Handlers;

public class ResourceOwnershipRequirement : IAuthorizationRequirement
{
    public string ResourceType { get; }

    public ResourceOwnershipRequirement(string resourceType)
    {
        ResourceType = resourceType;
    }
}

public class ResourceOwnershipHandler : AuthorizationHandler<ResourceOwnershipRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ResourceOwnershipRequirement requirement)
    {

        if (context.User.Identity?.IsAuthenticated == true)
        {

            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
