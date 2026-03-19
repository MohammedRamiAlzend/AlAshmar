using AlAshmar.Application.DTOs.Domain;

namespace AlAshmar.Application.UseCases.Teachers.GetEnrollments;

public record GetTeacherEnrollmentsQuery(Guid TeacherId) : IQuery<Result<List<ClassTeacherEnrollmentDto>>>;
