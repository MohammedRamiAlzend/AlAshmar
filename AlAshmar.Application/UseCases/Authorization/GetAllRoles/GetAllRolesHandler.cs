using AlAshmar.Application.Interfaces;
using AlAshmar.Domain.DTOs.Domain;

namespace AlAshmar.Application.UseCases.Authorization.GetAllRoles;

public record GetAllRolesQuery : IQuery<Result<List<RoleDto>>>;

public class GetAllRolesHandler : IRequestHandler<GetAllRolesQuery, Result<List<RoleDto>>>
{
    private readonly IAuthorizationService _authorizationService;

    public GetAllRolesHandler(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    public async Task<Result<List<RoleDto>>> Handle(GetAllRolesQuery query, CancellationToken cancellationToken = default)
    {
        return await _authorizationService.GetAllRolesAsync(cancellationToken);
    }
}
