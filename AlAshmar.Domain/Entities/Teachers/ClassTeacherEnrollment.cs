using System.ComponentModel.DataAnnotations.Schema;
using AlAshmar.Domain.Entities.Students;

namespace AlAshmar.Domain.Entities.Teachers;

public class ClassTeacherEnrollment : Entity<Guid>
{
    public Guid TeacherId { get; set; }
    [ForeignKey(nameof(TeacherId))]
    public Teacher? Teacher { get; set; }

    public bool IsMainTeacher { get; set; }

    public Guid ClassId { get; set; } // Class entity is not present in diagram; keep as FK Guid
}
