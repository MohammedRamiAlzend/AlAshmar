namespace AlAshmar.Controllers.Students;




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
