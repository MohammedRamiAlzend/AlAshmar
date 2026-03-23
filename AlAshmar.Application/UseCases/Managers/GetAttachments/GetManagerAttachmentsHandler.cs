using AlAshmar.Application.Repos;
using AlAshmar.Domain.DTOs.Domain;
using AlAshmar.Domain.Entities.Managers;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Application.UseCases.Managers.GetAttachments;

public class GetManagerAttachmentsHandler(IRepositoryBase<Manager, Guid> repository)
    : IRequestHandler<GetManagerAttachmentsQuery, Result<List<ManagerAttachmentDto>>>
{
    public async Task<Result<List<ManagerAttachmentDto>>> Handle(GetManagerAttachmentsQuery query, CancellationToken cancellationToken = default)
    {
        var manager = await repository.GetAsync(
            m => m.Id == query.ManagerId,
            q => q.Include(m => m.ManagerAttachments).ThenInclude(ma => ma.Attachment).ThenInclude(a => a.Extension));

        if (manager.IsError) return manager.Errors;
        if (manager.Value == null)
            return ApplicationErrors.ManagerNotFound;

        var attachmentDtos = manager.Value.ManagerAttachments
            .Select(ma => new ManagerAttachmentDto(
                ma.ManagerId,
                ma.AttachmentId,
                null,
                ma.Attachment != null ? new AttachmentDto(
                    ma.Attachment.Id,
                    ma.Attachment.Path,
                    ma.Attachment.Type,
                    ma.Attachment.SafeName,
                    ma.Attachment.OriginalName,
                    ma.Attachment.ExtensionId,
                    ma.Attachment.Extension != null ? new AllowableExtensionDto(ma.Attachment.Extension.Id, ma.Attachment.Extension.ExtName) : null) : null)).ToList();

        return attachmentDtos;
    }
}
