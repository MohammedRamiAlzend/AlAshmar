using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Repos;
using AlAshmar.Domain.Entities.Teachers;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Application.UseCases.Teachers.GetEnrollments;

public class GetTeacherEnrollmentsHandler(IRepositoryBase<ClassTeacherEnrollment, Guid> repository)
    : IRequestHandler<GetTeacherEnrollmentsQuery, Result<List<ClassTeacherEnrollmentDto>>>
{
    public async Task<Result<List<ClassTeacherEnrollmentDto>>> Handle(GetTeacherEnrollmentsQuery query, CancellationToken cancellationToken = default)
    {
        var enrollments = await repository.GetAllAsync(
            e => e.TeacherId == query.TeacherId,
            q => q.Include(e => e.Teacher)
        );

        if (enrollments.IsError) return enrollments.Errors;

        var dtos = enrollments.Value
            .Select(e => new ClassTeacherEnrollmentDto(
                e.Id,
                e.TeacherId,
                null,
                e.IsMainTeacher,
                e.ClassId))
            .ToList();

        return dtos;
    }
}
