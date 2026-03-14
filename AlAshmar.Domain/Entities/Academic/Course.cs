
namespace AlAshmar.Domain.Entities.Academic;





public class Course : Entity<Guid>
{
    public string EventName { get; set; } = null!;

    public Guid SemesterId { get; set; }
    public Semester? Semester { get; set; }

    public ICollection<Halaqa> Halaqas { get; set; } = new List<Halaqa>();
}
