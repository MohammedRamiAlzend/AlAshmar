using AlAshmar.Application.Common;
using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Domain.Commons;
using AlAshmar.Domain.Entities.Students;
using AlAshmar.Application.Repos;

namespace AlAshmar.Application.UseCases.Students.GetClassEnrollments;

public class GetClassEnrollmentsHandler(IRepositoryBase<ClassStudentEnrollment, Guid> repository)
    : IQueryHandler<GetClassEnrollmentsQuery, Result<List<ClassStudentEnrollmentDto>>>
{
    public async Task<Result<List<ClassStudentEnrollmentDto>>> Handle(GetClassEnrollmentsQuery query, CancellationToken cancellationToken = default)
    {
        var enrollments = await repository.GetAllAsync(e => e.StudentId == query.StudentId);

        if (enrollments.IsError) return enrollments.Errors;

        var enrollmentDtos = enrollments.Value
            .Select(e => new ClassStudentEnrollmentDto(
                e.Id, e.StudentId, null, e.ClassId)).ToList();

        return enrollmentDtos;
    }
}
