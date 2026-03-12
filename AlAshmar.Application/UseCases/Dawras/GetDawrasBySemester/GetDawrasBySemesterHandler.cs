using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Repos;
using AlAshmar.Application.Repos.Includes;
using AlAshmar.Domain.Entities.Academic;
using MediatR;

namespace AlAshmar.Application.UseCases.Dawras.GetDawrasBySemester;

public record GetDawrasBySemesterQuery(Guid SemesterId) : IQuery<Result<List<DawraDto>>>;

public class GetDawrasBySemesterHandler(IRepositoryBase<Dawra, Guid> repository)
    : IRequestHandler<GetDawrasBySemesterQuery, Result<List<DawraDto>>>
{
    public async Task<Result<List<DawraDto>>> Handle(GetDawrasBySemesterQuery query, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAllAsync(
            d => d.SemesterId == query.SemesterId,
            DawraIncludes.Basic.Apply());

        if (result.IsError)
            return result.Errors;

        var dawras = result.Value
            .Select(d => new DawraDto(d.Id, d.EventName, d.SemesterId, null, []))
            .ToList();

        return dawras;
    }
}
