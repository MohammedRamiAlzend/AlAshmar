using System.ComponentModel.DataAnnotations;

namespace AlAshmar.Domain.Entities.Abstraction
{
    public class Entity<TKey>
    {
        [Key]
        public virtual TKey Id { get; set; } = default!;
    }
}
