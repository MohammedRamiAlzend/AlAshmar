using System.ComponentModel.DataAnnotations.Schema;
using AlAshmar.Domain.Entities.Users;
using AlAshmar.Domain.Entities.Academic;
using AlAshmar.Domain.Entities.Teachers.Events;
using AlAshmar.Domain.Events;

namespace AlAshmar.Domain.Entities.Teachers;

public class Teacher : EntityWithEvents<Guid>
{
    public string Name { get; set; } = null!;
    public string FatherName { get; set; } = null!;
    public string MotherName { get; set; } = null!;
    public string NationalityNumber { get; set; } = null!;
    public string? Email { get; set; }

    public Guid UserId { get; set; }
    public User RelatedUser { get; set; }

    public ICollection<TeacherContactInfo> TeacherContactInfos { get; set; } = new List<TeacherContactInfo>();
    public ICollection<TeacherAttachment> TeacherAttachments { get; set; } = new List<TeacherAttachment>();
    public ICollection<ClassTeacherEnrollment> ClassTeacherEnrollments { get; set; } = new List<ClassTeacherEnrollment>();
    public ICollection<Point> GivenPoints { get; set; } = new List<Point>();

    // Domain methods
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
