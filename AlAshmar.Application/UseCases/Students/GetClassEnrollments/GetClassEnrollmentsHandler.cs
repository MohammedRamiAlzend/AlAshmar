using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Domain.Entities.Students;
using AlAshmar.Application.Repos;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Application.UseCases.Students.GetClassEnrollments;

public class GetClassEnrollmentsHandler(IRepositoryBase<ClassStudentEnrollment, Guid> repository)
    : IRequestHandler<GetClassEnrollmentsQuery, Result<List<ClassEnrollmentWithStudentDto>>>
{
    public async Task<Result<List<ClassEnrollmentWithStudentDto>>> Handle(GetClassEnrollmentsQuery query, CancellationToken cancellationToken = default)
    {
        var enrollments = await repository.GetAllAsync(
            e => e.StudentId == query.StudentId,
            q => q.Include(e => e.Student)
        );

        if (enrollments.IsError) return enrollments.Errors;

        var enrollmentDtos = enrollments.Value
            .Select(e => new ClassEnrollmentWithStudentDto(
                e.Id,
                e.ClassId,
                "Class",
                e.StudentId,
                e.Student?.Name ?? "Unknown",
                true)).ToList();

        return enrollmentDtos;
    }
}
