using System.ComponentModel.DataAnnotations.Schema;
using AlAshmar.Domain.Entities.Common;

namespace AlAshmar.Domain.Entities.Students;

public class StudentAttachment
{

    public Guid StudentId { get; set; }
    [ForeignKey(nameof(StudentId))]
    public Student? Student { get; set; }

    public Guid AttachmentId { get; set; }
    [ForeignKey(nameof(AttachmentId))]
    public Attacment? Attachment { get; set; }
}
