using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Repos;
using AlAshmar.Application.Repos.Includes;
using AlAshmar.Domain.Entities.Academic;
using MediatR;

namespace AlAshmar.Application.UseCases.Dawras.GetDawraById;

public record GetDawraByIdQuery(Guid Id) : IQuery<Result<DawraDto?>>;

public class GetDawraByIdHandler(IRepositoryBase<Dawra, Guid> repository)
    : IRequestHandler<GetDawraByIdQuery, Result<DawraDto?>>
{
    public async Task<Result<DawraDto?>> Handle(GetDawraByIdQuery query, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAllAsync(
            d => d.Id == query.Id,
            DawraIncludes.Full.Apply());

        if (result.IsError)
            return result.Errors;

        var dawra = result.Value.FirstOrDefault();
        if (dawra == null)
            return new Error("404", "Dawra not found", ErrorKind.NotFound);

        var semesterDto = dawra.Semester is not null
            ? new SemesterDto(dawra.Semester.Id, dawra.Semester.StartDate, dawra.Semester.EndDate, dawra.Semester.Name)
            : null;

        var halaqaDtos = dawra.Halaqas
            .Select(h => new HalaqaDto(h.Id, h.ClassName, h.DawraId, null))
            .ToList();

        return new DawraDto(dawra.Id, dawra.EventName, dawra.SemesterId, semesterDto, halaqaDtos);
    }
}
