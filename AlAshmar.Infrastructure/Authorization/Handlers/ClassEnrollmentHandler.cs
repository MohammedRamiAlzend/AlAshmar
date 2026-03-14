using System.Security.Claims;

namespace AlAshmar.Infrastructure.Authorization.Handlers;





public class ClassEnrollmentRequirement : IAuthorizationRequirement
{
}





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


        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            return;
        }


        var classIdString = httpContext.Request.RouteValues["classId"]?.ToString()
            ?? httpContext.Request.Query["classId"].FirstOrDefault();

        if (string.IsNullOrEmpty(classIdString) || !Guid.TryParse(classIdString, out var classId))
        {


            context.Succeed(requirement);
            return;
        }


        var isEnrolled = await _context.ClassTeacherEnrollments
            .AnyAsync(cte => cte.TeacherId == userId && cte.ClassId == classId);

        if (isEnrolled)
        {
            context.Succeed(requirement);
        }
    }
}
