using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Repos;
using AlAshmar.Domain.Entities.Academic;
using MediatR;

namespace AlAshmar.Application.UseCases.Dawras.CreateDawra;

public record CreateDawraCommand(
    string EventName,
    Guid SemesterId
) : ICommand<Result<DawraDto>>;

public class CreateDawraHandler(
    IRepositoryBase<Dawra, Guid> dawraRepository,
    IRepositoryBase<Semester, Guid> semesterRepository)
    : IRequestHandler<CreateDawraCommand, Result<DawraDto>>
{
    public async Task<Result<DawraDto>> Handle(CreateDawraCommand command, CancellationToken cancellationToken = default)
    {
        var semesterExists = await semesterRepository.AnyAsync(s => s.Id == command.SemesterId);
        if (!semesterExists)
            return new Error("404", "Semester not found", ErrorKind.NotFound);

        var dawra = new Dawra
        {
            EventName = command.EventName,
            SemesterId = command.SemesterId
        };

        var addResult = await dawraRepository.AddAsync(dawra);
        if (addResult.IsError)
            return addResult.Errors;

        return new DawraDto(dawra.Id, dawra.EventName, dawra.SemesterId, null, []);
    }
}
