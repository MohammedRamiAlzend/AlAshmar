using AlAshmar.Application.Common;
using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Domain.Commons;
using AlAshmar.Domain.Entities.Students;
using AlAshmar.Application.Repos;
using Microsoft.EntityFrameworkCore;
using Attacment = AlAshmar.Domain.Entities.Common.Attacment;
using MediatR;

namespace AlAshmar.Application.UseCases.Students.GetAttachments;

public class GetAttachmentsHandler(IRepositoryBase<Student, Guid> repository)
    : IRequestHandler<GetAttachmentsQuery, Result<List<StudentAttachmentDetailDto>>>
{
    public async Task<Result<List<StudentAttachmentDetailDto>>> Handle(GetAttachmentsQuery query, CancellationToken cancellationToken = default)
    {
        var student = await repository.GetAsync(
            s => s.Id == query.StudentId,
            q => q.Include(s => s.StudentAttachments).ThenInclude(sa => sa.Attachment).ThenInclude(a => a.Extention));

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
                    sa.Attachment.ExtentionId,
                    sa.Attachment.Extention != null ? new AllowableExtentionDto(sa.Attachment.Extention.Id, sa.Attachment.Extention.ExtName) : null) : null)).ToList();

        return attachmentDtos;
    }
}
