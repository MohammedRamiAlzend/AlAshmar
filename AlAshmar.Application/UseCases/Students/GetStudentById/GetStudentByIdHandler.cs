using AlAshmar.Application.Common;
using AlAshmar.Application.DTOs;
using AlAshmar.Domain.Commons;
using AlAshmar.Domain.Entities.Students;
using AlAshmar.Application.Repos;
using AlAshmar.Application.Repos.Includes;
using AlAshmar.Application.DTOs.Domain;
using MediatR;

namespace AlAshmar.Application.UseCases.Students.GetStudentById;

public record GetStudentByIdQuery(Guid Id) : IQuery<Result<StudentDetailDto?>>;

public class GetStudentByIdHandler : IRequestHandler<GetStudentByIdQuery, Result<StudentDetailDto?>>
{
    private readonly IRepositoryBase<Student, Guid> _repository;

    public GetStudentByIdHandler(IRepositoryBase<Student, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<Result<StudentDetailDto?>> Handle(GetStudentByIdQuery query, CancellationToken cancellationToken = default)
    {
        var studentResult = await _repository.GetAllAsync(
            s => s.Id == query.Id,
            StudentIncludes.Instance.Apply()
        );

        if (studentResult.IsError)
            return studentResult.Errors;

        var student = studentResult.Value.FirstOrDefault();

        if (student == null)
            return new Error("404", "Student not found", ErrorKind.NotFound);

        return MapToDetailDto(student);
    }

    private static StudentDetailDto MapToDetailDto(Student student)
    {
        return new StudentDetailDto(
            student.Id,
            student.Name,
            student.FatherName,
            student.MotherName,
            student.NationalityNumber,
            student.Email,
            student.UserId,
            student.User != null ? MapToUserDetailDto(student.User) : null,
            student.StudentContactInfos.Select(sc => new StudentContactInfoDetailDto(
                sc.StudentId,
                sc.ContactInfoId,
                sc.ContactInfo?.Number ?? string.Empty,
                sc.ContactInfo?.Email,
                sc.ContactInfo?.IsActive ?? false
            )).ToList(),
            student.StudentAttachments.Select(sa => new StudentAttachmentDetailDto(
                sa.StudentId,
                sa.AttachmentId,
                new AttachmentDetailDto(
                    sa.Attachment!.Id,
                    sa.Attachment.Path,
                    sa.Attachment.Type,
                    sa.Attachment.SafeName,
                    sa.Attachment.OriginalName,
                    sa.Attachment.ExtentionId,
                    sa.Attachment.Extention != null ? new AllowableExtentionDto(sa.Attachment.Extention.Id, sa.Attachment.Extention.ExtName) : null
                )
            )).ToList(),
            student.StudentHadiths.Select(h => new StudentHadithDetailDto(
                h.Id,
                h.HadithId,
                h.StudentId,
                h.TeacherId,
                h.ClassId,
                h.MemorizedAt,
                h.Status,
                h.Notes,
                h.Hadith != null ? new HadithSummaryDto(h.Hadith.Id, h.Hadith.Text, h.Hadith.BookId, h.Hadith.Book?.Name, h.Hadith.Chapter) : null
            )).ToList(),
            student.StudentQuraanPages.Select(q => new StudentQuraanPageDetailDto(
                q.Id,
                q.PageNumber,
                q.StudentId,
                q.TeacherId,
                q.ClassId,
                q.MemorizedAt,
                q.Status,
                q.Notes
            )).ToList(),
            student.StudentClassEventsPoints.Select(p => new StudentClassEventsPointDetailDto(
                p.Id,
                p.StudentId,
                p.ClassId,
                p.SmesterId,
                p.EventId,
                p.QuranPoints,
                p.HadithPoints,
                p.AttendancePoints,
                p.BehaviorPoints,
                p.TotalPoints
            )).ToList(),
            student.Points.Select(p => new PointDetailDto(
                p.Id,
                p.StudentId,
                p.EventId,
                p.ClassId,
                p.SmesterId,
                p.PointValue,
                p.CategoryId,
                p.Category != null ? new PointCategorySummaryDto(p.Category.Id, p.Category.Type) : null,
                p.GivenByTeacherId
            )).ToList()
        );
    }

    private static UserDetailDto MapToUserDetailDto(Domain.Entities.Users.User user)
    {
        return new UserDetailDto(
            user.Id,
            user.UserName,
            user.RoleId,
            user.RoleId != null ? new RoleSummaryDto(user.RoleId, "Student") : null
        );
    }
}
