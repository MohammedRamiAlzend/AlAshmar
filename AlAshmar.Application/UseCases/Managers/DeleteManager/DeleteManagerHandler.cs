using AlAshmar.Domain.Entities.Managers;
using AlAshmar.Application.Repos;

namespace AlAshmar.Application.UseCases.Managers.DeleteManager;

public record DeleteManagerCommand(Guid Id) : ICommand<Result<Success>>;

public class DeleteManagerHandler : IRequestHandler<DeleteManagerCommand, Result<Success>>
{
    private readonly IRepositoryBase<Manager, Guid> _repository;

    public DeleteManagerHandler(IRepositoryBase<Manager, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<Result<Success>> Handle(DeleteManagerCommand command, CancellationToken cancellationToken = default)
    {
        var manager = await _repository.GetByIdAsync(command.Id);
        if (manager.Value == null)
            return ApplicationErrors.ManagerNotFound;

        var result = await _repository.RemoveAsync(m => m.Id == command.Id);
        if (result.IsError)
            return result.Errors;

        return Result.Success;
    }
}
