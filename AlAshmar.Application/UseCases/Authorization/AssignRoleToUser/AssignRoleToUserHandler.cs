using AlAshmar.Application.Common;
using AlAshmar.Application.DTOs.Authorization;
using AlAshmar.Application.Interfaces;
using AlAshmar.Domain.Commons;

namespace AlAshmar.Application.UseCases.Authorization.AssignRoleToUser;

public record AssignRoleToUserCommand(AssignRoleToUserDto Dto) : ICommand<Result<Success>>;

public class AssignRoleToUserHandler : ICommandHandler<AssignRoleToUserCommand, Result<Success>>
{
    private readonly IAuthorizationService _authorizationService;

    public AssignRoleToUserHandler(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    public async Task<Result<Success>> Handle(AssignRoleToUserCommand command, CancellationToken cancellationToken = default)
    {
        return await _authorizationService.AssignRoleToUserAsync(command.Dto, cancellationToken);
    }
}
