using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Services.Crud;
using AlAshmar.Domain.Commons;
using AlAshmar.Infrastructure.Authorization.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlAshmar.Controllers;

/// <summary>
/// Base controller with common CRUD operations.
/// Depends on the <see cref="IAdvancedCrudService{TEntity,TDto,TKey}"/> service abstraction.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public abstract class BaseController<TEntity, TDto, TCreateDto, TUpdateDto, TKey> : ControllerBase
    where TEntity : Entity<TKey>
    where TDto : class
    where TCreateDto : class
    where TUpdateDto : class
    where TKey : struct
{
    protected readonly IAdvancedCrudService<TEntity, TDto, TKey> _crudService;
    protected readonly IMapper _mapper;

    protected BaseController(IAdvancedCrudService<TEntity, TDto, TKey> crudService, IMapper mapper)
    {
        _crudService = crudService;
        _mapper = mapper;
    }

    [HttpGet]
    public virtual async Task<ActionResult<List<TDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _crudService.GetAllAsync(cancellationToken);
        return result.IsError ? BadRequest(result.Errors) : Ok(result.Value);
    }

    [HttpGet("paged")]
    public virtual async Task<ActionResult<PagedList<TDto>>> GetPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _crudService.GetPagedAsync(page, pageSize, cancellationToken);
        return result.IsError ? BadRequest(result.Errors) : Ok(result.Value);
    }

    [HttpGet("{id}")]
    public virtual async Task<ActionResult<TDto>> GetById(TKey id, CancellationToken cancellationToken)
    {
        var result = await _crudService.GetByIdAsync(id, cancellationToken);
        if (result.IsError)
            return result.TopError.Type == ErrorKind.NotFound ? NotFound(result.Errors) : BadRequest(result.Errors);
        return Ok(result.Value);
    }

    [HttpPost]
    public virtual async Task<ActionResult<TDto>> Create([FromBody] TCreateDto dto, CancellationToken cancellationToken)
    {
        var mapped = _mapper.Map<TDto>(dto);
        var result = await _crudService.CreateAsync(mapped, cancellationToken);
        return result.IsError ? BadRequest(result.Errors) : CreatedAtAction(nameof(GetById), new { id = GetId(result.Value) }, result.Value);
    }

    [HttpPut("{id}")]
    public virtual async Task<ActionResult<TDto>> Update(TKey id, [FromBody] TUpdateDto dto, CancellationToken cancellationToken)
    {
        var mapped = _mapper.Map<TDto>(dto);
        var result = await _crudService.UpdateAsync(id, mapped, cancellationToken);
        if (result.IsError)
            return result.TopError.Type == ErrorKind.NotFound ? NotFound(result.Errors) : BadRequest(result.Errors);
        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    public virtual async Task<ActionResult> Delete(TKey id, CancellationToken cancellationToken)
    {
        var result = await _crudService.DeleteAsync(id, cancellationToken);
        if (result.IsError)
            return result.TopError.Type == ErrorKind.NotFound ? NotFound(result.Errors) : BadRequest(result.Errors);
        return NoContent();
    }

    protected abstract TKey GetId(TDto dto);
}
