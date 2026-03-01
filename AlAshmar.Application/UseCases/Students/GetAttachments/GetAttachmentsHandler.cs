using AlAshmar.Application.Common;
using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Domain.Commons;
using AlAshmar.Domain.Entities.Students;
using AlAshmar.Application.Repos;
using Microsoft.EntityFrameworkCore;
using Attacment = AlAshmar.Domain.Entities.Common.Attacment;

namespace AlAshmar.Application.UseCases.Students.GetAttachments;

public class GetAttachmentsHandler(IRepositoryBase<Student, Guid> repository)
    : IQueryHandler<GetAttachmentsQuery, Result<List<StudentAttachmentDto>>>
{
    public async Task<Result<List<StudentAttachmentDto>>> Handle(GetAttachmentsQuery query, CancellationToken cancellationToken = default)
    {
        var student = await repository.GetAsync(
            s => s.Id == query.StudentId,
            q => q.Include(s => s.StudentAttachments).ThenInclude(sa => sa.Attachment));

        if (student.IsError) return student.Errors;
        if (student.Value == null)
            return new Error("404", "Student not found", ErrorKind.NotFound);

        var attachmentDtos = student.Value.StudentAttachments
            .Select(sa => new StudentAttachmentDto(
                sa.StudentId,
                sa.AttachmentId,
                null,
                sa.Attachment != null ? new AttacmentDto(
                    sa.Attachment.Id,
                    sa.Attachment.Path,
                    sa.Attachment.Type,
                    sa.Attachment.SafeName,
                    sa.Attachment.OriginalName,
                    sa.Attachment.ExtentionId,
                    null) : null)).ToList();

        return attachmentDtos;
    }
}
