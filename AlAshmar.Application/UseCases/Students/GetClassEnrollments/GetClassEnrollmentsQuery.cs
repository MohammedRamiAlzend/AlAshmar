using AlAshmar.Domain.DTOs.Domain;

namespace AlAshmar.Application.UseCases.Students.GetClassEnrollments;

public record GetClassEnrollmentsQuery(Guid StudentId) : IQuery<Result<List<ClassEnrollmentWithStudentDto>>>;
