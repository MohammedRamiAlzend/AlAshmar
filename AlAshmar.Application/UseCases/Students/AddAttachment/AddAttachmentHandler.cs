using AlAshmar.Application.Repos;
using AlAshmar.Domain.Entities.Common;
using AlAshmar.Domain.Entities.Students;

namespace AlAshmar.Application.UseCases.Students.AddAttachment;

public class AddAttachmentHandler(
    IRepositoryBase<Student, Guid> repository,
    IRepositoryBase<Attachment, Guid> attachmentRepository)
    : IRequestHandler<AddAttachmentCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(AddAttachmentCommand command, CancellationToken cancellationToken = default)
    {
        var student = await repository.GetByIdAsync(command.StudentId);
        if (student.Value == null)
            return ApplicationErrors.StudentNotFound;

        var attachment = new Attachment
        {
            Path = command.Path,
            Type = command.Type,
            SafeName = command.SafeName,
            OriginalName = command.OriginalName,
            ExtensionId = command.ExtensionId
        };

        var addResult = await attachmentRepository.AddAsync(attachment);
        if (addResult.IsError)
            return addResult.Errors;

        var studentAttachment = new StudentAttachment
        {
            StudentId = command.StudentId,
            AttachmentId = attachment.Id
        };

        student.Value.StudentAttachments.Add(studentAttachment);

        var updateResult = await repository.UpdateAsync(student.Value);
        if (updateResult.IsError)
            return updateResult.Errors;

        return Result.Success;
    }
}
