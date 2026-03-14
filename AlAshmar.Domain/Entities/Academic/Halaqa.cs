using AlAshmar.Domain.Entities.Students;
using AlAshmar.Domain.Entities.Teachers;

namespace AlAshmar.Domain.Entities.Academic;





public class Halaqa : Entity<Guid>
{
    public string ClassName { get; set; } = null!;

    public Guid CourseId { get; set; }
    public Course? Course { get; set; }

    public ICollection<ClassTeacherEnrollment> ClassTeacherEnrollments { get; set; } = new List<ClassTeacherEnrollment>();
    public ICollection<ClassStudentEnrollment> ClassStudentEnrollments { get; set; } = new List<ClassStudentEnrollment>();
}
