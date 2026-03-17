using AlAshmar.Domain.Commons;
using AlAshmar.Domain.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlAshmar.Domain.Entities.Managers;

public class Manager : Entity<Guid>
{
    public string Name { get; private set; } = null!;

    public Guid? UserId { get; private set; }
    [ForeignKey(nameof(UserId))]
    public User? User { get; private set; }

    public ICollection<ManagerContactInfo> ManagerContactInfos { get; private set; } = new List<ManagerContactInfo>();
    public ICollection<ManagerAttachment> ManagerAttachments { get; private set; } = new List<ManagerAttachment>();

    public void UpdateBasicInfo(string name)
    {
        Name = name;
    }

    public static Manager Create(string name, string userName, string password)
    {
        return new Manager
        {
            Name = name,
            User = User.Create(userName, password, Constants.DefaultManagerRoleId)
        };
    }
}
