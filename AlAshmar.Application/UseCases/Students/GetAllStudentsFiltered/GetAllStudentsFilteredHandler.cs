using AlAshmar.Application.Common;
using AlAshmar.Domain.Commons;
using AlAshmar.Domain.Entities.Students;
using AlAshmar.Application.Repos;
using AlAshmar.Application.Repos.Includes;
using System.Linq.Expressions;
using ExpressionBuilderLib.src.Core;
using ExpressionBuilderLib.src.Core.Enums;
using AlAshmar.Application.DTOs.Domain;
using MediatR;

namespace AlAshmar.Application.UseCases.Students.GetAllStudentsFiltered;

/// <summary>
/// Query for getting filtered students with support for OR operations on filter parameters.
/// All filter parameters are nullable to support flexible filtering.
/// </summary>
/// <param name="PageNumber">Page number for pagination (1-based)</param>
/// <param name="PageSize">Number of items per page</param>
/// <param name="ClassId">Filter by class ID (nullable </param>
/// <param name="SemesterId">Filter by semester ID (nullable </param>
/// <param name="EventId">Filter by event ID (nullable </param>
/// <param name="TeacherId">Filter by teacher ID (nullable </param>
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
        // Default student role ID (matches Constants.DefaultStudentRoleId)
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
