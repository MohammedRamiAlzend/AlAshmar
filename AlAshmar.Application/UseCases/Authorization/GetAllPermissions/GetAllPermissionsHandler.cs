using AlAshmar.Application.DTOs.Authorization;
using AlAshmar.Application.Interfaces;

namespace AlAshmar.Application.UseCases.Authorization.GetAllPermissions;

public record GetAllPermissionsQuery : IQuery<Result<List<PermissionDto>>>;

public class GetAllPermissionsHandler : IRequestHandler<GetAllPermissionsQuery, Result<List<PermissionDto>>>
{
    private readonly IAuthorizationService _authorizationService;

    public GetAllPermissionsHandler(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    public async Task<Result<List<PermissionDto>>> Handle(GetAllPermissionsQuery query, CancellationToken cancellationToken = default)
    {
        return await _authorizationService.GetAllPermissionsAsync(cancellationToken);
    }
}
