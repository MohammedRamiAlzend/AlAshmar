
namespace AlAshmar.Domain.Entities.Teachers.Events;

public class TeacherCreatedEvent : DomainEvent
{
    public TeacherCreatedEvent(Guid teacherId, string name)
    {
        TeacherId = teacherId;
        Name = name;
    }

    public Guid TeacherId { get; }
    public string Name { get; }
}

public class TeacherUpdatedEvent : DomainEvent
{
    public TeacherUpdatedEvent(Guid teacherId)
    {
        TeacherId = teacherId;
    }

    public Guid TeacherId { get; }
}

public class TeacherDeletedEvent : DomainEvent
{
    public TeacherDeletedEvent(Guid teacherId)
    {
        TeacherId = teacherId;
    }

    public Guid TeacherId { get; }
}
