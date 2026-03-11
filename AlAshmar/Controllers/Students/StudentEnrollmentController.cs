namespace AlAshmar.Controllers.Students;

/// <summary>
/// Controller for student enrollment operations.
/// </summary>
[ApiController]
[Route("api/students")]
[Authorize]
public class StudentEnrollmentController : ControllerBase
{
    private readonly ISender _sender;

    public StudentEnrollmentController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Enroll a student in a class.
    /// </summary>
    [HttpPost("{id:guid}/enrollments")]
    public async Task<IActionResult> EnrollInClass(
        [FromRoute] Guid id,
        [FromBody] Guid classId,
        CancellationToken cancellationToken = default)
    {
        var command = new EnrollInClassCommand(id, classId);
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Get student's class enrollments.
    /// </summary>
    [HttpGet("{id:guid}/enrollments")]
    public async Task<IActionResult> GetClassEnrollments(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetClassEnrollmentsQuery(id);
        var result = await _sender.Send(query, cancellationToken);
        return result.ToActionResult();
    }
}
