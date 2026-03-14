
namespace AlAshmar.Domain.Events;




public abstract class EntityWithEvents<TKey> : Entity<TKey>, IHasDomainEvents
{
    private readonly List<DomainEvent> _domainEvents = [];

    public List<DomainEvent> DomainEvents => _domainEvents;

    public void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
