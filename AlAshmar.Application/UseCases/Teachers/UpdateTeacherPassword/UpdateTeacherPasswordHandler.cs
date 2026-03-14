using AlAshmar.Domain.Entities.Teachers;
using AlAshmar.Domain.Entities.Users;
using AlAshmar.Application.Repos;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Application.UseCases.Teachers.UpdateTeacherPassword;

public record UpdateTeacherPasswordCommand(
    Guid TeacherId,
    string NewPassword
) : IRequest<Result<Success>>;

public class UpdateTeacherPasswordHandler(
    IRepositoryBase<Teacher, Guid> teacherRepository,
    IRepositoryBase<User, Guid> userRepository)
    : IRequestHandler<UpdateTeacherPasswordCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(UpdateTeacherPasswordCommand command, CancellationToken cancellationToken = default)
    {
        var teacherResult = await teacherRepository.GetAsync(
            t => t.Id == command.TeacherId,
            transform: q => q.Include(t => t.RelatedUser));

        if (teacherResult.IsError || teacherResult.Value?.RelatedUser == null)
            return ApplicationErrors.TeacherUserNotFound;

        var user = teacherResult.Value.RelatedUser;
        user.UpdateUserPassword(PasswordHasher.Hash(command.NewPassword));

        var updateResult = await userRepository.UpdateAsync(user);
        if (updateResult.IsError)
            return updateResult.Errors;

        return new Success("Password updated successfully");
    }
}
