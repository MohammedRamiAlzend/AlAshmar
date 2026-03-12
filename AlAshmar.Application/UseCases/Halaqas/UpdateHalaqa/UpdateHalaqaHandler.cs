using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Repos;
using AlAshmar.Domain.Entities.Academic;
using MediatR;

namespace AlAshmar.Application.UseCases.Halaqas.UpdateHalaqa;

public record UpdateHalaqaCommand(
    Guid Id,
    string ClassName
) : ICommand<Result<HalaqaDto>>;

public class UpdateHalaqaHandler(IRepositoryBase<Halaqa, Guid> repository)
    : IRequestHandler<UpdateHalaqaCommand, Result<HalaqaDto>>
{
    public async Task<Result<HalaqaDto>> Handle(UpdateHalaqaCommand command, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetByIdAsync(command.Id);
        if (result.Value == null)
            return new Error("404", "Halaqa not found", ErrorKind.NotFound);

        var halaqa = result.Value;
        halaqa.ClassName = command.ClassName;

        var updateResult = await repository.UpdateAsync(halaqa);
        if (updateResult.IsError)
            return updateResult.Errors;

        return new HalaqaDto(halaqa.Id, halaqa.ClassName, halaqa.DawraId, null);
    }
}
