using System.ComponentModel.DataAnnotations.Schema;
using AlAshmar.Domain.Entities.Academic;

namespace AlAshmar.Domain.Entities.Teachers;

public class ClassTeacherEnrollment : Entity<Guid>
{
    public Guid TeacherId { get; set; }
    [ForeignKey(nameof(TeacherId))]
    public Teacher? Teacher { get; set; }

    public bool IsMainTeacher { get; set; }

    public Guid ClassId { get; set; }
    [ForeignKey(nameof(ClassId))]
    public Halaqa? Halaqa { get; set; }
}
