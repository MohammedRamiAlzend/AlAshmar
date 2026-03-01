using AlAshmar.Application.Common;
using AlAshmar.Domain.Commons;
using AlAshmar.Domain.Entities.Students;
using AlAshmar.Application.Repos;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ExpressionBuilderLib.src.Core;
using ExpressionBuilderLib.src.Core.Enums;

namespace AlAshmar.Application.UseCases.Students.GetAllStudentsFiltered;

public class GetAllStudentsFilteredHandler : IQueryHandler<GetAllStudentsFilteredQuery, Result<List<DTOs.Domain.StudentDto>>>
{
    private readonly IRepositoryBase<Student, Guid> _repository;

    public GetAllStudentsFilteredHandler(IRepositoryBase<Student, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<DTOs.Domain.StudentDto>>> Handle(GetAllStudentsFilteredQuery query, CancellationToken cancellationToken = default)
    {
        // Build the filter expression using ExpressionBuilderLib
        // Only non-null parameters are included (OR operation support)
        var filterExpression = BuildFilterExpression(query);

        // Create transform function to include related data
        Func<IQueryable<Student>, IQueryable<Student>> transform = q => q
            .Include(s => s.User)
            .Include(s => s.StudentContactInfos).ThenInclude(sc => sc.ContactInfo)
            .Include(s => s.StudentAttachments).ThenInclude(sa => sa.Attachment)
            .Include(s => s.StudentHadiths).ThenInclude(h => h.Hadith).ThenInclude(h => h.Book)
            .Include(s => s.StudentQuraanPages)
            .Include(s => s.StudentClassEventsPoints)
            .Include(s => s.Points).ThenInclude(p => p.Category);

        List<Student> students;

        // Apply pagination if parameters are provided
        if (query.PageNumber.HasValue && query.PageSize.HasValue)
        {
            var pageNumber = query.PageNumber.Value;
            var pageSize = query.PageSize.Value;

            var pagedResult = await _repository.GetPagedAsync(
                pageNumber,
                pageSize,
                filterExpression,
                transform
            );

            if (pagedResult.IsError)
                return pagedResult.Errors;

            students = pagedResult.Value.Items.ToList();
        }
        else
        {
            // No pagination - return all filtered results
            var result = await _repository.GetAllAsync(filterExpression, transform);

            if (result.IsError)
                return result.Errors;

            students = result.Value.ToList();
        }

        // Map to DTOs
        var studentDtos = students.Select(s => new DTOs.Domain.StudentDto(
            s.Id,
            s.Name,
            s.FatherName,
            s.MotherName,
            s.NationalityNumber,
            s.Email,
            s.UserId,
            s.User != null ? new DTOs.Domain.UserDto(s.User.Id, s.User.UserName, s.User.RoleId, null) : null,
            s.StudentContactInfos.Select(sc => new DTOs.Domain.StudentContactInfoDto(
                sc.StudentId, 
                sc.ContactInfoId, 
                null, 
                sc.ContactInfo != null ? new DTOs.Domain.ContactInfoDto(sc.ContactInfo.Id, sc.ContactInfo.Number, sc.ContactInfo.Email, sc.ContactInfo.IsActive) : null)).ToList(),
            s.StudentAttachments.Select(sa => new DTOs.Domain.StudentAttachmentDto(
                sa.StudentId, 
                sa.AttachmentId, 
                null,
                sa.Attachment != null ? new DTOs.Domain.AttacmentDto(sa.Attachment.Id, sa.Attachment.Path, sa.Attachment.Type, sa.Attachment.SafeName, sa.Attachment.OriginalName, sa.Attachment.ExtentionId, null) : null)).ToList(),
            s.StudentHadiths.Select(h => new DTOs.Domain.StudentHadithDto(h.Id, h.HadithId, h.StudentId, h.TeacherId, h.ClassId, h.MemorizedAt, h.Status, h.Notes)).ToList(),
            s.StudentQuraanPages.Select(q => new DTOs.Domain.StudentQuraanPageDto(q.Id, q.PageNumber, q.StudentId, q.TeacherId, q.ClassId, q.MemorizedAt, q.Status, q.Notes)).ToList(),
            s.StudentClassEventsPoints.Select(p => new DTOs.Domain.StudentClassEventsPointDto(p.Id, p.StudentId, p.ClassId, p.SmesterId, p.EventId, p.QuranPoints, p.HadithPoints, p.AttendancePoints, p.BehaviorPoints, p.TotalPoints)).ToList()
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
