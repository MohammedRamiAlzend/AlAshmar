using System.ComponentModel.DataAnnotations.Schema;
using AlAshmar.Domain.Entities.Common;

namespace AlAshmar.Domain.Entities.Teachers;

public class TeacherAttachment
{
    // Composite key: TeacherId + AttachmentId in OnModelCreating
    public Guid TeacherId { get; set; }
    [ForeignKey(nameof(TeacherId))]
    public Teacher? Teacher { get; set; }

    public Guid AttachmentId { get; set; }
    [ForeignKey(nameof(AttachmentId))]
    public Attacment? Attachment { get; set; }
}
