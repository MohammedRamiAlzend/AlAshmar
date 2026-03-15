namespace AlAshmar.Application.UseCases.Students.EnrollInClass;

public record EnrollInClassCommand(Guid StudentId, Guid ClassId) : IRequest<Result<Success>>;
