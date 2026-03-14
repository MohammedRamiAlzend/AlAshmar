using AlAshmar.Application.DTOs;
using AlAshmar.Domain.Entities.Managers;
using AlAshmar.Application.Repos;
using AlAshmar.Application.Repos.Includes;

namespace AlAshmar.Application.UseCases.Managers.GetAllManagers;

public record GetAllManagersQuery : IQuery<Result<List<ManagerDto>>>;

public class GetAllManagersHandler : IRequestHandler<GetAllManagersQuery, Result<List<ManagerDto>>>
{
    private readonly IRepositoryBase<Manager, Guid> _repository;

    public GetAllManagersHandler(IRepositoryBase<Manager, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<ManagerDto>>> Handle(GetAllManagersQuery query, CancellationToken cancellationToken = default)
    {
        var managersResult = await _repository.GetAllAsync(transform: ManagerIncludes.None.Apply());
        if (managersResult.IsError)
            return managersResult.Errors;

        var managers = managersResult.Value
            .Select(m => new ManagerDto(
                m.Id,
                m.Name,
                m.UserId
            ))
            .ToList();

        return managers;
    }
}
