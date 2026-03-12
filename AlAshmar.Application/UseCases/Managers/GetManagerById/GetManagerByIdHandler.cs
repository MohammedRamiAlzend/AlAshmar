using AlAshmar.Application.Common;
using AlAshmar.Application.DTOs;
using AlAshmar.Domain.Commons;
using AlAshmar.Domain.Entities.Managers;
using AlAshmar.Application.Repos;
using AlAshmar.Application.Repos.Includes;
using MediatR;

namespace AlAshmar.Application.UseCases.Managers.GetManagerById;

public record GetManagerByIdQuery(Guid Id) : IQuery<Result<ManagerDto?>>;

public class GetManagerByIdHandler : IRequestHandler<GetManagerByIdQuery, Result<ManagerDto?>>
{
    private readonly IRepositoryBase<Manager, Guid> _repository;

    public GetManagerByIdHandler(IRepositoryBase<Manager, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<Result<ManagerDto?>> Handle(GetManagerByIdQuery query, CancellationToken cancellationToken = default)
    {
        var managerResult = await _repository.GetAllAsync(
            m => m.Id == query.Id,
            ManagerIncludes.Full.Apply()
        );

        if (managerResult.IsError)
            return managerResult.Errors;

        var manager = managerResult.Value.FirstOrDefault();

        if (manager == null)
            return new Error("404", "Manager not found", ErrorKind.NotFound);

        return new ManagerDto(
            manager.Id,
            manager.Name,
            manager.UserId
        );
    }
}
