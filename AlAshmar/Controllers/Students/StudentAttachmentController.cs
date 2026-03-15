namespace AlAshmar.Controllers.Students;

using System.IO.Compression;

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

    [HttpPost("{id:guid}/attachments")]
    public async Task<IActionResult> AddAttachment(
        [FromRoute] Guid id,
        IFormFile formFile,
        CancellationToken cancellationToken = default)
    {
        if (formFile == null || formFile.Length == 0)
            return BadRequest("No file provided");

        var saveResult = await _filesManager.SaveFileAsync(formFile, $"students/{id}");
        if (saveResult.IsError || saveResult.Value == null)
            return BadRequest(new { errors = saveResult.Errors });

        var metadata = saveResult.Value!;
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

    [HttpGet("{id:guid}/attachments")]
    public async Task<IActionResult> GetAttachments(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAttachmentsQuery(id);
        var result = await _sender.Send(query, cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet("{id:guid}/attachments/zip")]
    public async Task<IActionResult> DownloadAttachmentsZip(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAttachmentsQuery(id);
        var result = await _sender.Send(query, cancellationToken);
        if (result.IsError)
            return result.ToActionResult();

        var attachments = (result.Value ?? [])
            .Where(a => a.Attachment != null && !string.IsNullOrWhiteSpace(a.Attachment.Path) && _filesManager.FileExists(a.Attachment.Path))
            .Select(a => a.Attachment!)
            .ToList();

        if (attachments.Count == 0)
            return NotFound("No attachments found");

        using var zipStream = new MemoryStream();
        using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
        {
            foreach (var attachment in attachments)
            {
                var fileBytesResult = await _filesManager.GetFileBytesAsync(attachment.Path);
                if (fileBytesResult.IsError)
                    continue;

                var entryName = Path.GetFileName(attachment.OriginalName);
                if (string.IsNullOrWhiteSpace(entryName))
                    entryName = attachment.SafeName;

                var entry = archive.CreateEntry(entryName);
                await using var entryStream = entry.Open();
                await entryStream.WriteAsync(fileBytesResult.Value, cancellationToken);
            }
        }

        zipStream.Position = 0;
        return File(zipStream.ToArray(), "application/zip", $"student-{id}-attachments.zip");
    }
}
