using AlAshmar.Application.Common;
using AlAshmar.Domain.Commons;
using AlAshmar.Domain.Entities.Students;
using AlAshmar.Domain.Entities.Common;
using AlAshmar.Application.Repos;
using MediatR;

namespace AlAshmar.Application.UseCases.Students.AddAttachment;

public class AddAttachmentHandler(
    IRepositoryBase<Student, Guid> repository,
    IRepositoryBase<Attacment, Guid> attachmentRepository)
    : IRequestHandler<AddAttachmentCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(AddAttachmentCommand command, CancellationToken cancellationToken = default)
    {
        var student = await repository.GetByIdAsync(command.StudentId);
        if (student.Value == null)
            return new Error("404", "Student not found", ErrorKind.NotFound);

        var attachment = new Attacment
        {
            Path = command.Path,
            Type = command.Type,
            SafeName = command.SafeName,
            OriginalName = command.OriginalName,
            ExtentionId = command.ExtensionId
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
