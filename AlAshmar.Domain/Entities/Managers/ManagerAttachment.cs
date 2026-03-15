using AlAshmar.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlAshmar.Domain.Entities.Managers;

public class ManagerAttachment
{

    public Guid ManagerId { get; set; }
    [ForeignKey(nameof(ManagerId))]
    public Manager? Manager { get; set; }

    public Guid AttachmentId { get; set; }
    [ForeignKey(nameof(AttachmentId))]
    public Attacment? Attachment { get; set; }
}
