using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Repos;
using AlAshmar.Application.Repos.Includes;
using AlAshmar.Domain.Entities.Academic;

namespace AlAshmar.Application.UseCases.Halaqas.GetAllHalaqas;

public record GetAllHalaqasQuery : IQuery<Result<List<HalaqaDto>>>;

public class GetAllHalaqasHandler(IRepositoryBase<Halaqa, Guid> repository)
    : IRequestHandler<GetAllHalaqasQuery, Result<List<HalaqaDto>>>
{
    public async Task<Result<List<HalaqaDto>>> Handle(GetAllHalaqasQuery query, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAllAsync(transform: HalaqaIncludes.None.Apply());
        if (result.IsError)
            return result.Errors;

        var halaqas = result.Value
            .Select(h => new HalaqaDto(h.Id, h.HalaqaName, h.CourseId, null))
            .ToList();

        return halaqas;
    }
}
