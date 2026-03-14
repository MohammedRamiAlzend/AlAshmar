using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Repos;
using AlAshmar.Domain.Entities.Academic;

namespace AlAshmar.Application.UseCases.Halaqas.CreateHalaqa;

public record CreateHalaqaCommand(
    string ClassName,
    Guid CourseId
) : IRequest<Result<HalaqaDto>>;

public class CreateHalaqaHandler(
    IRepositoryBase<Halaqa, Guid> halaqaRepository,
    IRepositoryBase<Course, Guid> courseRepository)
    : IRequestHandler<CreateHalaqaCommand, Result<HalaqaDto>>
{
    public async Task<Result<HalaqaDto>> Handle(CreateHalaqaCommand command, CancellationToken cancellationToken = default)
    {
        var courseExists = await courseRepository.AnyAsync(d => d.Id == command.CourseId);
        if (!courseExists)
            return ApplicationErrors.CourseNotFound;

        var halaqa = new Halaqa
        {
            ClassName = command.ClassName,
            CourseId = command.CourseId
        };

        var addResult = await halaqaRepository.AddAsync(halaqa);
        if (addResult.IsError)
            return addResult.Errors;

        return new HalaqaDto(halaqa.Id, halaqa.ClassName, halaqa.CourseId, null);
    }
}
