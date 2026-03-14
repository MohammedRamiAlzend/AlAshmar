
namespace AlAshmar.Application.UseCases.Students.AddAttachment;




public record AddAttachmentCommand(
    Guid StudentId,
    string Path,
    string Type,
    string SafeName,
    string OriginalName,
    Guid? ExtensionId
) : ICommand<Result<Success>>;
