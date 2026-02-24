using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Services.Crud;
using AlAshmar.Infrastructure.Authorization.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlAshmar.Controllers;

/// <summary>
/// Base controller with common CRUD operations.
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
    protected readonly IRepositoryBase<TEntity, TKey> _repository;
    protected readonly IMapper _mapper;

    protected BaseController(IRepositoryBase<TEntity, TKey> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public virtual async Task<ActionResult<List<TDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _repository.GetAllAsync();
        return result.IsError ? BadRequest(result.Errors) : Ok(result.Value.Select(_mapper.Map<TDto>).ToList());
    }

    [HttpGet("{id}")]
    public virtual async Task<ActionResult<TDto>> GetById(TKey id, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByIdAsync(id);
        if (result.Value == null)
            return NotFound();
        return Ok(_mapper.Map<TDto>(result.Value));
    }

    [HttpPost]
    public virtual async Task<ActionResult<TDto>> Create([FromBody] TCreateDto dto, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<TEntity>(dto);
        var result = await _repository.AddAsync(entity);
        return result.IsError ? BadRequest(result.Errors) : CreatedAtAction(nameof(GetById), new { id = GetId(_mapper.Map<TDto>(entity)) }, _mapper.Map<TDto>(entity));
    }

    [HttpPut("{id}")]
    public virtual async Task<ActionResult<TDto>> Update(TKey id, [FromBody] TUpdateDto dto, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing.Value == null)
            return NotFound();
        
        _mapper.Map(dto, existing.Value);
        var result = await _repository.UpdateAsync(existing.Value);
        return result.IsError ? BadRequest(result.Errors) : Ok(_mapper.Map<TDto>(existing.Value));
    }

    [HttpDelete("{id}")]
    public virtual async Task<ActionResult> Delete(TKey id, CancellationToken cancellationToken)
    {
        var result = await _repository.RemoveAsync(e => e.Id!.Equals(id));
        return result.IsError
            ? (result.Errors.First().Type == ErrorKind.NotFound ? NotFound(result.Errors) : BadRequest(result.Errors))
            : NoContent();
    }

    protected abstract TKey GetId(TDto dto);
}
