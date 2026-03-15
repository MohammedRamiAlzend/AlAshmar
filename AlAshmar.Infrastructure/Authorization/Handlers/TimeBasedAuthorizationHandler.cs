namespace AlAshmar.Infrastructure.Authorization.Handlers;

public class TimeBasedRequirement : IAuthorizationRequirement
{
    public int StartHour { get; }
    public int EndHour { get; }
    public DayOfWeek[]? AllowedDays { get; }

    public TimeBasedRequirement(int startHour = 8, int endHour = 17, DayOfWeek[]? allowedDays = null)
    {
        StartHour = startHour;
        EndHour = endHour;
        AllowedDays = allowedDays;
    }
}

public class TimeBasedAuthorizationHandler : AuthorizationHandler<TimeBasedRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        TimeBasedRequirement requirement)
    {
        var now = DateTime.Now;

        if (now.Hour < requirement.StartHour || now.Hour >= requirement.EndHour)
        {
            return Task.CompletedTask;
        }

        if (requirement.AllowedDays != null && !requirement.AllowedDays.Contains(now.DayOfWeek))
        {
            return Task.CompletedTask;
        }

        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
