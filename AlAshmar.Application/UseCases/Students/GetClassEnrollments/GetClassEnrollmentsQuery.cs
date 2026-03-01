using AlAshmar.Application.Common;
using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Domain.Commons;

namespace AlAshmar.Application.UseCases.Students.GetClassEnrollments;

/// <summary>
/// Query to get student's class enrollments.
/// </summary>
public record GetClassEnrollmentsQuery(Guid StudentId) : IQuery<Result<List<ClassStudentEnrollmentDto>>>;
