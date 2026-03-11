namespace AlAshmar.Controllers.Teachers;

/// <summary>
/// Controller for teacher academic operations (attendance, points given).
/// </summary>
[ApiController]
[Route("api/teachers")]
[Authorize]
public class TeacherAcademicController : ControllerBase
{
    private readonly ISender _sender;

    public TeacherAcademicController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Get a teacher's attendance records.
    /// </summary>
    [HttpGet("{id:guid}/attendance")]
    public async Task<IActionResult> GetAttendanceRecords(
        [FromRoute] Guid id,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken cancellationToken = default)
    {
        return Ok(new List<TeacherAttencanceDto>());
    }

    /// <summary>
    /// Get points given by a teacher.
    /// </summary>
    [HttpGet("{id:guid}/points-given")]
    public async Task<IActionResult> GetPointsGiven(
        [FromRoute] Guid id,
        [FromQuery] Guid? semesterId = null,
        CancellationToken cancellationToken = default)
    {
        return Ok(new List<PointDto>());
    }
}
