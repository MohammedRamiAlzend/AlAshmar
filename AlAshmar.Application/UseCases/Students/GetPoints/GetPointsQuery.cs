using AlAshmar.Application.DTOs.Domain;

namespace AlAshmar.Application.UseCases.Students.GetPoints;




public record GetPointsQuery(
    Guid StudentId,
    Guid? SemesterId = null
) : IQuery<Result<List<StudentPointDto>>>;
