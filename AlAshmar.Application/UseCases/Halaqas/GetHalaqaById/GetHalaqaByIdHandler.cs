using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Repos;
using AlAshmar.Application.Repos.Includes;
using AlAshmar.Domain.Entities.Academic;
using MediatR;

namespace AlAshmar.Application.UseCases.Halaqas.GetHalaqaById;

public record GetHalaqaByIdQuery(Guid Id) : IQuery<Result<HalaqaDto?>>;

public class GetHalaqaByIdHandler(IRepositoryBase<Halaqa, Guid> repository)
    : IRequestHandler<GetHalaqaByIdQuery, Result<HalaqaDto?>>
{
    public async Task<Result<HalaqaDto?>> Handle(GetHalaqaByIdQuery query, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAllAsync(
            h => h.Id == query.Id,
            HalaqaIncludes.Full.Apply());

        if (result.IsError)
            return result.Errors;

        var halaqa = result.Value.FirstOrDefault();
        if (halaqa == null)
            return new Error("404", "Halaqa not found", ErrorKind.NotFound);

        var dawraDto = halaqa.Dawra is not null
            ? new DawraDto(halaqa.Dawra.Id, halaqa.Dawra.EventName, halaqa.Dawra.SemesterId, null, [])
            : null;

        return new HalaqaDto(halaqa.Id, halaqa.ClassName, halaqa.DawraId, dawraDto);
    }
}
