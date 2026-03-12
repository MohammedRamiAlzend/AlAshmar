using AlAshmar.Domain.Entities.Abstraction;
using AlAshmar.Domain.Entities.Students;
using AlAshmar.Domain.Entities.Teachers;

namespace AlAshmar.Domain.Entities.Academic;

/// <summary>
/// Represents a Halaqa (learning circle/class) that belongs to a Dawra (course).
/// Each Dawra has many Halaqas, and each Halaqa has many students and teachers.
/// </summary>
public class Halaqa : Entity<Guid>
{
    public string ClassName { get; set; } = null!;

    public Guid DawraId { get; set; }
    public Dawra? Dawra { get; set; }

    public ICollection<ClassTeacherEnrollment> ClassTeacherEnrollments { get; set; } = new List<ClassTeacherEnrollment>();
    public ICollection<ClassStudentEnrollment> ClassStudentEnrollments { get; set; } = new List<ClassStudentEnrollment>();
}
