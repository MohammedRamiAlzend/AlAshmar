namespace AlAshmar.Controllers.Teachers;




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




    [HttpGet("{id:guid}/attendance")]
    public async Task<IActionResult> GetAttendanceRecords(
        [FromRoute] Guid id,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken cancellationToken = default)
    {
        return Ok(new List<TeacherAttencanceDto>());
    }




    [HttpGet("{id:guid}/points-given")]
    public async Task<IActionResult> GetPointsGiven(
        [FromRoute] Guid id,
        [FromQuery] Guid? semesterId = null,
        CancellationToken cancellationToken = default)
    {
        return Ok(new List<PointDto>());
    }
}
