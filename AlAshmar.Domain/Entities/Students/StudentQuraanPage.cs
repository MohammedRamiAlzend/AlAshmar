using System.ComponentModel.DataAnnotations.Schema;
using AlAshmar.Domain.Entities.Academic;
using AlAshmar.Domain.Entities.Teachers;

namespace AlAshmar.Domain.Entities.Students;

public class StudentQuraanPage : Entity<Guid>
{
    public int PageNumber { get; set; }

    public Guid StudentId { get; set; }
    [ForeignKey(nameof(StudentId))]
    public Student? Student { get; set; }

    public Guid? TeacherId { get; set; }
    [ForeignKey(nameof(TeacherId))]
    public Teacher? Teacher { get; set; }

    public Guid? ClassId { get; set; }
    [ForeignKey(nameof(ClassId))]
    public Halaqa? Halaqa { get; set; }

    public DateTime? MemorizedAt { get; set; }
    public string? Status { get; set; }
    public string? Notes { get; set; }
}
