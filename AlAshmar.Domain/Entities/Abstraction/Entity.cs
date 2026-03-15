namespace AlAshmar.Domain.Entities.Abstraction
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="Entity{TKey}" />
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class Entity<TKey>
    {
        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        [Key]
        public virtual TKey Id { get; set; } = default!;
    }
}
