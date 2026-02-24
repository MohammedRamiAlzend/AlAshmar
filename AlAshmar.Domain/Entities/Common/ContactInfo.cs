namespace AlAshmar.Domain.Entities.Common;

public class ContactInfo : Entity<Guid>
{
    public string Number { get; set; } = null!;
    public string? Email { get; set; }
    public bool IsActive { get; set; }
}
