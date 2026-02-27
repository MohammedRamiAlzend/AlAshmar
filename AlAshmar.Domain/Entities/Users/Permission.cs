using System.ComponentModel.DataAnnotations;

namespace AlAshmar.Domain.Entities.Users;

/// <summary>
/// Represents a granular permission that can be assigned to roles.
/// </summary>
public class Permission : Entity<Guid>
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;  // e.g., "students.create"

    [Required]
    [MaxLength(255)]
    public string Description { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string Resource { get; set; } = null!;  // e.g., "students"

    [Required]
    [MaxLength(50)]
    public string Action { get; set; } = null!;    // e.g., "create"

    // Navigation
    public ICollection<Role> Roles { get; set; } = new List<Role>();

    /// <summary>
    /// Creates a permission from a string like "students.create"
    /// </summary>
    public static Permission FromString(string permissionString, string description)
    {
        var parts = permissionString.Split('.');
        if (parts.Length < 2)
            throw new ArgumentException("Permission must be in format 'resource.action'");

        return new Permission
        {
            Id = Guid.NewGuid(),
            Resource = parts[0].ToLowerInvariant(),
            Action = parts[1].ToLowerInvariant(),
            Name = permissionString,
            Description = description
        };
    }

    /// <summary>
    /// Returns the permission string in format "resource.action"
    /// </summary>
    public string ToPermissionString() => $"{Resource}.{Action}";
}
