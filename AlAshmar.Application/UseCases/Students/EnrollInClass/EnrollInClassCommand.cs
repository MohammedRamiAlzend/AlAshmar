using AlAshmar.Application.Common;
using AlAshmar.Domain.Commons;

namespace AlAshmar.Application.UseCases.Students.EnrollInClass;

/// <summary>
/// Command to enroll a student in a class.
/// </summary>
public record EnrollInClassCommand(Guid StudentId, Guid ClassId) : ICommand<Result<Success>>;
