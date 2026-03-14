using AlAshmar.Application.DTOs.Domain;

namespace AlAshmar.Application.UseCases.Students.GetMemorizationProgress;




public record GetMemorizationProgressQuery(Guid StudentId) : IQuery<Result<StudentMemorizationProgressDto>>;
