namespace AlAshmar.Application.Repos.Includes;

/// <summary>
/// Defines a contract for centralizing eager-loading (Include) logic for a given entity type.
/// Implement this interface for each entity to define all navigation properties that should
/// be included when querying. Updating the implementation in one place automatically
/// propagates the change to all query methods (GetAllAsync, GetPagedAsync, GetAsync, etc.).
/// </summary>
/// <typeparam name="TEntity">The entity type whose includes are defined.</typeparam>
public interface IEntityIncludes<TEntity>
{
    /// <summary>
    /// Returns a transform function that applies all eager-loading includes for the entity.
    /// Pass the result directly as the <c>transform</c> parameter of any
    /// <see cref="IRepositoryBase{TEntity, TKey}"/> query method.
    /// </summary>
    Func<IQueryable<TEntity>, IQueryable<TEntity>> Apply();
}
