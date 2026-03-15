namespace AlAshmar.Domain.Entities.Academic;

public class Semester : Entity<Guid>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
