using AlAshmar.Application.Common;
using AlAshmar.Domain.Commons;
using AlAshmar.Domain.Entities.Students;
using AlAshmar.Application.Repos;

namespace AlAshmar.Application.UseCases.Students.EnrollInClass;

public class EnrollInClassHandler(IRepositoryBase<ClassStudentEnrollment, Guid> repository)
    : ICommandHandler<EnrollInClassCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(EnrollInClassCommand command, CancellationToken cancellationToken = default)
    {
        // Check if already enrolled
        var existingEnrollment = await repository.GetAllAsync(
            e => e.StudentId == command.StudentId && e.ClassId == command.ClassId);

        if (existingEnrollment.IsError) return existingEnrollment.Errors;
        if (existingEnrollment.Value.Any())
            return new Error("409", "Student is already enrolled in this class", ErrorKind.Conflict);

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
