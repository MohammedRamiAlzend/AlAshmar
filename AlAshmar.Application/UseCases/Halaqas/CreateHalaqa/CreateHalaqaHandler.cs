using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Repos;
using AlAshmar.Domain.Entities.Academic;
using MediatR;

namespace AlAshmar.Application.UseCases.Halaqas.CreateHalaqa;

public record CreateHalaqaCommand(
    string ClassName,
    Guid DawraId
) : ICommand<Result<HalaqaDto>>;

public class CreateHalaqaHandler(
    IRepositoryBase<Halaqa, Guid> halaqaRepository,
    IRepositoryBase<Dawra, Guid> dawraRepository)
    : IRequestHandler<CreateHalaqaCommand, Result<HalaqaDto>>
{
    public async Task<Result<HalaqaDto>> Handle(CreateHalaqaCommand command, CancellationToken cancellationToken = default)
    {
        var dawraExists = await dawraRepository.AnyAsync(d => d.Id == command.DawraId);
        if (!dawraExists)
            return new Error("404", "Dawra not found", ErrorKind.NotFound);

        var halaqa = new Halaqa
        {
            ClassName = command.ClassName,
            DawraId = command.DawraId
        };

        var addResult = await halaqaRepository.AddAsync(halaqa);
        if (addResult.IsError)
            return addResult.Errors;

        return new HalaqaDto(halaqa.Id, halaqa.ClassName, halaqa.DawraId, null);
    }
}
