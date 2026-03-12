using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Repos;
using AlAshmar.Application.Repos.Includes;
using AlAshmar.Domain.Entities.Academic;
using MediatR;

namespace AlAshmar.Application.UseCases.Dawras.GetAllDawras;

public record GetAllDawrasQuery : IQuery<Result<List<DawraDto>>>;

public class GetAllDawrasHandler(IRepositoryBase<Dawra, Guid> repository)
    : IRequestHandler<GetAllDawrasQuery, Result<List<DawraDto>>>
{
    public async Task<Result<List<DawraDto>>> Handle(GetAllDawrasQuery query, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAllAsync(transform: DawraIncludes.Basic.Apply());
        if (result.IsError)
            return result.Errors;

        var dawras = result.Value
            .Select(d => new DawraDto(d.Id, d.EventName, d.SemesterId, null, []))
            .ToList();

        return dawras;
    }
}
