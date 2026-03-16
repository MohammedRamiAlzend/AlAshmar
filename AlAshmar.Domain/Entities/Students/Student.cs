using AlAshmar.Domain.Entities.Academic;
using AlAshmar.Domain.Entities.Students.Events;
using AlAshmar.Domain.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlAshmar.Domain.Entities.Students;

public class Student : EntityWithEvents<Guid>
{
    public string Name { get; private set; } = null!;
    public string FatherName { get; private set; } = null!;
    public string MotherName { get; private set; } = null!;
    public string NationalityNumber { get; private set; } = null!;
    public string? Email { get; private set; }

    public Guid? UserId { get; private set; }
    [ForeignKey(nameof(UserId))]
    public User? User { get; private set; }

    public ICollection<StudentContactInfo> StudentContactInfos { get; private set; } = new List<StudentContactInfo>();
    public ICollection<StudentAttachment> StudentAttachments { get; private set; } = new List<StudentAttachment>();
    public ICollection<StudentHadith> StudentHadiths { get; private set; } = new List<StudentHadith>();
    public ICollection<StudentQuraanPage> StudentQuraanPages { get; private set; } = new List<StudentQuraanPage>();
    public ICollection<StudentClassEventsPoint> StudentClassEventsPoints { get; private set; } = new List<StudentClassEventsPoint>();
    public ICollection<Point> Points { get; private set; } = new List<Point>();

    public void UpdateBasicInfo(string name, string fatherName, string motherName, string nationalityNumber, string? email)
    {
        Name = name;
        FatherName = fatherName;
        MotherName = motherName;
        NationalityNumber = nationalityNumber;
        Email = email;
        AddDomainEvent(new StudentUpdatedEvent(Id));
    }

    public void AddPoint(Point point)
    {
        Points.Add(point);
    }

    public static Student Create(string name,
                                 string fatherName,
                                 string motherName,
                                 string nationalityNumber,
                                 string? email,
                                 string userName,
                                 string password)
    {
        var student = new Student
        {
            Name = name,
            FatherName = fatherName,
            MotherName = motherName,
            NationalityNumber = nationalityNumber,
            Email = email,
            User = User.Create(userName, password, Constants.DefaultStudentRoleId)
        };
        student.AddDomainEvent(new StudentCreatedEvent(student.Id, name));
        return student;
    }
}
