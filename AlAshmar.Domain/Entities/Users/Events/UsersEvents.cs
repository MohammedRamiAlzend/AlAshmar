using System.Text;

namespace AlAshmar.Domain.Entities.Users.Events;


public class UserCreatedEvent : DomainEvent
{
    public UserCreatedEvent(Guid userId, string userName)
    {
        UserId = userId;
        UserName = userName;
    }

    public Guid UserId { get; }
    public string UserName { get; }
}
public class UserUpdatePasswordEvent(Guid userId) : DomainEvent
{
    public Guid UserId { get; } = userId;
}
public class UserUpdatedEvent : DomainEvent
{
    public UserUpdatedEvent(Guid userId)
    {
        userId = UserId;
    }

    public Guid UserId { get; }
}

public class UserDeletedEvent : DomainEvent
{
    public UserDeletedEvent(Guid userId)
    {
        userId = UserId;
    }

    public Guid UserId { get; }
}