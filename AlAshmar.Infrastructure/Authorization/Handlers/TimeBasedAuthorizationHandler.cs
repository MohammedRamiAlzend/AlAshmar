using Microsoft.AspNetCore.Authorization;

namespace AlAshmar.Infrastructure.Authorization.Handlers;

/// <summary>
/// Requirement for time-based authorization.
/// Usage: [Authorize(Policy = "SchoolHours")]
/// </summary>
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

/// <summary>
/// Handler for time-based authorization.
/// Checks if the current time is within the allowed time window.
/// </summary>
public class TimeBasedAuthorizationHandler : AuthorizationHandler<TimeBasedRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        TimeBasedRequirement requirement)
    {
        var now = DateTime.Now;

        // Check if current hour is within allowed range
        if (now.Hour < requirement.StartHour || now.Hour >= requirement.EndHour)
        {
            return Task.CompletedTask;
        }

        // Check if current day is allowed
        if (requirement.AllowedDays != null && !requirement.AllowedDays.Contains(now.DayOfWeek))
        {
            return Task.CompletedTask;
        }

        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
