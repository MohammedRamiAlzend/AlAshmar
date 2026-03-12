using System.ComponentModel.DataAnnotations.Schema;
using AlAshmar.Domain.Entities.Academic;

namespace AlAshmar.Domain.Entities.Students;

public class StudentClassEventsPoint : Entity<Guid>
{
    public Guid StudentId { get; set; }
    [ForeignKey(nameof(StudentId))]
    public Student? Student { get; set; }

    public Guid ClassId { get; set; }
    [ForeignKey(nameof(ClassId))]
    public Halaqa? Halaqa { get; set; }

    public Guid SmesterId { get; set; } // spelled as in diagram
    [ForeignKey(nameof(SmesterId))]
    public Semester? Semester { get; set; }

    public Guid EventId { get; set; }
    [ForeignKey(nameof(EventId))]
    public Course? Course { get; set; }

    public int QuranPoints { get; set; }
    public int HadithPoints { get; set; }
    public int AttendancePoints { get; set; }
    public int BehaviorPoints { get; set; }
    public int TotalPoints { get; set; }
}

