using System.ComponentModel.DataAnnotations.Schema;
using AlAshmar.Domain.Entities.Common;

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
