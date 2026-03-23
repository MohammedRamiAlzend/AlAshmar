using AlAshmar.Domain.DTOs.Domain;

namespace AlAshmar.Application.UseCases.Teachers.GetAttachments;

public record GetTeacherAttachmentsQuery(Guid TeacherId) : IQuery<Result<List<TeacherAttachmentDto>>>;
