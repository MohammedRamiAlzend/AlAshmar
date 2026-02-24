using System.ComponentModel.DataAnnotations.Schema;
using AlAshmar.Domain.Entities.Common;

namespace AlAshmar.Domain.Entities.Managers;

public class ManagerAttachment
{
    // Composite key: ManagerId + AttachmentId in OnModelCreating
    public Guid ManagerId { get; set; }
    [ForeignKey(nameof(ManagerId))]
    public Manager? Manager { get; set; }

    public Guid AttachmentId { get; set; }
    [ForeignKey(nameof(AttachmentId))]
    public Attacment? Attachment { get; set; }
}
