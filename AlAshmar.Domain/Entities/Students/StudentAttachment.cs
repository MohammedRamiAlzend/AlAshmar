using AlAshmar.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

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
