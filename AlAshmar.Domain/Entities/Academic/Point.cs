using System.ComponentModel.DataAnnotations.Schema;
using AlAshmar.Domain.Entities.Students;
using AlAshmar.Domain.Entities.Teachers;

namespace AlAshmar.Domain.Entities.Academic;

public class Point : Entity<Guid>
{
    public Guid StudentId { get; set; }
    [ForeignKey(nameof(StudentId))]
    public Student? Student { get; set; }

    public Guid EventId { get; set; }
    [ForeignKey(nameof(EventId))]
    public Course? Course { get; set; }

    public Guid ClassId { get; set; }
    [ForeignKey(nameof(ClassId))]
    public Halaqa? Halaqa { get; set; }

    public Guid SmesterId { get; set; }

    public int PointValue { get; set; }

    public Guid? CategoryId { get; set; }
    [ForeignKey(nameof(CategoryId))]
    public PointCategory? Category { get; set; }

    public Guid? GivenByTeacherId { get; set; }
    [ForeignKey(nameof(GivenByTeacherId))]
    public Teacher? GivenByTeacher { get; set; }

    public string? Notes { get; set; }
}
