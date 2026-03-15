namespace AlAshmar.Controllers.Students;

[ApiController]
[Route("api/students")]
[Authorize]
public class StudentAcademicController : ControllerBase
{
    private readonly ISender _sender;

    public StudentAcademicController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{id:guid}/memorization")]
    public async Task<IActionResult> GetMemorizationProgress(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetMemorizationProgressQuery(id);
        var result = await _sender.Send(query, cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet("{id:guid}/attendance")]
    public async Task<IActionResult> GetAttendanceRecords(
        [FromRoute] Guid id,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAttendanceRecordsQuery(id, fromDate, toDate);
        var result = await _sender.Send(query, cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet("{id:guid}/points")]
    public async Task<IActionResult> GetPoints(
        [FromRoute] Guid id,
        [FromQuery] Guid? semesterId = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetPointsQuery(id, semesterId);
        var result = await _sender.Send(query, cancellationToken);
        return result.ToActionResult();
    }
}
