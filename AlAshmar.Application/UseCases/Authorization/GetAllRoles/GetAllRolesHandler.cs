using AlAshmar.Application.Common;
using AlAshmar.Application.DTOs.Authorization;
using AlAshmar.Application.Interfaces;
using AlAshmar.Domain.Commons;

namespace AlAshmar.Application.UseCases.Authorization.GetAllRoles;

public record GetAllRolesQuery : IQuery<Result<List<RoleDto>>>;

public class GetAllRolesHandler : IQueryHandler<GetAllRolesQuery, Result<List<RoleDto>>>
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
