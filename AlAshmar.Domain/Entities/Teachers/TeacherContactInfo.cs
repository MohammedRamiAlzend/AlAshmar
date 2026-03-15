using AlAshmar.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

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
