using AlAshmar.Domain.Entities.Academic;
using AlAshmar.Domain.Entities.Teachers.Events;
using AlAshmar.Domain.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlAshmar.Domain.Entities.Teachers;

public class Teacher : EntityWithEvents<Guid>
{
    public string Name { get; private set; } = null!;
    public string FatherName { get; private set; } = null!;
    public string MotherName { get; private set; } = null!;
    public string NationalityNumber { get; private set; } = null!;
    public string? Email { get; private set; }

    public Guid UserId { get; private set; }
    public User RelatedUser { get; private set; } = null!;

    public ICollection<TeacherContactInfo> TeacherContactInfos { get; private set; } = new List<TeacherContactInfo>();
    public ICollection<TeacherAttachment> TeacherAttachments { get; private set; } = new List<TeacherAttachment>();
    public ICollection<ClassTeacherEnrollment> ClassTeacherEnrollments { get; private set; } = new List<ClassTeacherEnrollment>();
    public ICollection<Point> GivenPoints { get; private set; } = new List<Point>();

    public void UpdateBasicInfo(string name, string fatherName, string motherName, string nationalityNumber, string? email)
    {
        Name = name;
        FatherName = fatherName;
        MotherName = motherName;
        NationalityNumber = nationalityNumber;
        Email = email;
        AddDomainEvent(new TeacherUpdatedEvent(Id));
    }

    public void AddGivenPoint(Point point)
    {
        GivenPoints.Add(point);
    }

    public static Teacher Create(
        string name,
        string fatherName,
        string motherName,
        string nationalityNumber,
        string? email,
        string userName,
        string password
        )
    {
        var teacher = new Teacher
        {
            Name = name,
            FatherName = fatherName,
            MotherName = motherName,
            NationalityNumber = nationalityNumber,
            Email = email,
            RelatedUser = User.Create(userName, password, Constants.DefaultTeacherRoleId),
        };
        teacher.AddDomainEvent(new TeacherCreatedEvent(teacher.Id, name));
        return teacher;
    }
}
