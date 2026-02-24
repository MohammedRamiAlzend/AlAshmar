using AlAshmar.Application.Common;
using AlAshmar.Application.DTOs.Authorization;
using AlAshmar.Application.Interfaces;
using AlAshmar.Domain.Commons;

namespace AlAshmar.Application.UseCases.Authorization.GetRoleById;

public record GetRoleByIdQuery(Guid Id) : IQuery<Result<RoleDto?>>;

public class GetRoleByIdHandler : IQueryHandler<GetRoleByIdQuery, Result<RoleDto?>>
{
    private readonly IAuthorizationService _authorizationService;

    public GetRoleByIdHandler(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    public async Task<Result<RoleDto?>> Handle(GetRoleByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _authorizationService.GetRoleByIdAsync(query.Id, cancellationToken);
    }
}
