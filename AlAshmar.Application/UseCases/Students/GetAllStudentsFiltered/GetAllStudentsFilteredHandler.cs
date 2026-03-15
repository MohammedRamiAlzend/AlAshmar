using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Repos;
using AlAshmar.Application.Repos.Includes;
using AlAshmar.Domain.Entities.Students;
using ExpressionBuilderLib.src.Core;

namespace AlAshmar.Application.UseCases.Students.GetAllStudentsFiltered;

public record GetAllStudentsFilteredQuery(
    int? PageNumber = null,
    int? PageSize = null,
    Guid? ClassId = null,
    Guid? SemesterId = null,
    Guid? EventId = null,
    Guid? TeacherId = null
) : IQuery<Result<List<StudentListItemDto>>>;

public class GetAllStudentsFilteredHandler(IRepositoryBase<Student, Guid> repository) : IRequestHandler<GetAllStudentsFilteredQuery, Result<List<StudentListItemDto>>>
{
    public async Task<Result<List<StudentListItemDto>>> Handle(GetAllStudentsFilteredQuery query, CancellationToken cancellationToken = default)
    {
        var filterExpression = BuildFilterExpression(query);

        var transform = StudentIncludes.Basic.Apply();

        List<Student> students;

        if (query.PageNumber.HasValue && query.PageSize.HasValue)
        {
            var pageNumber = query.PageNumber.Value;
            var pageSize = query.PageSize.Value;

            var pagedResult = await repository.GetPagedAsync(
                pageNumber,
                pageSize,
                filterExpression,
                transform
            );

            if (pagedResult.IsError)
                return pagedResult.Errors;

            students = [.. pagedResult.Value!.Items];
        }
        else
        {
            var result = await repository.GetAllAsync(filterExpression, transform);

            if (result.IsError)
                return result.Errors;

            students = [.. result.Value!];
        }

        var studentDtos = students.Select(s => new StudentListItemDto(
            s.Id,
            s.Name,
            s.FatherName,
            s.MotherName,
            s.NationalityNumber,
            s.Email,
            s.User?.UserName,
            s.User?.RoleId != null ? GetRoleType(s.User.RoleId) : null
        )).ToList();

        return studentDtos;
    }

    private static string GetRoleType(Guid roleId)
    {

        return roleId == Guid.Parse("00000000-0000-0000-0000-000000000001") ? "Student" : "Unknown";
    }

    private static Expression<Func<Student, bool>>? BuildFilterExpression(GetAllStudentsFilteredQuery query)
    {
        var expressions = new List<Expression<Func<Student, bool>>>();

        if (query.ClassId.HasValue)
        {
            expressions.Add(BuildClassFilter(query.ClassId.Value));
        }

        if (query.SemesterId.HasValue)
        {
            expressions.Add(BuildSemesterFilter(query.SemesterId.Value));
        }

        if (query.EventId.HasValue)
        {
            expressions.Add(BuildEventFilter(query.EventId.Value));
        }

        if (query.TeacherId.HasValue)
        {
            expressions.Add(BuildTeacherFilter(query.TeacherId.Value));
        }

        return expressions.Count > 0 ? ExpressionCombiner.OrAll(expressions.ToArray()) : null;
    }

    private static Expression<Func<Student, bool>> BuildClassFilter(Guid classId)
    {
        return s => s.StudentClassEventsPoints.Any(p => p.ClassId == classId)
                 || s.StudentHadiths.Any(h => h.ClassId == classId)
                 || s.StudentQuraanPages.Any(q => q.ClassId == classId);
    }

    private static Expression<Func<Student, bool>> BuildSemesterFilter(Guid semesterId)
    {
        return s => s.StudentClassEventsPoints.Any(p => p.SmesterId == semesterId)
                 || s.Points.Any(p => p.SmesterId == semesterId);
    }

    private static Expression<Func<Student, bool>> BuildEventFilter(Guid eventId)
    {
        return s => s.StudentClassEventsPoints.Any(p => p.EventId == eventId)
                 || s.Points.Any(p => p.EventId == eventId);
    }

    private static Expression<Func<Student, bool>> BuildTeacherFilter(Guid teacherId)
    {
        return s => s.StudentHadiths.Any(h => h.TeacherId == teacherId)
                 || s.StudentQuraanPages.Any(q => q.TeacherId == teacherId)
                 || s.Points.Any(p => p.GivenByTeacherId == teacherId);
    }
}
