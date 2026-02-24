using AlAshmar.Infrastructure.Authorization.Handlers;
using Microsoft.AspNetCore.Authorization;

namespace AlAshmar.Infrastructure.Authorization.Attributes;

/// <summary>
/// Attribute to require a specific permission.
/// Usage: [RequirePermission("students.create")]
/// </summary>
public class RequirePermissionAttribute : AuthorizeAttribute
{
    public RequirePermissionAttribute(string permission)
        : base(policy: $"permission:{permission}")
    {
    }
}

/// <summary>
/// Attribute to require resource ownership.
/// Usage: [RequireOwnership("students")]
/// </summary>
public class RequireOwnershipAttribute : AuthorizeAttribute
{
    public RequireOwnershipAttribute(string resourceType)
        : base(policy: $"owns:{resourceType}")
    {
    }
}

/// <summary>
/// Attribute to require class enrollment.
/// Usage: [RequireClassEnrollment]
/// </summary>
public class RequireClassEnrollmentAttribute : AuthorizeAttribute
{
    public RequireClassEnrollmentAttribute()
        : base(policy: "EnrolledInClass")
    {
    }
}

/// <summary>
/// Attribute to require access during school hours.
/// Usage: [RequireSchoolHours]
/// </summary>
public class RequireSchoolHoursAttribute : AuthorizeAttribute
{
    public RequireSchoolHoursAttribute()
        : base(policy: "SchoolHours")
    {
    }
}

/// <summary>
/// Attribute to require access during exam hours (8 AM - 4 PM, excluding Friday).
/// Usage: [RequireExamHours]
/// </summary>
public class RequireExamHoursAttribute : AuthorizeAttribute
{
    public RequireExamHoursAttribute()
        : base(policy: "ExamHours")
    {
    }
}

/// <summary>
/// Combined attribute for layered authorization.
/// Usage: [AuthorizeWithLayer(Roles = "Teacher", Permission = "points.assign")]
/// </summary>
public class AuthorizeWithLayerAttribute : AuthorizeAttribute
{
    public AuthorizeWithLayerAttribute(string? roles = null, string? permission = null)
        : base(policy: BuildPolicy(roles, permission))
    {
    }

    private static string BuildPolicy(string? roles, string? permission)
    {
        if (!string.IsNullOrEmpty(roles) && !string.IsNullOrEmpty(permission))
        {
            return $"Combined:{roles}:{permission}";
        }
        if (!string.IsNullOrEmpty(roles))
        {
            return roles;
        }
        if (!string.IsNullOrEmpty(permission))
        {
            return $"permission:{permission}";
        }
        return "Authenticated";
    }
}
