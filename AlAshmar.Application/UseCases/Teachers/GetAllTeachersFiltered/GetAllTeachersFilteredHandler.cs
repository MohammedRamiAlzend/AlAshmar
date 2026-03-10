using AlAshmar.Application.Common;
using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Domain.Commons;
using AlAshmar.Domain.Entities.Teachers;
using AlAshmar.Application.Repos;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ExpressionBuilderLib.src.Core;
using ExpressionBuilderLib.src.Core.Enums;

namespace AlAshmar.Application.UseCases.Teachers.GetAllTeachersFiltered;

/// <summary>
/// Handler for getting filtered teachers with support for OR operations on filter parameters.
/// </summary>
public class GetAllTeachersFilteredHandler(IRepositoryBase<Teacher, Guid> repository) : IQueryHandler<GetAllTeachersFilteredQuery, Result<List<TeacherDto>>>
{
    public async Task<Result<List<TeacherDto>>> Handle(GetAllTeachersFilteredQuery query, CancellationToken cancellationToken = default)
    {
        var filterExpression = BuildFilterExpression(query);

        Func<IQueryable<Teacher>, IQueryable<Teacher>> transform = q => q
            .Include(t => t.RelatedUser)
            .Include(t => t.TeacherContactInfos).ThenInclude(tc => tc.ContactInfo)
            .Include(t => t.TeacherAttachments).ThenInclude(ta => ta.Attachment)
            .Include(t => t.ClassTeacherEnrollments)
            .Include(t => t.GivenPoints).ThenInclude(p => p.Category);

        List<Teacher> teachers;

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

            teachers = [.. pagedResult.Value!.Items];
        }
        else
        {
            var result = await repository.GetAllAsync(filterExpression, transform);

            if (result.IsError)
                return result.Errors;

            teachers = [.. result.Value!];
        }

        var teacherDtos = teachers.Select(t => new TeacherDto(
            t.Id,
            t.Name,
            t.FatherName,
            t.MotherName,
            t.NationalityNumber,
            t.Email,
            t.UserId,
            t.RelatedUser != null ? new UserDto(t.RelatedUser.Id, t.RelatedUser.UserName, t.RelatedUser.RoleId, null) : null,
            t.TeacherContactInfos.Select(tc => new TeacherContactInfoDto(
                tc.TeacherId,
                tc.ContactInfoId,
                null,
                tc.ContactInfo != null ? new ContactInfoDto(tc.ContactInfo.Id, tc.ContactInfo.Number, tc.ContactInfo.Email, tc.ContactInfo.IsActive) : null)).ToList(),
            t.TeacherAttachments.Select(ta => new TeacherAttachmentDto(
                ta.TeacherId,
                ta.AttachmentId,
                null,
                ta.Attachment != null ? new AttacmentDto(ta.Attachment.Id, ta.Attachment.Path, ta.Attachment.Type, ta.Attachment.SafeName, ta.Attachment.OriginalName, ta.Attachment.ExtentionId, null) : null)).ToList(),
            t.ClassTeacherEnrollments.Select(cte => new ClassTeacherEnrollmentDto(
                cte.Id,
                cte.TeacherId,
                null,
                cte.IsMainTeacher,
                cte.ClassId)).ToList()
        )).ToList();

        return teacherDtos;
    }

    private static Expression<Func<Teacher, bool>>? BuildFilterExpression(GetAllTeachersFilteredQuery query)
    {
        var expressions = new List<Expression<Func<Teacher, bool>>>();

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

        return expressions.Count > 0 ? ExpressionCombiner.OrAll(expressions.ToArray()) : null;
    }

    private static Expression<Func<Teacher, bool>> BuildClassFilter(Guid classId)
    {
        return t => t.ClassTeacherEnrollments.Any(cte => cte.ClassId == classId);
    }

    private static Expression<Func<Teacher, bool>> BuildSemesterFilter(Guid semesterId)
    {
        return t => t.GivenPoints.Any(p => p.SmesterId == semesterId);
    }

    private static Expression<Func<Teacher, bool>> BuildEventFilter(Guid eventId)
    {
        return t => t.GivenPoints.Any(p => p.EventId == eventId);
    }
}
