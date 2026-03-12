using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Repos;
using AlAshmar.Application.Repos.Includes;
using AlAshmar.Domain.Entities.Academic;
using MediatR;

namespace AlAshmar.Application.UseCases.Halaqas.GetHalaqasByDawra;

public record GetHalaqasByDawraQuery(Guid DawraId) : IQuery<Result<List<HalaqaDto>>>;

public class GetHalaqasByDawraHandler(IRepositoryBase<Halaqa, Guid> repository)
    : IRequestHandler<GetHalaqasByDawraQuery, Result<List<HalaqaDto>>>
{
    public async Task<Result<List<HalaqaDto>>> Handle(GetHalaqasByDawraQuery query, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAllAsync(
            h => h.DawraId == query.DawraId,
            HalaqaIncludes.None.Apply());

        if (result.IsError)
            return result.Errors;

        var halaqas = result.Value
            .Select(h => new HalaqaDto(h.Id, h.ClassName, h.DawraId, null))
            .ToList();

        return halaqas;
    }
}
