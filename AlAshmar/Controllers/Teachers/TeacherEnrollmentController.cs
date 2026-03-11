namespace AlAshmar.Controllers.Teachers;

/// <summary>
/// Controller for teacher class enrollment operations.
/// </summary>
[ApiController]
[Route("api/teachers")]
[Authorize]
public class TeacherEnrollmentController : ControllerBase
{
    private readonly ISender _sender;

    public TeacherEnrollmentController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Get a teacher's class enrollments.
    /// </summary>
    [HttpGet("{id:guid}/enrollments")]
    public async Task<IActionResult> GetClassEnrollments(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        return Ok(new List<ClassTeacherEnrollmentDto>());
    }
}
