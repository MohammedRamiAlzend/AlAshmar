using AlAshmar.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlAshmar.Domain.Entities.Managers;

public class ManagerContactInfo
{

    public Guid ManagerId { get; set; }
    [ForeignKey(nameof(ManagerId))]
    public Manager? Manager { get; set; }

    public Guid ContactInfoId { get; set; }
    [ForeignKey(nameof(ContactInfoId))]
    public ContactInfo? ContactInfo { get; set; }
}
