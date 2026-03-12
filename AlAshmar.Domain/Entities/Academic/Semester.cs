using AlAshmar.Domain.Entities.Abstraction;

namespace AlAshmar.Domain.Entities.Academic;

public class Semester : Entity<Guid>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<Dawra> Dawras { get; set; } = new List<Dawra>();
}
