namespace AlAshmar.Controllers.Students;

/// <summary>
/// Controller for student management operations including filtering, enrollment, and academic tracking.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StudentManagementController : ControllerBase
{
    private readonly ISender _sender;

    public StudentManagementController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Get all students filtered by various criteria with support for OR operations.
    /// All filter parameters are optional - null values are ignored in filtering.
    /// </summary>
    /// <param name="pageNumber">Page number for pagination (1-based)</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="classId">Filter by class ID (nullable - supports OR operation)</param>
    /// <param name="semesterId">Filter by semester ID (nullable - supports OR operation)</param>
    /// <param name="eventId">Filter by event ID (nullable - supports OR operation)</param>
    /// <param name="teacherId">Filter by teacher ID (nullable - supports OR operation)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of filtered students with their related data</returns>
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
    /// <param name="id">Student ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
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
    /// Create a new student with basic info and optional contact details.
    /// </summary>
    /// <param name="dto">Student creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpPost]
    public async Task<IActionResult> CreateStudent(
        [FromBody] CreateStudentDto dto,
        CancellationToken cancellationToken = default)
    {
        // Generate username from email or nationality number
        var userName = dto.Email ?? dto.NationalityNumber ?? $"student_{Guid.NewGuid():N}".Substring(0, 20);
        // Generate a secure random password
        var password = GenerateSecurePassword();

        var command = new CreateStudentCommand(
            dto.Name, dto.FatherName, dto.MotherName,
            dto.NationalityNumber, dto.Email,
            userName, password
        );
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Generates a secure random password with 12 characters.
    /// </summary>
    private static string GenerateSecurePassword(int length = 12)
    {
        const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";
        var randomBytes = new byte[length];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        
        var password = new char[length];
        for (int i = 0; i < length; i++)
        {
            password[i] = validChars[randomBytes[i] % validChars.Length];
        }
        
        return new string(password);
    }

    /// <summary>
    /// Update an existing student's basic information.
    /// </summary>
    /// <param name="id">Student ID</param>
    /// <param name="dto">Updated student data</param>
    /// <param name="cancellationToken">Cancellation token</param>
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
    /// <param name="id">Student ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
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
    /// Get student's memorization progress (Hadith and Quran).
    /// </summary>
    /// <param name="id">Student ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("{id:guid}/memorization")]
    public async Task<IActionResult> GetMemorizationProgress(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetMemorizationProgressQuery(id);
        var result = await _sender.Send(query, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Get student's attendance records.
    /// </summary>
    /// <param name="id">Student ID</param>
    /// <param name="fromDate">Optional from date</param>
    /// <param name="toDate">Optional to date</param>
    /// <param name="cancellationToken">Cancellation token</param>
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

    /// <summary>
    /// Get student's points and achievements.
    /// </summary>
    /// <param name="id">Student ID</param>
    /// <param name="semesterId">Optional semester filter</param>
    /// <param name="cancellationToken">Cancellation token</param>
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

    /// <summary>
    /// Enroll a student in a class.
    /// </summary>
    /// <param name="id">Student ID</param>
    /// <param name="classId">Class ID to enroll in</param>
    /// <param name="cancellationToken">Cancellation token</param>
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
    /// <param name="id">Student ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("{id:guid}/enrollments")]
    public async Task<IActionResult> GetClassEnrollments(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetClassEnrollmentsQuery(id);
        var result = await _sender.Send(query, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Add attachment to a student's profile.
    /// </summary>
    /// <param name="id">Student ID</param>
    /// <param name="formFile">Attachment file</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpPost("{id:guid}/attachments")]
    public async Task<IActionResult> AddAttachment(
        [FromRoute] Guid id,
        IFormFile formFile,
        CancellationToken cancellationToken = default)
    {
        if (formFile == null || formFile.Length == 0)
            return BadRequest("No file provided");

        // Generate safe file name and path
        var fileName = $"{Guid.NewGuid():N}_{formFile.FileName}";
        var path = Path.Combine("uploads", "students", id.ToString(), fileName);

        var command = new AddAttachmentCommand(
            id,
            path,
            formFile.ContentType,
            fileName,
            formFile.FileName,
            null // ExtensionId can be added based on file type
        );
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Get student's attachments.
    /// </summary>
    /// <param name="id">Student ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("{id:guid}/attachments")]
    public async Task<IActionResult> GetAttachments(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAttachmentsQuery(id);
        var result = await _sender.Send(query, cancellationToken);
        return result.ToActionResult();
    }
}
