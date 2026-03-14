using System.ComponentModel.DataAnnotations.Schema;
using AlAshmar.Domain.Entities.Common;

namespace AlAshmar.Domain.Entities.Teachers;

public class TeacherContactInfo
{

    public Guid TeacherId { get; set; }
    [ForeignKey(nameof(TeacherId))]
    public Teacher? Teacher { get; set; }

    public Guid ContactInfoId { get; set; }
    [ForeignKey(nameof(ContactInfoId))]
    public ContactInfo? ContactInfo { get; set; }
}
