using AlAshmar.Application.Interfaces;
using System.Security.Claims;

namespace AlAshmar.Infrastructure.Services;

public class HttpContextServiceManager(IHttpContextAccessor httpContextAccessor) : IHttpContextServiceManager
{
    public Guid? GetCurrentUserId()
    {
        var user = httpContextAccessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated == true)
        {
            var nameIdentifier = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(nameIdentifier, out var userId))
                return userId;
        }
        return null;
    }

    public string? GetCurrentUserName()
    {
        var user = httpContextAccessor.HttpContext?.User;
        return user?.Identity?.Name;
    }

    public bool IsAuthenticated()
    {
        return httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true;
    }
}
