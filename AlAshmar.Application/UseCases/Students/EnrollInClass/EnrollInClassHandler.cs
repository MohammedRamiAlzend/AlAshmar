using AlAshmar.Domain.Entities.Students;
using AlAshmar.Application.Repos;

namespace AlAshmar.Application.UseCases.Students.EnrollInClass;

public class EnrollInClassHandler(IRepositoryBase<ClassStudentEnrollment, Guid> repository)
    : IRequestHandler<EnrollInClassCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(EnrollInClassCommand command, CancellationToken cancellationToken = default)
    {

        var existingEnrollment = await repository.GetAllAsync(
            e => e.StudentId == command.StudentId && e.ClassId == command.ClassId);

        if (existingEnrollment.IsError) return existingEnrollment.Errors;
        if (existingEnrollment.Value.Any())
            return ApplicationErrors.StudentAlreadyEnrolledInClass;

        var enrollment = new ClassStudentEnrollment
        {
            StudentId = command.StudentId,
            ClassId = command.ClassId
        };

        var addResult = await repository.AddAsync(enrollment);
        if (addResult.IsError) return addResult.Errors;

        return Result.Success;
    }
}
