namespace AlAshmar.Controllers.Teachers;




[ApiController]
[Route("api/teachers")]
[Authorize]
public class TeacherController : ControllerBase
{
    private readonly ISender _sender;

    public TeacherController(ISender sender)
    {
        _sender = sender;
    }





    [HttpGet("filtered")]
    public async Task<IActionResult> GetAllTeachersFiltered(
        [FromQuery] int? pageNumber = null,
        [FromQuery] int? pageSize = null,
        [FromQuery] Guid? classId = null,
        [FromQuery] Guid? semesterId = null,
        [FromQuery] Guid? eventId = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAllTeachersFilteredQuery(pageNumber, pageSize, classId, semesterId, eventId);
        var result = await _sender.Send(query, cancellationToken);
        return result.ToActionResult();
    }




    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTeacherById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetTeacherByIdQuery(id);
        var result = await _sender.Send(query, cancellationToken);
        return result.ToActionResult();
    }




    [HttpPost]
    public async Task<IActionResult> CreateTeacher(
        [FromBody] CreateTeacherDto dto,
        CancellationToken cancellationToken = default)
    {
        var userName = dto.Email ?? dto.NationalityNumber;
        var password = dto.NationalityNumber;

        var command = new CreateTeacherCommand(
            dto.Name, dto.FatherName, dto.MotherName,
            dto.NationalityNumber, dto.Email,
            userName, password
        );
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }




    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateTeacher(
        [FromRoute] Guid id,
        [FromBody] UpdateTeacherDto dto,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateTeacherCommand(
            id, dto.Name, dto.FatherName, dto.MotherName,
            dto.NationalityNumber, dto.Email
        );
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }




    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTeacher(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteTeacherCommand(id);
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }




    [HttpPut("{id:guid}/password")]
    public async Task<IActionResult> UpdatePassword(
        [FromRoute] Guid id,
        [FromBody] UpdatePasswordDto dto,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateTeacherPasswordCommand(id, dto.NewPassword);
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }
}
