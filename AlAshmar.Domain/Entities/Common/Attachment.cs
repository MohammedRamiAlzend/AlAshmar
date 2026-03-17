using System.ComponentModel.DataAnnotations.Schema;

namespace AlAshmar.Domain.Entities.Common;

public class Attachment : Entity<Guid>
{
    public string Path { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string SafeName { get; set; } = null!;
    public string OriginalName { get; set; } = null!;

    public Guid? ExtensionId { get; set; }
    [ForeignKey(nameof(ExtensionId))]
    public AllowableExtension? Extension { get; set; }
}
