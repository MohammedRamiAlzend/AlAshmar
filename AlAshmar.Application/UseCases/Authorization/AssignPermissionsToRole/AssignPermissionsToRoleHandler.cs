using AlAshmar.Application.Common;
using AlAshmar.Application.DTOs.Authorization;
using AlAshmar.Application.Interfaces;
using AlAshmar.Domain.Commons;

namespace AlAshmar.Application.UseCases.Authorization.AssignPermissionsToRole;

public record AssignPermissionsToRoleCommand(AssignPermissionsToRoleDto Dto) : ICommand<Result<RoleDto>>;

public class AssignPermissionsToRoleHandler : ICommandHandler<AssignPermissionsToRoleCommand, Result<RoleDto>>
{
    private readonly IAuthorizationService _authorizationService;

    public AssignPermissionsToRoleHandler(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    public async Task<Result<RoleDto>> Handle(AssignPermissionsToRoleCommand command, CancellationToken cancellationToken = default)
    {
        return await _authorizationService.AssignPermissionsToRoleAsync(command.Dto, cancellationToken);
    }
}
