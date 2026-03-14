
namespace AlAshmar.Application.Services.Crud;




public interface ICrudService<TEntity, TDto, TKey>
    where TEntity : Entity<TKey>
    where TDto : class
{
    Task<Result<TDto>> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
    Task<Result<List<TDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<PagedList<TDto>>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<Result<TDto>> CreateAsync(TDto dto, CancellationToken cancellationToken = default);
    Task<Result<TDto>> UpdateAsync(TKey id, TDto dto, CancellationToken cancellationToken = default);
    Task<Result<Success>> DeleteAsync(TKey id, CancellationToken cancellationToken = default);
}




public interface IAdvancedCrudService<TEntity, TDto, TKey> : ICrudService<TEntity, TDto, TKey>
    where TEntity : Entity<TKey>
    where TDto : class
{
    Task<Result<List<TDto>>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task<Result<bool>> ExistsAsync(TKey id, CancellationToken cancellationToken = default);
}
