using AlAshmar.Application.UseCases.Dawras.CreateDawra;
using AlAshmar.Application.UseCases.Dawras.DeleteDawra;
using AlAshmar.Application.UseCases.Dawras.GetAllDawras;
using AlAshmar.Application.UseCases.Dawras.GetDawraById;
using AlAshmar.Application.UseCases.Dawras.GetDawrasBySemester;
using AlAshmar.Application.UseCases.Dawras.UpdateDawra;
using AlAshmar.Application.DTOs.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlAshmar.Controllers.Academic;

/// <summary>
/// Controller for Dawra (course/program) CRUD operations.
/// Each Semester has many Dawras; each Dawra has many Halaqas.
/// </summary>
[ApiController]
[Route("api/dawras")]
[Authorize]
public class DawrasController : ControllerBase
{
    private readonly ISender _sender;

    public DawrasController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Get all dawras.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetAllDawrasQuery(), cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Get all dawras in a specific semester.
    /// </summary>
    [HttpGet("by-semester/{semesterId:guid}")]
    public async Task<IActionResult> GetBySemester(
        [FromRoute] Guid semesterId,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetDawrasBySemesterQuery(semesterId), cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Get a dawra by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetDawraByIdQuery(id), cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Create a new dawra.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateDawraDto dto,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new CreateDawraCommand(dto.EventName, dto.SemesterId), cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Update an existing dawra.
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateDawraDto dto,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new UpdateDawraCommand(id, dto.EventName), cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Delete a dawra by ID.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new DeleteDawraCommand(id), cancellationToken);
        return result.ToActionResult();
    }
}
