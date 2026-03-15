using AlAshmar.Domain.Entities.Academic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlAshmar.Domain.Entities.Students;

public class ClassStudentEnrollment : Entity<Guid>
{
    public Guid StudentId { get; set; }
    [ForeignKey(nameof(StudentId))]
    public Student? Student { get; set; }

    public Guid ClassId { get; set; }
    [ForeignKey(nameof(ClassId))]
    public Halaqa? Halaqa { get; set; }
}
