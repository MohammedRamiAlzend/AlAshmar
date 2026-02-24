using System.Security.Claims;
using AlAshmar.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Infrastructure.Authorization.Handlers;

/// <summary>
/// Requirement for class enrollment authorization.
/// Usage: [Authorize(Policy = "EnrolledInClass")]
/// </summary>
public class ClassEnrollmentRequirement : IAuthorizationRequirement
{
}

/// <summary>
/// Handler for class enrollment authorization.
/// Checks if the user (teacher) is enrolled in the class they're trying to access.
/// </summary>
public class ClassEnrollmentHandler : AuthorizationHandler<ClassEnrollmentRequirement>
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ClassEnrollmentHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ClassEnrollmentRequirement requirement)
    {
        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return;
        }

        // Get the class ID from the route or query string
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            return;
        }

        // Try to get class ID from route values or query string
        var classIdString = httpContext.Request.RouteValues["classId"]?.ToString()
            ?? httpContext.Request.Query["classId"].FirstOrDefault();

        if (string.IsNullOrEmpty(classIdString) || !Guid.TryParse(classIdString, out var classId))
        {
            // If no class ID is provided, we can't verify enrollment
            // This is OK for endpoints that don't need class-specific access
            context.Succeed(requirement);
            return;
        }

        // Check if the teacher is enrolled in this class
        var isEnrolled = await _context.ClassTeacherEnrollments
            .AnyAsync(cte => cte.TeacherId == userId && cte.ClassId == classId);

        if (isEnrolled)
        {
            context.Succeed(requirement);
        }
    }
}
