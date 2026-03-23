global using AlAshmar.Application.Repos;
global using AlAshmar.Domain.Entities.Managers;
using AlAshmar.Domain.DTOs.Domain;

namespace AlAshmar.Application.UseCases.Managers.UpdateManager;

public record UpdateManagerCommand(
    Guid Id,
    string Name
) : IRequest<Result<ManagerDto>>;

public class UpdateManagerHandler : IRequestHandler<UpdateManagerCommand, Result<ManagerDto>>
{
    private readonly IRepositoryBase<Manager, Guid> _repository;

    public UpdateManagerHandler(IRepositoryBase<Manager, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<Result<ManagerDto>> Handle(UpdateManagerCommand command, CancellationToken cancellationToken = default)
    {
        var managerResult = await _repository.GetByIdAsync(command.Id);
        if (managerResult.Value == null)
            return ApplicationErrors.ManagerNotFound;

        var manager = managerResult.Value;
        manager.UpdateBasicInfo(command.Name);

        var updateResult = await _repository.UpdateAsync(manager);
        if (updateResult.IsError)
            return updateResult.Errors;

        return new ManagerDto(
            manager.Id,
            manager.Name,
            manager.UserId
        );
    }
}
