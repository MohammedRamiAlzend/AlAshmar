using AlAshmar.Application.Interfaces;
using AlAshmar.Domain.DTOs.Domain;

namespace AlAshmar.Application.UseCases.Authorization.AssignRoleToUser;

public record AssignRoleToUserCommand(AssignRoleToUserDto Dto) : IRequest<Result<Success>>;

public class AssignRoleToUserHandler : IRequestHandler<AssignRoleToUserCommand, Result<Success>>
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
