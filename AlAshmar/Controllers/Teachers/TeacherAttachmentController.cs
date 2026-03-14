namespace AlAshmar.Controllers.Teachers;




[ApiController]
[Route("api/teachers")]
[Authorize]
public class TeacherAttachmentController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IFilesManagerService _filesManager;

    public TeacherAttachmentController(ISender sender, IFilesManagerService filesManager)
    {
        _sender = sender;
        _filesManager = filesManager;
    }




    [HttpPost("{id:guid}/attachments")]
    public async Task<IActionResult> AddAttachment(
        [FromRoute] Guid id,
        IFormFile formFile,
        CancellationToken cancellationToken = default)
    {
        if (formFile == null || formFile.Length == 0)
            return BadRequest("No file provided");

        var saveResult = await _filesManager.SaveFileAsync(formFile, $"teachers/{id}");
        if (saveResult.IsError)
            return BadRequest(new { errors = saveResult.Errors });

        var metadata = saveResult.Value;
        var command = new AddTeacherAttachmentCommand(
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




    [HttpGet("{id:guid}/attachments")]
    public async Task<IActionResult> GetAttachments(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        return Ok(new List<TeacherAttachmentDto>());
    }
}
