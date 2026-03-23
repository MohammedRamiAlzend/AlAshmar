using AlAshmar.Application.Repos;
using AlAshmar.Domain.DTOs.Domain;
using AlAshmar.Domain.Entities.Academic;

namespace AlAshmar.Application.UseCases.Halaqas.UpdateHalaqa;

public record UpdateHalaqaCommand(
    Guid Id,
    string ClassName
) : IRequest<Result<HalaqaDto>>;

public class UpdateHalaqaHandler(IRepositoryBase<Halaqa, Guid> repository)
    : IRequestHandler<UpdateHalaqaCommand, Result<HalaqaDto>>
{
    public async Task<Result<HalaqaDto>> Handle(UpdateHalaqaCommand command, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetByIdAsync(command.Id);
        if (result.Value == null)
            return ApplicationErrors.HalaqaNotFound;

        var halaqa = result.Value;
        halaqa.HalaqaName = command.ClassName;

        var updateResult = await repository.UpdateAsync(halaqa);
        if (updateResult.IsError)
            return updateResult.Errors;

        return new HalaqaDto(halaqa.Id, halaqa.HalaqaName, halaqa.CourseId, null);
    }
}
