using System.ComponentModel.DataAnnotations.Schema;

namespace AlAshmar.Domain.Entities.Common;

public class Attacment : Entity<Guid>
{
    public string Path { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string SafeName { get; set; } = null!;
    public string OriginalName { get; set; } = null!;

    public Guid? ExtentionId { get; set; }
    [ForeignKey(nameof(ExtentionId))]
    public AllowableExtention? Extention { get; set; }
}
