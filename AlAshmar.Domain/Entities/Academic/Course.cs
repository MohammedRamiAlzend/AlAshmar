using AlAshmar.Domain.Entities.Abstraction;

namespace AlAshmar.Domain.Entities.Academic;

/// <summary>
/// Represents a Course (course/program) that belongs to a Semester.
/// Each semester has many Courses, and each Course has many Halaqas (learning circles).
/// </summary>
public class Course : Entity<Guid>
{
    public string EventName { get; set; } = null!;

    public Guid SemesterId { get; set; }
    public Semester? Semester { get; set; }

    public ICollection<Halaqa> Halaqas { get; set; } = new List<Halaqa>();
}
