using System.ComponentModel.DataAnnotations.Schema;
using AlAshmar.Domain.Entities.Users;

namespace AlAshmar.Domain.Entities.Managers;

public class Manager : Entity<Guid>
{
    public string Name { get; set; } = null!;

    public Guid? UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    public ICollection<ManagerContactInfo> ManagerContactInfos { get; set; } = new List<ManagerContactInfo>();
    public ICollection<ManagerAttachment> ManagerAttachments { get; set; } = new List<ManagerAttachment>();
}
