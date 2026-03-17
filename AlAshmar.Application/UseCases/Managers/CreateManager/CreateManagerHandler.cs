using AlAshmar.Application.DTOs;
using AlAshmar.Application.Repos;
using AlAshmar.Domain.Entities.Managers;

namespace AlAshmar.Application.UseCases.Managers.CreateManager;

public record CreateManagerCommand(
    string Name,
    string UserName,
    string Password
) : IRequest<Result<ManagerDto>>;

public class CreateManagerHandler(IRepositoryBase<Manager, Guid> repository)
    : IRequestHandler<CreateManagerCommand, Result<ManagerDto>>
{
    public async Task<Result<ManagerDto>> Handle(CreateManagerCommand command, CancellationToken cancellationToken = default)
    {
        var manager = Manager.Create(command.Name, command.UserName, command.Password);

        var addResult = await repository.AddAsync(manager);
        if (addResult.IsError)
            return addResult.Errors;

        return new ManagerDto(
            manager.Id,
            manager.Name,
            manager.UserId
        );
    }
}
