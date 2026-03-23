using AlAshmar.Domain.DTOs.Domain;

namespace AlAshmar.Controllers.Teachers;

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

    [HttpGet("{id:guid}/enrollments")]
    public async Task<IActionResult> GetClassEnrollments(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        return Ok(new List<ClassTeacherEnrollmentDto>());
    }
}
