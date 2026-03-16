using AlAshmar.Application.Repos;
using AlAshmar.Domain.Entities.Common;
using AlAshmar.Domain.Entities.Managers;

namespace AlAshmar.Application.UseCases.Managers.AddAttachment;

public record AddManagerAttachmentCommand(
    Guid ManagerId,
    string Path,
    string Type,
    string SafeName,
    string OriginalName,
    Guid? ExtensionId
) : IRequest<Result<Success>>;

public class AddManagerAttachmentHandler(
    IRepositoryBase<Manager, Guid> repository,
    IRepositoryBase<Attachment, Guid> attachmentRepository)
    : IRequestHandler<AddManagerAttachmentCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(AddManagerAttachmentCommand command, CancellationToken cancellationToken = default)
    {
        var manager = await repository.GetByIdAsync(command.ManagerId);
        if (manager.Value == null)
            return ApplicationErrors.ManagerNotFound;

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

        var managerAttachment = new ManagerAttachment
        {
            ManagerId = command.ManagerId,
            AttachmentId = attachment.Id
        };

        manager.Value.ManagerAttachments.Add(managerAttachment);

        var updateResult = await repository.UpdateAsync(manager.Value);
        if (updateResult.IsError)
            return updateResult.Errors;

        return Result.Success;
    }
}
