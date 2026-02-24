using AlAshmar.Application.Repos;
using AlAshmar.Domain.Entities.Abstraction;

namespace AlAshmar.Infrastructure.Persistence.UnitOfWork;

/// <summary>
/// Unit of Work interface for coordinating multiple repositories.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Gets the repository for the specified entity type.
    /// </summary>
    IRepositoryBase<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : Entity<TKey>;

    /// <summary>
    /// Saves all changes made to the database.
    /// </summary>
    Task<int> SaveChangesAsync();

    /// <summary>
    /// Begins a new database transaction.
    /// </summary>
    Task BeginTransactionAsync();

    /// <summary>
    /// Commits the current transaction.
    /// </summary>
    Task CommitTransactionAsync();

    /// <summary>
    /// Rolls back the current transaction.
    /// </summary>
    Task RollbackTransactionAsync();
}
