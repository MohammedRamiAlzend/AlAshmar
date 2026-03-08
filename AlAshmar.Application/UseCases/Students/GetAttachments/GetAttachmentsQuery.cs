using AlAshmar.Application.Common;
using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Domain.Commons;

namespace AlAshmar.Application.UseCases.Students.GetAttachments;

/// <summary>
/// Query to get student's attachments.
/// </summary>
public record GetAttachmentsQuery(Guid StudentId) : IQuery<Result<List<StudentAttachmentDetailDto>>>;
