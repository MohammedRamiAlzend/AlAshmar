using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Repos;
using AlAshmar.Application.Repos.Includes;
using AlAshmar.Domain.Entities.Academic;

namespace AlAshmar.Application.UseCases.Halaqas.GetHalaqasByCourse;

public record GetHalaqasByCourseQuery(Guid CourseId) : IQuery<Result<List<HalaqaDto>>>;

public class GetHalaqasByCourseHandler(IRepositoryBase<Halaqa, Guid> repository)
    : IRequestHandler<GetHalaqasByCourseQuery, Result<List<HalaqaDto>>>
{
    public async Task<Result<List<HalaqaDto>>> Handle(GetHalaqasByCourseQuery query, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAllAsync(
            h => h.CourseId == query.CourseId,
            HalaqaIncludes.None.Apply());

        if (result.IsError)
            return result.Errors;

        var halaqas = result.Value
            .Select(h => new HalaqaDto(h.Id, h.ClassName, h.CourseId, null))
            .ToList();

        return halaqas;
    }
}
