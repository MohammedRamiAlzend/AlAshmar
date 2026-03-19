namespace AlAshmar.Application.UseCases.Teachers.EnrollInClass;

public record EnrollTeacherInClassCommand(Guid TeacherId, Guid ClassId, bool IsMainTeacher = false) : IRequest<Result<Success>>;
