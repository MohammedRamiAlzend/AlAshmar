using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Repos;
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
            q => q.Include(m => m.ManagerAttachments).ThenInclude(ma => ma.Attachment).ThenInclude(a => a.Extention));

        if (manager.IsError) return manager.Errors;
        if (manager.Value == null)
            return ApplicationErrors.ManagerNotFound;

        var attachmentDtos = manager.Value.ManagerAttachments
            .Select(ma => new ManagerAttachmentDto(
                ma.ManagerId,
                ma.AttachmentId,
                null,
                ma.Attachment != null ? new AttacmentDto(
                    ma.Attachment.Id,
                    ma.Attachment.Path,
                    ma.Attachment.Type,
                    ma.Attachment.SafeName,
                    ma.Attachment.OriginalName,
                    ma.Attachment.ExtentionId,
                    ma.Attachment.Extention != null ? new AllowableExtentionDto(ma.Attachment.Extention.Id, ma.Attachment.Extention.ExtName) : null) : null)).ToList();

        return attachmentDtos;
    }
}
