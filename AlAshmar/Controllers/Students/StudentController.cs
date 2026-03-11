using AlAshmar.Application.UseCases.Students.UpdateStudentPassword;

namespace AlAshmar.Controllers.Students;

/// <summary>
/// Controller for core student CRUD operations.
/// </summary>
[ApiController]
[Route("api/students")]
[Authorize]
public class StudentController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IFilesManagerService _filesManager;

    public StudentController(ISender sender, IFilesManagerService filesManager)
    {
        _sender = sender;
        _filesManager = filesManager;
    }

    /// <summary>
    /// Get all students filtered by various criteria with support for OR operations.
    /// All filter parameters are optional - null values are ignored in filtering.
    /// </summary>
    [HttpGet("filtered")]
    public async Task<IActionResult> GetAllStudentFiltered(
        [FromQuery] int? pageNumber = null,
        [FromQuery] int? pageSize = null,
        [FromQuery] Guid? classId = null,
        [FromQuery] Guid? semesterId = null,
        [FromQuery] Guid? eventId = null,
        [FromQuery] Guid? teacherId = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAllStudentsFilteredQuery(pageNumber, pageSize, classId, semesterId, eventId, teacherId);
        var result = await _sender.Send(query, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Get a student by ID with full details including contact info, attachments, and academic records.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetStudentById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetStudentByIdQuery(id);
        var result = await _sender.Send(query, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Create a new student. The default password is set to the student's NationalityNumber.
    /// </summary>
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateStudent(
        [FromForm] CreateStudentDto dto,
        [FromForm] List<IFormFile>? photos,
        CancellationToken cancellationToken = default)
    {
        var userName = dto.Email ?? dto.NationalityNumber;
        var password = dto.NationalityNumber;

        var command = new CreateStudentCommand(
            dto.Name, dto.FatherName, dto.MotherName,
            dto.NationalityNumber, dto.Email,
            userName, password
        );
        var result = await _sender.Send(command, cancellationToken);
        if (result.IsError)
            return result.ToActionResult();

        var studentId = result.Value.Id;

        if (photos != null && photos.Count > 0)
        {
            var photoErrors = new List<string>();

            foreach (var photo in photos)
            {
                if (photo == null || photo.Length == 0)
                    continue;

                var saveResult = await _filesManager.SaveFileAsync(photo, $"students/{studentId}");
                if (saveResult.IsError)
                {
                    photoErrors.Add($"Failed to save photo '{photo.FileName}': {saveResult.TopError.Description}");
                    continue;
                }

                var metadata = saveResult.Value;
                var attachmentCommand = new AddAttachmentCommand(
                    studentId,
                    metadata.FilePath,
                    metadata.ContentType,
                    metadata.StoredFileName,
                    metadata.OriginalFileName,
                    null
                );
                var attachResult = await _sender.Send(attachmentCommand, cancellationToken);
                if (attachResult.IsError)
                    photoErrors.Add($"Failed to register photo '{photo.FileName}': {attachResult.TopError.Description}");
            }

            if (photoErrors.Count > 0)
                return Ok(new { data = result.Value, photoUploadWarnings = photoErrors });
        }

        return result.ToActionResult();
    }

    /// <summary>
    /// Update an existing student's basic information.
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateStudent(
        [FromRoute] Guid id,
        [FromBody] UpdateStudentDto dto,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateStudentCommand(
            id, dto.Name, dto.FatherName, dto.MotherName,
            dto.NationalityNumber, dto.Email
        );
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Delete a student by ID.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteStudent(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteStudentCommand(id);
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Update a student's password.
    /// </summary>
    [HttpPut("{id:guid}/password")]
    public async Task<IActionResult> UpdatePassword(
        [FromRoute] Guid id,
        [FromBody] UpdatePasswordDto dto,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateStudentPasswordCommand(id, dto.NewPassword);
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }
}
