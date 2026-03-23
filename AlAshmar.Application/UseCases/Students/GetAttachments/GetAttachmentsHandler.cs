using AlAshmar.Application.Repos;
using AlAshmar.Domain.DTOs.Domain;
using AlAshmar.Domain.Entities.Students;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Application.UseCases.Students.GetAttachments;

public class GetAttachmentsHandler(IRepositoryBase<Student, Guid> repository)
    : IRequestHandler<GetAttachmentsQuery, Result<List<StudentAttachmentDetailDto>>>
{
    public async Task<Result<List<StudentAttachmentDetailDto>>> Handle(GetAttachmentsQuery query, CancellationToken cancellationToken = default)
    {
        var student = await repository.GetAsync(
            s => s.Id == query.StudentId,
            q => q.Include(s => s.StudentAttachments).ThenInclude(sa => sa.Attachment).ThenInclude(a => a.Extension));

        if (student.IsError) return student.Errors;
        if (student.Value == null)
            return ApplicationErrors.StudentNotFound;

        var attachmentDtos = student.Value.StudentAttachments
            .Select(sa => new StudentAttachmentDetailDto(
                sa.StudentId,
                sa.AttachmentId,
                sa.Attachment != null ? new AttachmentDetailDto(
                    sa.Attachment.Id,
                    sa.Attachment.Path,
                    sa.Attachment.Type,
                    sa.Attachment.SafeName,
                    sa.Attachment.OriginalName,
                    sa.Attachment.ExtensionId,
                    sa.Attachment.Extension != null ? new AllowableExtensionDto(sa.Attachment.Extension.Id, sa.Attachment.Extension.ExtName) : null) : null)).ToList();

        return attachmentDtos;
    }
}
