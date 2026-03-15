using AlAshmar.Application.Repos;
using AlAshmar.Domain.Entities.Students;
using AlAshmar.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Application.UseCases.Students.UpdateStudentPassword;

public record UpdateStudentPasswordCommand(
    Guid StudentId,
    string NewPassword
) : IRequest<Result<Success>>;

public class UpdateStudentPasswordHandler(
    IRepositoryBase<Student, Guid> studentRepository,
    IRepositoryBase<User, Guid> userRepository)
    : IRequestHandler<UpdateStudentPasswordCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(UpdateStudentPasswordCommand command, CancellationToken cancellationToken = default)
    {
        var studentResult = await studentRepository.GetAsync(
            s => s.Id == command.StudentId,
            transform: q => q.Include(s => s.User));

        if (studentResult.IsError || studentResult.Value?.User == null)
            return ApplicationErrors.StudentUserNotFound;

        var user = studentResult.Value.User;
        user.UpdateUserPassword(PasswordHasher.Hash(command.NewPassword));

        var updateResult = await userRepository.UpdateAsync(user);
        if (updateResult.IsError)
            return updateResult.Errors;

        return new Success("Password updated successfully");
    }
}
