using AlAshmar.Application.Common;
using AlAshmar.Application.DTOs;
using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Repos;
using AlAshmar.Domain.Commons;
using AlAshmar.Domain.Entities.Academic;
using AlAshmar.Domain.Entities.Common;
using AlAshmar.Domain.Entities.Students;
using AlAshmar.Domain.Entities.Teachers;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Application.Services.Domain;

/// <summary>
/// Filter parameters for teacher queries.
/// </summary>
public record TeacherFilterParameters(
    int? PageNumber = null,
    int? PageSize = null,
    Guid? ClassId = null,
    Guid? SemesterId = null,
    Guid? EventId = null
);

/// <summary>
/// Filter parameters for attendance queries.
/// </summary>
public record AttendanceFilterParameters(
    DateTime? FromDate = null,
    DateTime? ToDate = null
);

/// <summary>
/// Filter parameters for points queries.
/// </summary>
public record PointsFilterParameters(
    Guid? SemesterId = null
);

/// <summary>
/// Parameters for assigning a teacher to a class.
/// </summary>
public record ClassAssignmentParameters(
    Guid ClassId,
    bool IsMainTeacher = false
);

/// <summary>
/// Parameters for contact info operations.
/// </summary>
public record ContactInfoParameters(
    string Number,
    string? Email = null,
    bool IsActive = true
);

/// <summary>
/// Parameters for search operations.
/// </summary>
public record SearchParameters(
    string Query,
    int? PageNumber = 1,
    int? PageSize = 10
);

/// <summary>
/// Service interface for teacher management operations.
/// </summary>
public interface ITeacherManagementService
{
    Task<Result<List<AlAshmar.Application.DTOs.Domain.TeacherDto>>> GetAllFilteredAsync(
        TeacherFilterParameters filter,
        CancellationToken cancellationToken = default);

    Task<Result<AlAshmar.Application.DTOs.Domain.TeacherDto?>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<AlAshmar.Application.DTOs.Domain.TeacherDto>> CreateAsync(
        string name,
        string fatherName,
        string motherName,
        string nationalityNumber,
        string? email,
        string userName,
        string password,
        CancellationToken cancellationToken = default);

    Task<Result<AlAshmar.Application.DTOs.Domain.TeacherDto>> UpdateAsync(
        Guid id,
        string name,
        string fatherName,
        string motherName,
        string nationalityNumber,
        string? email,
        CancellationToken cancellationToken = default);

    Task<Result<Success>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<List<ClassTeacherEnrollmentDto>>> GetClassEnrollmentsAsync(
        Guid teacherId,
        CancellationToken cancellationToken = default);

    Task<Result<List<TeacherAttencanceDto>>> GetAttendanceRecordsAsync(
        Guid teacherId,
        AttendanceFilterParameters filter,
        CancellationToken cancellationToken = default);

    Task<Result<List<PointDto>>> GetPointsGivenAsync(
        Guid teacherId,
        PointsFilterParameters filter,
        CancellationToken cancellationToken = default);

    Task<Result<Success>> AddAttachmentAsync(
        Guid teacherId,
        string path,
        string type,
        string safeName,
        string originalName,
        Guid? extensionId = null,
        CancellationToken cancellationToken = default);

    Task<Result<List<TeacherAttachmentDto>>> GetAttachmentsAsync(
        Guid teacherId,
        CancellationToken cancellationToken = default);

    Task<Result<Success>> AddContactInfoAsync(
        Guid teacherId,
        ContactInfoParameters parameters,
        CancellationToken cancellationToken = default);

    Task<Result<List<TeacherContactInfoDto>>> GetContactInfosAsync(
        Guid teacherId,
        CancellationToken cancellationToken = default);

    Task<Result<Success>> RemoveContactInfoAsync(
        Guid teacherId,
        Guid contactInfoId,
        CancellationToken cancellationToken = default);

    Task<Result<Success>> AssignToClassAsync(
        Guid teacherId,
        ClassAssignmentParameters parameters,
        CancellationToken cancellationToken = default);

    Task<Result<Success>> RemoveFromClassAsync(
        Guid teacherId,
        Guid classId,
        CancellationToken cancellationToken = default);

    Task<Result<Success>> SetAsMainTeacherAsync(
        Guid teacherId,
        Guid classId,
        CancellationToken cancellationToken = default);

