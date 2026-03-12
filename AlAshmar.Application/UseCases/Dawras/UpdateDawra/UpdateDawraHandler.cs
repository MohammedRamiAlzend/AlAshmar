using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Repos;
using AlAshmar.Domain.Entities.Academic;
using MediatR;

namespace AlAshmar.Application.UseCases.Dawras.UpdateDawra;

public record UpdateDawraCommand(
    Guid Id,
    string EventName
) : ICommand<Result<DawraDto>>;

public class UpdateDawraHandler(IRepositoryBase<Dawra, Guid> repository)
    : IRequestHandler<UpdateDawraCommand, Result<DawraDto>>
{
    public async Task<Result<DawraDto>> Handle(UpdateDawraCommand command, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetByIdAsync(command.Id);
        if (result.Value == null)
            return new Error("404", "Dawra not found", ErrorKind.NotFound);

        var dawra = result.Value;
        dawra.EventName = command.EventName;

        var updateResult = await repository.UpdateAsync(dawra);
        if (updateResult.IsError)
            return updateResult.Errors;

        return new DawraDto(dawra.Id, dawra.EventName, dawra.SemesterId, null, []);
    }
}
