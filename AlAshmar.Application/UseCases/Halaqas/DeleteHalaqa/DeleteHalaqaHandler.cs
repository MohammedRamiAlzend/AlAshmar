using AlAshmar.Application.Repos;
using AlAshmar.Domain.Entities.Academic;
using MediatR;

namespace AlAshmar.Application.UseCases.Halaqas.DeleteHalaqa;

public record DeleteHalaqaCommand(Guid Id) : ICommand<Result<Success>>;

public class DeleteHalaqaHandler(IRepositoryBase<Halaqa, Guid> repository)
    : IRequestHandler<DeleteHalaqaCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(DeleteHalaqaCommand command, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetByIdAsync(command.Id);
        if (result.Value == null)
            return new Error("404", "Halaqa not found", ErrorKind.NotFound);

        var deleteResult = await repository.RemoveAsync(h => h.Id == command.Id);
        if (deleteResult.IsError)
            return deleteResult.Errors;

        return Result.Success;
    }
}
