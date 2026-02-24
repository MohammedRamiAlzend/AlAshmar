using AlAshmar.Application.Repos;
using AlAshmar.Domain.Commons;

namespace AlAshmar.Application.Services.Crud;

/// <summary>
/// Base implementation of CRUD service with common operations.
/// </summary>
public abstract class CrudServiceBase<TEntity, TDto, TKey> : IAdvancedCrudService<TEntity, TDto, TKey>
    where TEntity : Entity<TKey>
    where TDto : class
{
    protected readonly IRepositoryBase<TEntity, TKey> _repository;
    protected readonly IMapper _mapper;

    protected CrudServiceBase(IRepositoryBase<TEntity, TKey> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public virtual async Task<Result<TDto>> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity.Value == null)
            return new Error("404", $"{typeof(TEntity).Name} not found", ErrorKind.NotFound);

        return _mapper.Map<TDto>(entity.Value);
    }

    public virtual async Task<Result<List<TDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAllAsync();
        if (result.IsError)
            return result.Errors;

        return result.Value.Select(_mapper.Map<TDto>).ToList();
    }

    public virtual async Task<Result<PagedList<TDto>>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetPagedAsync(page, pageSize);
        if (result.IsError)
            return result.Errors;

        var items = result.Value.Items.Select(_mapper.Map<TDto>).ToList();
        return PagedList<TDto>.Create(items, result.Value.TotalItems, page, pageSize);
    }

    public virtual async Task<Result<TDto>> CreateAsync(TDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<TEntity>(dto);
        var result = await _repository.AddAsync(entity);
        if (result.IsError)
            return result.Errors;

        return _mapper.Map<TDto>(entity);
    }

    public virtual async Task<Result<TDto>> UpdateAsync(TKey id, TDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing.Value == null)
            return new Error("404", $"{typeof(TEntity).Name} not found", ErrorKind.NotFound);

        _mapper.Map(dto, existing.Value);
        var result = await _repository.UpdateAsync(existing.Value);
        if (result.IsError)
            return result.Errors;

        return _mapper.Map<TDto>(existing.Value);
    }

    public virtual async Task<Result<Success>> DeleteAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing.Value == null)
            return new Error("404", $"{typeof(TEntity).Name} not found", ErrorKind.NotFound);

        var result = await _repository.RemoveAsync(e => e.Id!.Equals(id));
        if (result.IsError)
            return result.Errors;
        
        return Result.Success;
    }

    public virtual async Task<Result<List<TDto>>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetAllAsync(predicate);
        if (result.IsError)
            return result.Errors;

        return result.Value.Select(_mapper.Map<TDto>).ToList();
    }

    public virtual async Task<Result<bool>> ExistsAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await _repository.AnyAsync(e => e.Id!.Equals(id));
    }
}
