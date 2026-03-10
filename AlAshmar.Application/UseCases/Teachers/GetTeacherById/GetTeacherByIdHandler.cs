using AlAshmar.Application.Common;
using AlAshmar.Domain.Commons;
using AlAshmar.Domain.Entities.Teachers;
using AlAshmar.Application.Repos;
using AlAshmar.Application.DTOs.Domain;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace AlAshmar.Application.UseCases.Teachers.GetTeacherById;

public record GetTeacherByIdQuery(Guid Id) : IQuery<Result<TeacherDto?>>;

public class GetTeacherByIdHandler : IRequestHandler<GetTeacherByIdQuery, Result<TeacherDto?>>
{
    private readonly IRepositoryBase<Teacher, Guid> _repository;

    public GetTeacherByIdHandler(IRepositoryBase<Teacher, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<Result<TeacherDto?>> Handle(GetTeacherByIdQuery query, CancellationToken cancellationToken = default)
    {
        var teacherResult = await _repository.GetAllAsync(
            t => t.Id == query.Id,
            q => q.Include(t => t.RelatedUser)
                  .Include(t => t.TeacherContactInfos).ThenInclude(tc => tc.ContactInfo)
                  .Include(t => t.TeacherAttachments).ThenInclude(ta => ta.Attachment)
                  .Include(t => t.ClassTeacherEnrollments)
        );

        if (teacherResult.IsError)
            return teacherResult.Errors;

        var teacher = teacherResult.Value.FirstOrDefault();

        if (teacher == null)
            return new Error("404", "Teacher not found", ErrorKind.NotFound);

        return new TeacherDto(
            teacher.Id,
            teacher.Name,
            teacher.FatherName,
            teacher.MotherName,
            teacher.NationalityNumber,
            teacher.Email,
            teacher.UserId,
            teacher.RelatedUser != null ? new UserDto(teacher.RelatedUser.Id, teacher.RelatedUser.UserName, teacher.RelatedUser.RoleId, null) : null,
            teacher.TeacherContactInfos.Select(tc => new TeacherContactInfoDto(
                tc.TeacherId, tc.ContactInfoId, null,
                tc.ContactInfo != null ? new ContactInfoDto(tc.ContactInfo.Id, tc.ContactInfo.Number, tc.ContactInfo.Email, tc.ContactInfo.IsActive) : null
            )).ToList(),
            teacher.TeacherAttachments.Select(ta => new TeacherAttachmentDto(
                ta.TeacherId, ta.AttachmentId, null,
                ta.Attachment != null ? new AttacmentDto(ta.Attachment.Id, ta.Attachment.Path, ta.Attachment.Type, ta.Attachment.SafeName, ta.Attachment.OriginalName, ta.Attachment.ExtentionId, null) : null
            )).ToList(),
            teacher.ClassTeacherEnrollments.Select(ce => new ClassTeacherEnrollmentDto(
                ce.Id, ce.TeacherId, null, ce.IsMainTeacher, ce.ClassId
            )).ToList()
        );
    }
}
