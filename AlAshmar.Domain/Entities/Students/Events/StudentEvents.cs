
namespace AlAshmar.Domain.Entities.Students.Events;

public class StudentCreatedEvent : DomainEvent
{
    public StudentCreatedEvent(Guid studentId, string name)
    {
        StudentId = studentId;
        Name = name;
    }

    public Guid StudentId { get; }
    public string Name { get; }
}

public class StudentUpdatedEvent : DomainEvent
{
    public StudentUpdatedEvent(Guid studentId)
    {
        StudentId = studentId;
    }

    public Guid StudentId { get; }
}

public class StudentDeletedEvent : DomainEvent
{
    public StudentDeletedEvent(Guid studentId)
    {
        StudentId = studentId;
    }

    public Guid StudentId { get; }
}
