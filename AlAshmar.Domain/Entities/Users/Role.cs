using System.ComponentModel.DataAnnotations;

namespace AlAshmar.Domain.Entities.Users;

public class Role : Entity<Guid>
{
    [Required]
    [MaxLength(50)]
    public string Type { get; set; } = null!;

    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}
