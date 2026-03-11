namespace AlAshmar.Controllers.Students;

/// <summary>
/// Controller for student attachment operations.
/// </summary>
[ApiController]
[Route("api/students")]
[Authorize]
public class StudentAttachmentController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IFilesManagerService _filesManager;

    public StudentAttachmentController(ISender sender, IFilesManagerService filesManager)
    {
        _sender = sender;
        _filesManager = filesManager;
    }

    /// <summary>
    /// Add attachment to a student's profile.
    /// </summary>
    [HttpPost("{id:guid}/attachments")]
    public async Task<IActionResult> AddAttachment(
        [FromRoute] Guid id,
        IFormFile formFile,
        CancellationToken cancellationToken = default)
    {
        if (formFile == null || formFile.Length == 0)
            return BadRequest("No file provided");

        var saveResult = await _filesManager.SaveFileAsync(formFile, $"students/{id}");
        if (saveResult.IsError)
            return BadRequest(new { errors = saveResult.Errors });

        var metadata = saveResult.Value;
        var command = new AddAttachmentCommand(
            id,
            metadata.FilePath,
            metadata.ContentType,
            metadata.StoredFileName,
            metadata.OriginalFileName,
            null
        );
        var result = await _sender.Send(command, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    /// Get student's attachments.
    /// </summary>
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
