using AlAshmar.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlAshmar.Domain.Entities.Teachers;

public class TeacherAttachment
{

    public Guid TeacherId { get; set; }
    [ForeignKey(nameof(TeacherId))]
    public Teacher? Teacher { get; set; }

    public Guid AttachmentId { get; set; }
    [ForeignKey(nameof(AttachmentId))]
    public Attachment? Attachment { get; set; }
}
