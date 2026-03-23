using AlAshmar.Domain.DTOs.Domain;

namespace AlAshmar.Application.UseCases.Managers.GetAttachments;

public record GetManagerAttachmentsQuery(Guid ManagerId) : IQuery<Result<List<ManagerAttachmentDto>>>;
