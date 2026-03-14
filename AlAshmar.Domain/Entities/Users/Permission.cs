using System.ComponentModel.DataAnnotations;

namespace AlAshmar.Domain.Entities.Users;




public class Permission : Entity<Guid>
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    public string Description { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string Resource { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string Action { get; set; } = null!;


    public ICollection<Role> Roles { get; set; } = new List<Role>();




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




    public string ToPermissionString() => $"{Resource}.{Action}";
}