    Task<Result<TeacherStatisticsDto>> GetStatisticsAsync(
        Guid teacherId,
        PointsFilterParameters? filter = null,
        CancellationToken cancellationToken = default);

    Task<Result<List<AlAshmar.Application.DTOs.Domain.TeacherDto>>> SearchByNameAsync(
        SearchParameters parameters,
        CancellationToken cancellationToken = default);

    Task<Result<List<StudentDto>>> GetStudentsAsync(
        Guid teacherId,
        Guid? classId = null,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Service implementation for teacher management operations.
/// </summary>
public class TeacherManagementService : ITeacherManagementService
{
    private readonly IRepositoryBase<Teacher, Guid> _teacherRepository;
    private readonly IRepositoryBase<TeacherAttencance, Guid> _attendanceRepository;
    private readonly IRepositoryBase<ClassTeacherEnrollment, Guid> _enrollmentRepository;
    private readonly IRepositoryBase<Point, Guid> _pointRepository;
    private readonly IRepositoryBase<Attacment, Guid> _attachmentRepository;
    private readonly IRepositoryBase<ContactInfo, Guid> _contactInfoRepository;
    private readonly IRepositoryBase<ClassStudentEnrollment, Guid> _classStudentEnrollmentRepository;

    public TeacherManagementService(
        IRepositoryBase<Teacher, Guid> teacherRepository,
        IRepositoryBase<TeacherAttencance, Guid> attendanceRepository,
        IRepositoryBase<ClassTeacherEnrollment, Guid> enrollmentRepository,
        IRepositoryBase<Point, Guid> pointRepository,
        IRepositoryBase<Attacment, Guid> attachmentRepository,
        IRepositoryBase<ContactInfo, Guid> contactInfoRepository,
        IRepositoryBase<ClassStudentEnrollment, Guid> classStudentEnrollmentRepository)
    {
        _teacherRepository = teacherRepository;
        _attendanceRepository = attendanceRepository;
        _enrollmentRepository = enrollmentRepository;
        _pointRepository = pointRepository;
        _attachmentRepository = attachmentRepository;
        _contactInfoRepository = contactInfoRepository;
        _classStudentEnrollmentRepository = classStudentEnrollmentRepository;
    }

    public async Task<Result<List<AlAshmar.Application.DTOs.Domain.TeacherDto>>> GetAllFilteredAsync(
        TeacherFilterParameters filter,
        CancellationToken cancellationToken = default)
    {
        var filterExpression = BuildFilterExpression(filter.ClassId, filter.SemesterId, filter.EventId);

        Func<IQueryable<Teacher>, IQueryable<Teacher>> transform = q => q
            .Include(t => t.RelatedUser)
            .Include(t => t.TeacherContactInfos).ThenInclude(tc => tc.ContactInfo)
            .Include(t => t.TeacherAttachments).ThenInclude(ta => ta.Attachment)
            .Include(t => t.ClassTeacherEnrollments)
            .Include(t => t.GivenPoints).ThenInclude(p => p.Category);

        List<Teacher> teachers;

        if (filter.PageNumber.HasValue && filter.PageSize.HasValue)
        {
            var pagedResult = await _teacherRepository.GetPagedAsync(
                filter.PageNumber.Value,
                filter.PageSize.Value,
                filterExpression,
                transform
            );

            if (pagedResult.IsError)
                return pagedResult.Errors;

            teachers = [.. pagedResult.Value!.Items];
        }
        else
        {
            var result = await _teacherRepository.GetAllAsync(filterExpression, transform);

            if (result.IsError)
                return result.Errors;

            teachers = [.. result.Value!];
        }

        var teacherDtos = teachers.Select(t => new AlAshmar.Application.DTOs.Domain.TeacherDto(
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

    public async Task<Result<AlAshmar.Application.DTOs.Domain.TeacherDto?>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var teacher = await _teacherRepository.GetByIdAsync(id);

        if (teacher.Value == null)
            return new Error("404", "Teacher not found", ErrorKind.NotFound);

        return new AlAshmar.Application.DTOs.Domain.TeacherDto(
            teacher.Value.Id,
            teacher.Value.Name,
            teacher.Value.FatherName,
            teacher.Value.MotherName,
            teacher.Value.NationalityNumber,
            teacher.Value.Email,
            teacher.Value.UserId,
            null,
            [],
            [],
            []
        );
    }

    public async Task<Result<AlAshmar.Application.DTOs.Domain.TeacherDto>> CreateAsync(
        string name,
        string fatherName,
        string motherName,
        string? nationalityNumber,
        string? email,
        string userName,
        string password,
        CancellationToken cancellationToken = default)
    {
        var teacher = Teacher.Create(
            name,
            fatherName,
            motherName,
            nationalityNumber,
            email,
            userName,
            PasswordHasher.Hash(password)
        );

        var addResult = await _teacherRepository.AddAsync(teacher);
        if (addResult.IsError)
            return addResult.Errors;

        return new AlAshmar.Application.DTOs.Domain.TeacherDto(
            teacher.Id,
            teacher.Name,
            teacher.FatherName,
            teacher.MotherName,
            teacher.NationalityNumber,
            teacher.Email,
            teacher.UserId,
            null,
            [],
            [],
            []
        );
    }

    public async Task<Result<AlAshmar.Application.DTOs.Domain.TeacherDto>> UpdateAsync(
        Guid id,
        string name,
        string fatherName,
        string motherName,
        string? nationalityNumber,
        string? email,
        CancellationToken cancellationToken = default)
    {
        var teacherResult = await _teacherRepository.GetByIdAsync(id);
        if (teacherResult.Value == null)
            return new Error("404", "Teacher not found", ErrorKind.NotFound);

        var teacher = teacherResult.Value;
        teacher.UpdateBasicInfo(
            name,
            fatherName,
            motherName,
            nationalityNumber,
            email
        );

        var updateResult = await _teacherRepository.UpdateAsync(teacher);
        if (updateResult.IsError)
            return updateResult.Errors;

        return new AlAshmar.Application.DTOs.Domain.TeacherDto(
            teacher.Id,
            teacher.Name,
            teacher.FatherName,
            teacher.MotherName,
            teacher.NationalityNumber,
            teacher.Email,
            teacher.UserId,
            null,
            [],
            [],
            []
        );
    }

    public async Task<Result<Success>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var teacher = await _teacherRepository.GetByIdAsync(id);
        if (teacher.Value == null)
            return new Error("404", "Teacher not found", ErrorKind.NotFound);

        var result = await _teacherRepository.RemoveAsync(t => t.Id == id);
        if (result.IsError)
            return result.Errors;

        return Result.Success;
    }

    public async Task<Result<List<ClassTeacherEnrollmentDto>>> GetClassEnrollmentsAsync(
        Guid teacherId,
        CancellationToken cancellationToken = default)
    {
        var enrollments = await _enrollmentRepository.GetAllAsync(
            e => e.TeacherId == teacherId);

        if (enrollments.IsError) return enrollments.Errors;

        var enrollmentDtos = enrollments.Value
            .Select(e => new ClassTeacherEnrollmentDto(
                e.Id, e.TeacherId, null, e.IsMainTeacher, e.ClassId)).ToList();

        return enrollmentDtos;
    }

    public async Task<Result<List<TeacherAttencanceDto>>> GetAttendanceRecordsAsync(
        Guid teacherId,
        AttendanceFilterParameters filter,
        CancellationToken cancellationToken = default)
    {
        var attendances = await _attendanceRepository.GetAllAsync(
            a => a.ClassTeacherId == teacherId &&
                 (!filter.FromDate.HasValue || a.StartDate >= filter.FromDate.Value) &&
                 (!filter.ToDate.HasValue || a.EndDate <= filter.ToDate.Value));

        if (attendances.IsError) return attendances.Errors;

        var attendanceDtos = attendances.Value
            .Select(a => new TeacherAttencanceDto(
                a.Id, a.StartDate, a.EndDate, a.ClassTeacherId)).ToList();

        return attendanceDtos;
    }

    public async Task<Result<List<PointDto>>> GetPointsGivenAsync(
        Guid teacherId,
        PointsFilterParameters filter,
        CancellationToken cancellationToken = default)
    {
        var pointsQuery = await _pointRepository.GetAllAsync(
            p => p.GivenByTeacherId == teacherId &&
                 (!filter.SemesterId.HasValue || p.SmesterId == filter.SemesterId.Value),
            q => q.Include(p => p.Category));

        if (pointsQuery.IsError) return pointsQuery.Errors;

        var pointDtos = pointsQuery.Value
            .Select(p => new PointDto(
                p.Id, p.StudentId, p.EventId, p.ClassId, p.SmesterId,
                p.PointValue, p.CategoryId,
                p.Category != null ? new PointCategoryDto(p.Category.Id, p.Category.Type) : null,
                p.GivenByTeacherId)).ToList();

        return pointDtos;
    }

    public async Task<Result<Success>> AddAttachmentAsync(
        Guid teacherId,
        string path,
        string type,
        string safeName,
        string originalName,
        Guid? extensionId = null,
        CancellationToken cancellationToken = default)
    {
        var teacher = await _teacherRepository.GetByIdAsync(teacherId);
        if (teacher.Value == null)
            return new Error("404", "Teacher not found", ErrorKind.NotFound);

        var attachment = new Attacment
        {
            Path = path,
            Type = type,
            SafeName = safeName,
            OriginalName = originalName,
            ExtentionId = extensionId
        };

        var addResult = await _attachmentRepository.AddAsync(attachment);
        if (addResult.IsError)
            return addResult.Errors;

        var teacherAttachment = new TeacherAttachment
        {
            TeacherId = teacherId,
            AttachmentId = attachment.Id
        };

        teacher.Value.TeacherAttachments.Add(teacherAttachment);

        var updateResult = await _teacherRepository.UpdateAsync(teacher.Value);
        if (updateResult.IsError)
            return updateResult.Errors;

        return Result.Success;
    }

    public async Task<Result<List<TeacherAttachmentDto>>> GetAttachmentsAsync(
        Guid teacherId,
        CancellationToken cancellationToken = default)
    {
        var teacher = await _teacherRepository.GetAsync(
            t => t.Id == teacherId,
            q => q.Include(t => t.TeacherAttachments).ThenInclude(ta => ta.Attachment));

        if (teacher.IsError) return teacher.Errors;
        if (teacher.Value == null)
            return new Error("404", "Teacher not found", ErrorKind.NotFound);

        var attachmentDtos = teacher.Value.TeacherAttachments
            .Select(ta => new TeacherAttachmentDto(
                ta.TeacherId,
                ta.AttachmentId,
                null,
                ta.Attachment != null ? new AttacmentDto(
                    ta.Attachment.Id,
                    ta.Attachment.Path,
                    ta.Attachment.Type,
                    ta.Attachment.SafeName,
                    ta.Attachment.OriginalName,
                    ta.Attachment.ExtentionId,
                    null) : null)).ToList();

        return attachmentDtos;
    }

    private static System.Linq.Expressions.Expression<Func<Teacher, bool>>? BuildFilterExpression(
        Guid? classId,
        Guid? semesterId,
        Guid? eventId)
    {
        var expressions = new List<System.Linq.Expressions.Expression<Func<Teacher, bool>>>();

        if (classId.HasValue)
        {
            expressions.Add(BuildClassFilter(classId.Value));
        }

        if (semesterId.HasValue)
        {
            expressions.Add(BuildSemesterFilter(semesterId.Value));
        }

        if (eventId.HasValue)
        {
            expressions.Add(BuildEventFilter(eventId.Value));
        }

        return expressions.Count > 0 ? ExpressionBuilderLib.src.Core.ExpressionCombiner.OrAll(expressions.ToArray()) : null;
    }

    private static System.Linq.Expressions.Expression<Func<Teacher, bool>> BuildClassFilter(Guid classId)
    {
        return t => t.ClassTeacherEnrollments.Any(cte => cte.ClassId == classId);
    }

    private static System.Linq.Expressions.Expression<Func<Teacher, bool>> BuildSemesterFilter(Guid semesterId)
    {
        return t => t.GivenPoints.Any(p => p.SmesterId == semesterId);
    }

    private static System.Linq.Expressions.Expression<Func<Teacher, bool>> BuildEventFilter(Guid eventId)
    {
        return t => t.GivenPoints.Any(p => p.EventId == eventId);
    }

    public async Task<Result<Success>> AddContactInfoAsync(
        Guid teacherId,
        ContactInfoParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var teacher = await _teacherRepository.GetByIdAsync(teacherId);
        if (teacher.Value == null)
            return new Error("404", "Teacher not found", ErrorKind.NotFound);

        var contactInfo = new ContactInfo
        {
            Number = parameters.Number,
            Email = parameters.Email,
            IsActive = parameters.IsActive
        };

        var addResult = await _contactInfoRepository.AddAsync(contactInfo);
        if (addResult.IsError)
            return addResult.Errors;

        var teacherContactInfo = new TeacherContactInfo
        {
            TeacherId = teacherId,
            ContactInfoId = contactInfo.Id
        };

        teacher.Value.TeacherContactInfos.Add(teacherContactInfo);

        var updateResult = await _teacherRepository.UpdateAsync(teacher.Value);
        if (updateResult.IsError)
            return updateResult.Errors;

        return Result.Success;
    }

    public async Task<Result<List<TeacherContactInfoDto>>> GetContactInfosAsync(
        Guid teacherId,
        CancellationToken cancellationToken = default)
    {
        var teacher = await _teacherRepository.GetAsync(
            t => t.Id == teacherId,
            q => q.Include(t => t.TeacherContactInfos).ThenInclude(tc => tc.ContactInfo));

        if (teacher.IsError) return teacher.Errors;
        if (teacher.Value == null)
            return new Error("404", "Teacher not found", ErrorKind.NotFound);

        var contactInfoDtos = teacher.Value.TeacherContactInfos
            .Select(tc => new TeacherContactInfoDto(
                tc.TeacherId,
                tc.ContactInfoId,
                null,
                tc.ContactInfo != null ? new ContactInfoDto(
                    tc.ContactInfo.Id,
                    tc.ContactInfo.Number,
                    tc.ContactInfo.Email,
                    tc.ContactInfo.IsActive) : null)).ToList();

        return contactInfoDtos;
    }

    public async Task<Result<Success>> RemoveContactInfoAsync(
        Guid teacherId,
        Guid contactInfoId,
        CancellationToken cancellationToken = default)
    {
        var teacher = await _teacherRepository.GetAsync(
            t => t.Id == teacherId,
            q => q.Include(t => t.TeacherContactInfos));

        if (teacher.IsError) return teacher.Errors;
        if (teacher.Value == null)
            return new Error("404", "Teacher not found", ErrorKind.NotFound);

        var teacherContactInfo = teacher.Value.TeacherContactInfos
            .FirstOrDefault(tc => tc.ContactInfoId == contactInfoId);

        if (teacherContactInfo == null)
            return new Error("404", "Teacher contact info not found", ErrorKind.NotFound);

        teacher.Value.TeacherContactInfos.Remove(teacherContactInfo);

        var updateResult = await _teacherRepository.UpdateAsync(teacher.Value);
        if (updateResult.IsError)
            return updateResult.Errors;

        return Result.Success;
    }

    public async Task<Result<Success>> AssignToClassAsync(
        Guid teacherId,
        ClassAssignmentParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var existingEnrollment = await _enrollmentRepository.GetAllAsync(
            e => e.TeacherId == teacherId && e.ClassId == parameters.ClassId);

        if (existingEnrollment.IsError) return existingEnrollment.Errors;
        if (existingEnrollment.Value.Any())
            return new Error("409", "Teacher is already assigned to this class", ErrorKind.Conflict);

        var enrollment = new ClassTeacherEnrollment
        {
            TeacherId = teacherId,
            ClassId = parameters.ClassId,
            IsMainTeacher = parameters.IsMainTeacher
        };

        var addResult = await _enrollmentRepository.AddAsync(enrollment);
        if (addResult.IsError)
            return addResult.Errors;

        return Result.Success;
    }

    public async Task<Result<Success>> RemoveFromClassAsync(
        Guid teacherId,
        Guid classId,
        CancellationToken cancellationToken = default)
    {
        var enrollment = await _enrollmentRepository.GetAsync(
            e => e.TeacherId == teacherId && e.ClassId == classId);

        if (enrollment.IsError) return enrollment.Errors;
        if (enrollment.Value == null)
            return new Error("404", "Teacher class enrollment not found", ErrorKind.NotFound);

        var deleteResult = await _enrollmentRepository.RemoveAsync(
            e => e.TeacherId == teacherId && e.ClassId == classId);

        if (deleteResult.IsError)
            return deleteResult.Errors;

        return Result.Success;
    }

    public async Task<Result<Success>> SetAsMainTeacherAsync(
        Guid teacherId,
        Guid classId,
        CancellationToken cancellationToken = default)
    {
        var enrollment = await _enrollmentRepository.GetAsync(
            e => e.TeacherId == teacherId && e.ClassId == classId);

        if (enrollment.IsError) return enrollment.Errors;
        if (enrollment.Value == null)
            return new Error("404", "Teacher class enrollment not found", ErrorKind.NotFound);

        enrollment.Value.IsMainTeacher = true;

        var updateResult = await _enrollmentRepository.UpdateAsync(enrollment.Value);
        if (updateResult.IsError)
            return updateResult.Errors;

        return Result.Success;
    }

    public async Task<Result<TeacherStatisticsDto>> GetStatisticsAsync(
        Guid teacherId,
        PointsFilterParameters? filter = null,
        CancellationToken cancellationToken = default)
    {
        var teacher = await _teacherRepository.GetAsync(
            t => t.Id == teacherId,
            q => q.Include(t => t.ClassTeacherEnrollments)
                  .Include(t => t.GivenPoints)
                  .Include(t => t.TeacherContactInfos));

        if (teacher.IsError) return teacher.Errors;
        if (teacher.Value == null)
            return new Error("404", "Teacher not found", ErrorKind.NotFound);

        var totalClasses = teacher.Value.ClassTeacherEnrollments.Count;
        var isMainTeacherCount = teacher.Value.ClassTeacherEnrollments.Count(c => c.IsMainTeacher);

        var pointsQuery = teacher.Value.GivenPoints.AsQueryable();
        if (filter?.SemesterId.HasValue == true)
        {
            pointsQuery = pointsQuery.Where(p => p.SmesterId == filter.SemesterId.Value);
        }
        var totalPointsGiven = pointsQuery.Sum(p => p.PointValue);
        var totalPointsCount = filter?.SemesterId.HasValue == true
            ? teacher.Value.GivenPoints.Count(p => p.SmesterId == filter.SemesterId.Value)
            : teacher.Value.GivenPoints.Count;

        var totalContactInfos = teacher.Value.TeacherContactInfos.Count;

        return new TeacherStatisticsDto(
            teacherId,
            totalClasses,
            isMainTeacherCount,
            totalPointsGiven,
            totalPointsCount,
            totalContactInfos
        );
    }

    public async Task<Result<List<AlAshmar.Application.DTOs.Domain.TeacherDto>>> SearchByNameAsync(
        SearchParameters parameters,
        CancellationToken cancellationToken = default)
    {
        System.Linq.Expressions.Expression<Func<Teacher, bool>> filterExpression = t =>
            t.Name.Contains(parameters.Query) ||
            t.FatherName.Contains(parameters.Query) ||
            t.MotherName.Contains(parameters.Query);

        var result = await _teacherRepository.GetPagedAsync(
            parameters.PageNumber ?? 1,
            parameters.PageSize ?? 10,
            filterExpression);

        if (result.IsError)
            return result.Errors;

        var teacherDtos = result.Value!.Items.Select(t => new AlAshmar.Application.DTOs.Domain.TeacherDto(
            t.Id,
            t.Name,
            t.FatherName,
            t.MotherName,
            t.NationalityNumber,
            t.Email,
            t.UserId,
            null,
            [],
            [],
            []
        )).ToList();

        return teacherDtos;
    }

    public async Task<Result<List<StudentDto>>> GetStudentsAsync(
        Guid teacherId,
        Guid? classId = null,
        CancellationToken cancellationToken = default)
    {
        var enrollments = await _enrollmentRepository.GetAllAsync(
            e => e.TeacherId == teacherId);

        if (enrollments.IsError) return enrollments.Errors;

        var classIds = enrollments.Value.Select(e => e.ClassId).ToList();
        if (classId.HasValue)
        {
            classIds = classIds.Where(c => c == classId.Value).ToList();
        }

        if (!classIds.Any())
            return new List<StudentDto>();

        var studentEnrollments = await _classStudentEnrollmentRepository.GetAllAsync(
            e => classIds.Contains(e.ClassId),
            q => q.Include(e => e.Student));

        if (studentEnrollments.IsError) return studentEnrollments.Errors;

        var studentDtos = studentEnrollments.Value
            .Select(e => e.Student)
            .Where(s => s != null)
            .Select(s => new StudentDto(
                s!.Id,
                s.Name,
                s.FatherName,
                s.MotherName,
                s.NationalityNumber,
                s.Email,
                s.UserId,
                null,
                [],
                [],
                [],
                [],
                [],
                []
            ))
            .DistinctBy(s => s.Id)
            .ToList();

        return studentDtos;
    }
}
