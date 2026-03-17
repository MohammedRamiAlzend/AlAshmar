using AlAshmar.Application.Repos;
using AlAshmar.Domain.Entities.Common;
using AlAshmar.Domain.Entities.Teachers;

namespace AlAshmar.Application.UseCases.Teachers.AddAttachment;

public record AddTeacherAttachmentCommand(
    Guid TeacherId,
    string Path,
    string Type,
    string SafeName,
    string OriginalName,
    Guid? ExtensionId
) : IRequest<Result<Success>>;

public class AddTeacherAttachmentHandler(
    IRepositoryBase<Teacher, Guid> repository,
    IRepositoryBase<Attachment, Guid> attachmentRepository)
    : IRequestHandler<AddTeacherAttachmentCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(AddTeacherAttachmentCommand command, CancellationToken cancellationToken = default)
    {
        var teacher = await repository.GetByIdAsync(command.TeacherId);
        if (teacher.Value == null)
            return ApplicationErrors.TeacherNotFound;

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

        var teacherAttachment = new TeacherAttachment
        {
            TeacherId = command.TeacherId,
            AttachmentId = attachment.Id
        };

        teacher.Value.TeacherAttachments.Add(teacherAttachment);

        var updateResult = await repository.UpdateAsync(teacher.Value);
        if (updateResult.IsError)
            return updateResult.Errors;

        return Result.Success;
    }
}
