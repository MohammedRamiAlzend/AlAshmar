using AlAshmar.Application.Common;
using AlAshmar.Domain.Commons;

namespace AlAshmar.Application.UseCases.Students.AddAttachment;

/// <summary>
/// Command to add an attachment to a student's profile.
/// </summary>
public record AddAttachmentCommand(
    Guid StudentId,
    string Path,
    string Type,
    string SafeName,
    string OriginalName,
    Guid? ExtensionId
) : ICommand<Result<Success>>;
