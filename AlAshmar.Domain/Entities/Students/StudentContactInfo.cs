using System.ComponentModel.DataAnnotations.Schema;
using AlAshmar.Domain.Entities.Common;

namespace AlAshmar.Domain.Entities.Students;

public class StudentContactInfo
{
    public Guid StudentId { get; set; }
    [ForeignKey(nameof(StudentId))]
    public Student? Student { get; set; }

    public Guid ContactInfoId { get; set; }
    [ForeignKey(nameof(ContactInfoId))]
    public ContactInfo? ContactInfo { get; set; }
}
