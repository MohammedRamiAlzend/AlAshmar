using AlAshmar.Domain.Entities.Abstraction;

namespace AlAshmar.Domain.Entities.Students;

public class StudentAttendance : Entity<Guid>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public Guid ClassStudentId { get; set; } // FK to ClassStudentEnrollment (or similar)
}
