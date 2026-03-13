using System.ComponentModel.DataAnnotations.Schema;
using AlAshmar.Domain.Entities.Academic;

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
