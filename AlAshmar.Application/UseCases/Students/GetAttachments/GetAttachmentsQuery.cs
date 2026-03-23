using AlAshmar.Domain.DTOs.Domain;

namespace AlAshmar.Application.UseCases.Students.GetAttachments;

public record GetAttachmentsQuery(Guid StudentId) : IQuery<Result<List<StudentAttachmentDetailDto>>>;
