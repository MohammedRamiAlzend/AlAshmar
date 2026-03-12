using AlAshmar.Application.Repos;
using AlAshmar.Domain.Entities.Academic;
using MediatR;

namespace AlAshmar.Application.UseCases.Dawras.DeleteDawra;

public record DeleteDawraCommand(Guid Id) : ICommand<Result<Success>>;

public class DeleteDawraHandler(IRepositoryBase<Dawra, Guid> repository)
    : IRequestHandler<DeleteDawraCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(DeleteDawraCommand command, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetByIdAsync(command.Id);
        if (result.Value == null)
            return new Error("404", "Dawra not found", ErrorKind.NotFound);

        var deleteResult = await repository.RemoveAsync(d => d.Id == command.Id);
        if (deleteResult.IsError)
            return deleteResult.Errors;

        return Result.Success;
    }
}
