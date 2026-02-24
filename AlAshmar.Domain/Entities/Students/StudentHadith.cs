using System.ComponentModel.DataAnnotations.Schema;
using AlAshmar.Domain.Entities.Academic;
using AlAshmar.Domain.Entities.Teachers;

namespace AlAshmar.Domain.Entities.Students;

public class StudentHadith : Entity<Guid>
{
    public Guid HadithId { get; set; }
    [ForeignKey(nameof(HadithId))]
    public Hadith? Hadith { get; set; }

    public Guid StudentId { get; set; }
    [ForeignKey(nameof(StudentId))]
    public Student? Student { get; set; }

    public Guid? TeacherId { get; set; }
    [ForeignKey(nameof(TeacherId))]
    public Teacher? Teacher { get; set; }

    public Guid? ClassId { get; set; } // class entity not in diagram

    public DateTime? MemorizedAt { get; set; }
    public string? Status { get; set; }
    public string? Notes { get; set; }
}
