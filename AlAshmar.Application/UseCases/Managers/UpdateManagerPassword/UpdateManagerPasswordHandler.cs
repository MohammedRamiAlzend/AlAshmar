using AlAshmar.Application.Common;
using AlAshmar.Domain.Commons;
using AlAshmar.Domain.Entities.Managers;
using AlAshmar.Domain.Entities.Users;
using AlAshmar.Application.Repos;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace AlAshmar.Application.UseCases.Managers.UpdateManagerPassword;

public record UpdateManagerPasswordCommand(
    Guid ManagerId,
    string NewPassword
) : ICommand<Result<Success>>;

public class UpdateManagerPasswordHandler(
    IRepositoryBase<Manager, Guid> managerRepository,
    IRepositoryBase<User, Guid> userRepository)
    : IRequestHandler<UpdateManagerPasswordCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(UpdateManagerPasswordCommand command, CancellationToken cancellationToken = default)
    {
        var managerResult = await managerRepository.GetAsync(
            m => m.Id == command.ManagerId,
            transform: q => q.Include(m => m.User));

        if (managerResult.IsError || managerResult.Value?.User == null)
            return new Error("404", "Manager or associated user not found", ErrorKind.NotFound);

        var user = managerResult.Value.User;
        user.UpdateUserPassword(PasswordHasher.Hash(command.NewPassword));

        var updateResult = await userRepository.UpdateAsync(user);
        if (updateResult.IsError)
            return updateResult.Errors;

        return new Success("Password updated successfully");
    }
}
