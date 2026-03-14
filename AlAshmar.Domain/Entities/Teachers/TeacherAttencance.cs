
namespace AlAshmar.Domain.Entities.Teachers;

public class TeacherAttencance : Entity<Guid>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public Guid ClassTeacherId { get; set; }
}
