namespace AlAshmar.Controllers.Teachers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TeacherManagementController : ControllerBase
{
    private readonly IQueryHandler<GetAllTeachersFilteredQuery, Result<List<TeacherDto>>> _filterHandler;
    private readonly IQueryHandler<GetTeacherByIdQuery, Result<TeacherDto?>> _getByIdHandler;
    private readonly ICommandHandler<CreateTeacherCommand, Result<TeacherDto>> _createHandler;
    private readonly ICommandHandler<UpdateTeacherCommand, Result<TeacherDto>> _updateHandler;
    private readonly ICommandHandler<DeleteTeacherCommand, Result<Success>> _deleteHandler;

    public TeacherManagementController(
        IQueryHandler<GetAllTeachersFilteredQuery, Result<List<TeacherDto>>> filterHandler,
        IQueryHandler<GetTeacherByIdQuery, Result<TeacherDto?>> getByIdHandler,
        ICommandHandler<CreateTeacherCommand, Result<TeacherDto>> createHandler,
        ICommandHandler<UpdateTeacherCommand, Result<TeacherDto>> updateHandler,
        ICommandHandler<DeleteTeacherCommand, Result<Success>> deleteHandler)
    {
        _filterHandler = filterHandler;
        _getByIdHandler = getByIdHandler;
        _createHandler = createHandler;
        _updateHandler = updateHandler;
        _deleteHandler = deleteHandler;
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
        var result = await _filterHandler.Handle(query, cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTeacherById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetTeacherByIdQuery(id);
        var result = await _getByIdHandler.Handle(query, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost]
    public async Task<IActionResult> CreateTeacher(
        [FromBody] CreateTeacherDto dto,
        CancellationToken cancellationToken = default)
    {
        var userName = dto.Email ?? dto.NationalityNumber ?? $"teacher_{Guid.NewGuid():N}".Substring(0, 20);
        var password = GenerateSecurePassword();
        
        var command = new CreateTeacherCommand(
            dto.Name, dto.FatherName, dto.MotherName,
            dto.NationalityNumber, dto.Email,
            userName, password
        );
        var result = await _createHandler.Handle(command, cancellationToken);
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
        var result = await _updateHandler.Handle(command, cancellationToken);
        return result.ToActionResult();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTeacher(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteTeacherCommand(id);
        var result = await _deleteHandler.Handle(command, cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet("{id:guid}/enrollments")]
    public async Task<IActionResult> GetClassEnrollments(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        return Ok(new List<ClassTeacherEnrollmentDto>());
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

    [HttpPost("{id:guid}/attachments")]
    public async Task<IActionResult> AddAttachment(
        [FromRoute] Guid id,
        IFormFile formFile,
        CancellationToken cancellationToken = default)
    {
        if (formFile == null || formFile.Length == 0)
            return BadRequest("No file provided");

        return Ok(new { Message = "Attachment upload not yet implemented" });
    }

    [HttpGet("{id:guid}/attachments")]
    public async Task<IActionResult> GetAttachments(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        return Ok(new List<TeacherAttachmentDto>());
    }

    private static string GenerateSecurePassword(int length = 12)
    {
        const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";
        var randomBytes = new byte[length];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        
        var password = new char[length];
        for (int i = 0; i < length; i++)
        {
            password[i] = validChars[randomBytes[i] % validChars.Length];
        }
        
        return new string(password);
    }
}
