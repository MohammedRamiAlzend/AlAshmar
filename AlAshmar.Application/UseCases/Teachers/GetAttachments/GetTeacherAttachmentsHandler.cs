using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Repos;
using AlAshmar.Domain.Entities.Teachers;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Application.UseCases.Teachers.GetAttachments;

public class GetTeacherAttachmentsHandler(IRepositoryBase<Teacher, Guid> repository)
    : IRequestHandler<GetTeacherAttachmentsQuery, Result<List<TeacherAttachmentDto>>>
{
    public async Task<Result<List<TeacherAttachmentDto>>> Handle(GetTeacherAttachmentsQuery query, CancellationToken cancellationToken = default)
    {
        var teacher = await repository.GetAsync(
            t => t.Id == query.TeacherId,
            q => q.Include(t => t.TeacherAttachments).ThenInclude(ta => ta.Attachment).ThenInclude(a => a.Extention));

        if (teacher.IsError) return teacher.Errors;
        if (teacher.Value == null)
            return ApplicationErrors.TeacherNotFound;

        var attachmentDtos = teacher.Value.TeacherAttachments
            .Select(ta => new TeacherAttachmentDto(
                ta.TeacherId,
                ta.AttachmentId,
                null,
                ta.Attachment != null ? new AttacmentDto(
                    ta.Attachment.Id,
                    ta.Attachment.Path,
                    ta.Attachment.Type,
                    ta.Attachment.SafeName,
                    ta.Attachment.OriginalName,
                    ta.Attachment.ExtentionId,
                    ta.Attachment.Extention != null ? new AllowableExtentionDto(ta.Attachment.Extention.Id, ta.Attachment.Extention.ExtName) : null) : null)).ToList();

        return attachmentDtos;
    }
}
