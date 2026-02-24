using System.ComponentModel.DataAnnotations.Schema;

namespace AlAshmar.Domain.Entities.Academic;

public class Hadith : Entity<Guid>
{
    public string Text { get; set; } = null!;

    public Guid? BookId { get; set; }
    [ForeignKey(nameof(BookId))]
    public Book? Book { get; set; }

    public string? Chapter { get; set; }
}
