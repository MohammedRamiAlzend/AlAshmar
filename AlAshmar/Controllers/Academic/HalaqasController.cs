using AlAshmar.Application.UseCases.Halaqas.CreateHalaqa;
using AlAshmar.Application.UseCases.Halaqas.DeleteHalaqa;
using AlAshmar.Application.UseCases.Halaqas.GetAllHalaqas;
using AlAshmar.Application.UseCases.Halaqas.GetHalaqaById;
using AlAshmar.Application.UseCases.Halaqas.GetHalaqasByDawra;
using AlAshmar.Application.UseCases.Halaqas.UpdateHalaqa;
using AlAshmar.Application.DTOs.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlAshmar.Controllers.Academic;

/// <summary>
/// Controller for Halaqa (learning circle/class) CRUD operations.
/// Each Dawra (course) has many Halaqas; each Halaqa has many students and teachers.
/// </summary>
[ApiController]
[Route("api/halaqas")]
[Authorize]
public class HalaqasController : ControllerBase
{
    private readonly ISender _sender;

    public HalaqasController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Get all halaqas.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetAllHalaqasQuery(), cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Get all halaqas in a specific dawra (course).
    /// </summary>
    [HttpGet("by-dawra/{dawraId:guid}")]
    public async Task<IActionResult> GetByDawra(
        [FromRoute] Guid dawraId,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetHalaqasByDawraQuery(dawraId), cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Get a halaqa by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetHalaqaByIdQuery(id), cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Create a new halaqa.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateHalaqaDto dto,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new CreateHalaqaCommand(dto.ClassName, dto.DawraId), cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Update an existing halaqa.
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateHalaqaDto dto,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new UpdateHalaqaCommand(id, dto.ClassName), cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Delete a halaqa by ID.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new DeleteHalaqaCommand(id), cancellationToken);
        return result.ToActionResult();
    }
}
