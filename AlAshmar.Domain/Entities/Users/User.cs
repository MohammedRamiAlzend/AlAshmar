using AlAshmar.Domain.Entities.Users.Events;
using System.ComponentModel.DataAnnotations;

namespace AlAshmar.Domain.Entities.Users;

public class User : EntityWithEvents<Guid>
{
    [Required]
    public string UserName { get; set; } = null!;

    [Required]
    public string HashedPassword { get; set; } = null!;

    public Guid RoleId { get; set; }
    public Role Role { get; set; }

    public static User Create(string userName, string hashedPassword, Guid roleId)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = userName,
            HashedPassword = PasswordHasher.Hash(hashedPassword),
            RoleId = roleId
        };
        user.AddDomainEvent(new UserCreatedEvent(user.Id, user.UserName));
        return user;
    }

    public void UpdateUserPassword(string newHashedPassword)
    {
        this.HashedPassword = newHashedPassword;
        this.AddDomainEvent(new UserUpdatePasswordEvent(this.Id));
    }
}
