using AlAshmar.Application.Common;
using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Domain.Commons;
using AlAshmar.Domain.Entities.Students;
using AlAshmar.Domain.Entities.Academic;
using AlAshmar.Application.Repos;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Application.UseCases.Students.GetPoints;

public class GetPointsHandler(
    IRepositoryBase<Point, Guid> repository,
    IRepositoryBase<StudentClassEventsPoint, Guid> classEventsRepository)
    : IQueryHandler<GetPointsQuery, Result<List<StudentPointDto>>>
{
    public async Task<Result<List<StudentPointDto>>> Handle(GetPointsQuery query, CancellationToken cancellationToken = default)
    {
        var pointsQuery = await repository.GetAllAsync(
            p => p.StudentId == query.StudentId &&
                 (!query.SemesterId.HasValue || p.SmesterId == query.SemesterId.Value),
            q => q.Include(p => p.Category));

        if (pointsQuery.IsError) return pointsQuery.Errors;

        var pointDtos = pointsQuery.Value
            .Select(p => new StudentPointDto(
                p.Id, p.StudentId, p.EventId, p.ClassId, p.SmesterId,
                p.PointValue, p.CategoryId,
                p.Category != null ? new PointCategorySummaryDto(p.Category.Id, p.Category.Type) : null,
                p.GivenByTeacherId)).ToList();

        return pointDtos;
    }
}
