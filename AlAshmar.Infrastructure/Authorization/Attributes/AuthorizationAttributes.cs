namespace AlAshmar.Infrastructure.Authorization.Attributes;

public class RequirePermissionAttribute : AuthorizeAttribute
{
    public RequirePermissionAttribute(string permission)
        : base(policy: $"permission:{permission}")
    {
    }
}

public class RequireOwnershipAttribute : AuthorizeAttribute
{
    public RequireOwnershipAttribute(string resourceType)
        : base(policy: $"owns:{resourceType}")
    {
    }
}

public class RequireClassEnrollmentAttribute : AuthorizeAttribute
{
    public RequireClassEnrollmentAttribute()
        : base(policy: "EnrolledInClass")
    {
    }
}

public class RequireSchoolHoursAttribute : AuthorizeAttribute
{
    public RequireSchoolHoursAttribute()
        : base(policy: "SchoolHours")
    {
    }
}

public class RequireExamHoursAttribute : AuthorizeAttribute
{
    public RequireExamHoursAttribute()
        : base(policy: "ExamHours")
    {
    }
}

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
