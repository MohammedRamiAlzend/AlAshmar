using AlAshmar.Application.Repos;
using AlAshmar.Domain.Commons;
using AlAshmar.Domain.Entities.Teachers;

namespace AlAshmar.Application.UseCases.Teachers.EnrollInClass;

public class EnrollTeacherInClassHandler(IRepositoryBase<ClassTeacherEnrollment, Guid> repository)
    : IRequestHandler<EnrollTeacherInClassCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(EnrollTeacherInClassCommand command, CancellationToken cancellationToken = default)
    {
        var existingEnrollment = await repository.GetAllAsync(
            e => e.TeacherId == command.TeacherId && e.ClassId == command.ClassId);

        if (existingEnrollment.IsError) return existingEnrollment.Errors;
        if (existingEnrollment.Value.Any())
            return ApplicationErrors.TeacherAlreadyAssignedToClass;

        var enrollment = new ClassTeacherEnrollment
        {
            TeacherId = command.TeacherId,
            ClassId = command.ClassId,
            IsMainTeacher = command.IsMainTeacher
        };

        var addResult = await repository.AddAsync(enrollment);
        if (addResult.IsError) return addResult.Errors;

        return Result.Success;
    }
}
