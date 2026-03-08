using AlAshmar.Application.Common;
using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Domain.Commons;

namespace AlAshmar.Application.UseCases.Students.GetPoints;

/// <summary>
/// Query to get student's points and achievements.
/// </summary>
public record GetPointsQuery(
    Guid StudentId,
    Guid? SemesterId = null
) : IQuery<Result<List<StudentPointDto>>>;
