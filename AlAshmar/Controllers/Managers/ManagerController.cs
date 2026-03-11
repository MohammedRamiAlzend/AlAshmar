namespace AlAshmar.Controllers.Managers;

/// <summary>
/// Controller for core manager CRUD operations.
/// </summary>
[ApiController]
[Route("api/managers")]
[Authorize]
public class ManagerController : ControllerBase
{
    private readonly ISender _sender;

    public ManagerController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Get all managers.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllManagers(CancellationToken cancellationToken = default)
    {
        var query = new GetAllManagersQuery();
        var result = await _sender.Send(query, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Get a manager by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetManagerById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetManagerByIdQuery(id);
        var result = await _sender.Send(query, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Create a new manager.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateManager(
        [FromBody] CreateManagerCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Update an existing manager's information.
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateManager(
        [FromRoute] Guid id,
        [FromBody] UpdateManagerDto dto,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateManagerCommand(id, dto.Name);
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Delete a manager by ID.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteManager(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteManagerCommand(id);
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Update a manager's password.
    /// </summary>
    [HttpPut("{id:guid}/password")]
    public async Task<IActionResult> UpdatePassword(
        [FromRoute] Guid id,
        [FromBody] UpdatePasswordDto dto,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateManagerPasswordCommand(id, dto.NewPassword);
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }
}
