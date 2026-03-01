using AlAshmar.Application.Common;
using AlAshmar.Domain.Commons;
using AlAshmar.Domain.Entities.Students;
using AlAshmar.Application.Repos;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ExpressionBuilderLib.src.Core;
using ExpressionBuilderLib.src.Core.Enums;
using AlAshmar.Application.DTOs.Domain;

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
) : IQuery<Result<List<StudentDto>>>;

public class GetAllStudentsFilteredHandler(IRepositoryBase<Student, Guid> repository) : IQueryHandler<GetAllStudentsFilteredQuery, Result<List<StudentDto>>>
{
    public async Task<Result<List<StudentDto>>> Handle(GetAllStudentsFilteredQuery query, CancellationToken cancellationToken = default)
    {
        var filterExpression = BuildFilterExpression(query);

        Func<IQueryable<Student>, IQueryable<Student>> transform = q => q
            .Include(s => s.User)
            .Include(s => s.StudentContactInfos).ThenInclude(sc => sc.ContactInfo)
            .Include(s => s.StudentAttachments).ThenInclude(sa => sa.Attachment)
            .Include(s => s.StudentHadiths).ThenInclude(h => h.Hadith).ThenInclude(h => h.Book)
            .Include(s => s.StudentQuraanPages)
            .Include(s => s.StudentClassEventsPoints)
            .Include(s => s.Points).ThenInclude(p => p.Category);

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

        var studentDtos = students.Select(s => new StudentDto(
            s.Id,
            s.Name,
            s.FatherName,
            s.MotherName,
            s.NationalityNumber,
            s.Email,
            s.UserId,
            s.User != null ? new UserDto(s.User.Id, s.User.UserName, s.User.RoleId, null) : null,
            s.StudentContactInfos.Select(sc => new StudentContactInfoDto(
                sc.StudentId, 
                sc.ContactInfoId, 
                null, 
                sc.ContactInfo != null ? new ContactInfoDto(sc.ContactInfo.Id, sc.ContactInfo.Number, sc.ContactInfo.Email, sc.ContactInfo.IsActive) : null)).ToList(),
            s.StudentAttachments.Select(sa => new StudentAttachmentDto(
                sa.StudentId, 
                sa.AttachmentId, 
                null,
                sa.Attachment != null ? new AttacmentDto(sa.Attachment.Id, sa.Attachment.Path, sa.Attachment.Type, sa.Attachment.SafeName, sa.Attachment.OriginalName, sa.Attachment.ExtentionId, null) : null)).ToList(),
            s.StudentHadiths.Select(h => new StudentHadithDto(h.Id, h.HadithId, h.StudentId, h.TeacherId, h.ClassId, h.MemorizedAt, h.Status, h.Notes)).ToList(),
            s.StudentQuraanPages.Select(q => new StudentQuraanPageDto(q.Id, q.PageNumber, q.StudentId, q.TeacherId, q.ClassId, q.MemorizedAt, q.Status, q.Notes)).ToList(),
            s.StudentClassEventsPoints.Select(p => new StudentClassEventsPointDto(p.Id, p.StudentId, p.ClassId, p.SmesterId, p.EventId, p.QuranPoints, p.HadithPoints, p.AttendancePoints, p.BehaviorPoints, p.TotalPoints)).ToList()
        )).ToList();

        return studentDtos;
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
