using AlAshmar.Domain.Entities.Abstraction;

namespace AlAshmar.Domain.Entities.Academic;

public class PointCategory : Entity<Guid>
{
    public string Type { get; set; } = null!;
}
