using AlAshmar.Application.Repos;
using AlAshmar.Application.Repos.Includes;
using AlAshmar.Domain.DTOs.Domain;
using AlAshmar.Domain.Entities.Academic;

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
            return ApplicationErrors.HalaqaNotFound;

        var courseDto = halaqa.Course is not null
            ? new CourseDto(halaqa.Course.Id, halaqa.Course.CourseName, halaqa.Course.SemesterId, null, [])
            : null;

        return new HalaqaDto(halaqa.Id, halaqa.HalaqaName, halaqa.CourseId, courseDto);
    }
}
