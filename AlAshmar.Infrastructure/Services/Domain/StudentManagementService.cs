using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Repos;
using AlAshmar.Application.Repos.Includes;
using AlAshmar.Domain.Entities.Academic;
using AlAshmar.Domain.Entities.Common;
using AlAshmar.Domain.Entities.Students;

namespace AlAshmar.Infrastructure.Services.Domain;

public interface IStudentManagementService
{
    Task<Result<List<StudentDto>>> GetAllFilteredAsync(
        int? pageNumber = null,
        int? pageSize = null,
        Guid? classId = null,
        Guid? semesterId = null,
        Guid? eventId = null,
        Guid? teacherId = null,
        CancellationToken cancellationToken = default);

    Task<Result<StudentSummaryDto?>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<StudentSummaryDto>> CreateAsync(
        string name,
        string fatherName,
        string motherName,
        string nationalityNumber,
        string? email,
        string userName,
        string password,
        CancellationToken cancellationToken = default);

    Task<Result<StudentSummaryDto>> UpdateAsync(
        Guid id,
        string name,
        string fatherName,
        string motherName,
        string nationalityNumber,
        string? email,
        CancellationToken cancellationToken = default);

    Task<Result<Success>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<StudentMemorizationProgressDto>> GetMemorizationProgressAsync(
        Guid studentId,
        CancellationToken cancellationToken = default);

    Task<Result<List<StudentAttendanceDto>>> GetAttendanceRecordsAsync(
        Guid studentId,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default);

    Task<Result<List<PointDto>>> GetPointsAsync(
        Guid studentId,
        Guid? semesterId = null,
        CancellationToken cancellationToken = default);

    Task<Result<Success>> EnrollInClassAsync(
        Guid studentId,
        Guid classId,
        CancellationToken cancellationToken = default);

    Task<Result<List<ClassStudentEnrollmentDto>>> GetClassEnrollmentsAsync(
        Guid studentId,
        CancellationToken cancellationToken = default);

    Task<Result<Success>> AddAttachmentAsync(
        Guid studentId,
        string path,
        string type,
        string safeName,
        string originalName,
        Guid? extensionId = null,
        CancellationToken cancellationToken = default);

    Task<Result<List<StudentAttachmentDto>>> GetAttachmentsAsync(
        Guid studentId,
        CancellationToken cancellationToken = default);
}

public class StudentManagementService : IStudentManagementService
{
    private readonly IRepositoryBase<Student, Guid> _studentRepository;
    private readonly IRepositoryBase<StudentHadith, Guid> _hadithRepository;
    private readonly IRepositoryBase<StudentQuraanPage, Guid> _quranRepository;
    private readonly IRepositoryBase<StudentAttendance, Guid> _attendanceRepository;
    private readonly IRepositoryBase<Point, Guid> _pointRepository;
    private readonly IRepositoryBase<StudentClassEventsPoint, Guid> _classEventsRepository;
    private readonly IRepositoryBase<ClassStudentEnrollment, Guid> _enrollmentRepository;
    private readonly IRepositoryBase<Attacment, Guid> _attachmentRepository;

    public StudentManagementService(
        IRepositoryBase<Student, Guid> studentRepository,
        IRepositoryBase<StudentHadith, Guid> hadithRepository,
        IRepositoryBase<StudentQuraanPage, Guid> quranRepository,
        IRepositoryBase<StudentAttendance, Guid> attendanceRepository,
        IRepositoryBase<Point, Guid> pointRepository,
        IRepositoryBase<StudentClassEventsPoint, Guid> classEventsRepository,
        IRepositoryBase<ClassStudentEnrollment, Guid> enrollmentRepository,
        IRepositoryBase<Attacment, Guid> attachmentRepository)
    {
        _studentRepository = studentRepository;
        _hadithRepository = hadithRepository;
        _quranRepository = quranRepository;
        _attendanceRepository = attendanceRepository;
        _pointRepository = pointRepository;
        _classEventsRepository = classEventsRepository;
        _enrollmentRepository = enrollmentRepository;
        _attachmentRepository = attachmentRepository;
    }

    public async Task<Result<List<StudentDto>>> GetAllFilteredAsync(
        int? pageNumber = null,
        int? pageSize = null,
        Guid? classId = null,
        Guid? semesterId = null,
        Guid? eventId = null,
        Guid? teacherId = null,
        CancellationToken cancellationToken = default)
    {
        var filterExpression = BuildFilterExpression(classId, semesterId, eventId, teacherId);

        var transform = StudentIncludes.Full.Apply();

        List<Student> students;

        if (pageNumber.HasValue && pageSize.HasValue)
        {
            var pagedResult = await _studentRepository.GetPagedAsync(
                pageNumber.Value,
                pageSize.Value,
                filterExpression,
                transform
            );

            if (pagedResult.IsError)
                return pagedResult.Errors;

            students = [.. pagedResult.Value!.Items];
        }
        else
        {
            var result = await _studentRepository.GetAllAsync(filterExpression, transform);

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
            s.StudentClassEventsPoints.Select(p => new StudentClassEventsPointDto(p.Id, p.StudentId, p.ClassId, p.SmesterId, p.EventId, p.QuranPoints, p.HadithPoints, p.AttendancePoints, p.BehaviorPoints, p.TotalPoints)).ToList(),
            s.Points.Select(p => new PointDto(p.Id, p.StudentId, p.EventId, p.ClassId, p.SmesterId, p.PointValue, p.CategoryId, p.Category != null ? new PointCategoryDto(p.Category.Id, p.Category.Type) : null, p.GivenByTeacherId)).ToList()
        )).ToList();

        return studentDtos;
    }

    public async Task<Result<StudentSummaryDto?>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var student = await _studentRepository.GetByIdAsync(id);

        if (student.Value == null)
            return ApplicationErrors.StudentNotFound;

        return new StudentSummaryDto(
            student.Value.Id,
            student.Value.Name,
            student.Value.FatherName,
            student.Value.MotherName,
            student.Value.NationalityNumber,
            student.Value.Email,
            student.Value.UserId
        );
    }

    public async Task<Result<StudentSummaryDto>> CreateAsync(
        string name,
        string fatherName,
        string motherName,
        string nationalityNumber,
        string? email,
        string userName,
        string password,
        CancellationToken cancellationToken = default)
    {
        var student = Student.Create(
            name,
            fatherName,
            motherName,
            nationalityNumber,
            email,
            userName,
            password
        );

        var addResult = await _studentRepository.AddAsync(student);
        if (addResult.IsError)
            return addResult.Errors;

        return new StudentSummaryDto(
            student.Id,
            student.Name,
            student.FatherName,
            student.MotherName,
            student.NationalityNumber,
            student.Email,
            student.UserId
        );
    }

    public async Task<Result<StudentSummaryDto>> UpdateAsync(
        Guid id,
        string name,
        string fatherName,
        string motherName,
        string nationalityNumber,
        string? email,
        CancellationToken cancellationToken = default)
    {
        var studentResult = await _studentRepository.GetByIdAsync(id);
        if (studentResult.Value == null)
            return ApplicationErrors.StudentNotFound;

        var student = studentResult.Value;
        student.UpdateBasicInfo(
            name,
            fatherName,
            motherName,
            nationalityNumber,
            email
        );

        var updateResult = await _studentRepository.UpdateAsync(student);
        if (updateResult.IsError)
            return updateResult.Errors;

        return new StudentSummaryDto(
            student.Id,
            student.Name,
            student.FatherName,
            student.MotherName,
            student.NationalityNumber,
            student.Email,
            student.UserId
        );
    }

    public async Task<Result<Success>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var student = await _studentRepository.GetByIdAsync(id);
        if (student.Value == null)
            return ApplicationErrors.StudentNotFound;

        var result = await _studentRepository.RemoveAsync(s => s.Id == id);
        if (result.IsError)
            return result.Errors;

        return Result.Success;
    }

    public async Task<Result<StudentMemorizationProgressDto>> GetMemorizationProgressAsync(
        Guid studentId,
        CancellationToken cancellationToken = default)
    {
        var hadiths = await _hadithRepository.GetAllAsync(
            h => h.StudentId == studentId,
            q => q.Include(h => h.Hadith).ThenInclude(h => h.Book));

        var quranPages = await _quranRepository.GetAllAsync(
            q => q.StudentId == studentId);

        if (hadiths.IsError) return hadiths.Errors;
        if (quranPages.IsError) return quranPages.Errors;

        var hadithDtos = hadiths.Value
            .Select(h => new StudentHadithSummaryDto(
                h.Id, h.HadithId,
                h.Hadith != null ? h.Hadith.Text : null,
                h.Hadith != null && h.Hadith.Book != null ? h.Hadith.Book.Name : null,
                h.Hadith != null ? h.Hadith.Chapter : null,
                h.MemorizedAt, h.Status)).ToList();

        var quranDtos = quranPages.Value
            .Select(q => new StudentQuraanPageSummaryDto(
                q.Id, q.PageNumber, q.StudentId,
                q.MemorizedAt, q.Status)).ToList();

        return new StudentMemorizationProgressDto(
            studentId,
            hadithDtos,
            quranDtos,
            hadithDtos.Count(h => h.Status == "Memorized"),
            quranDtos.Count(q => q.Status == "Memorized")
        );
    }

    public async Task<Result<List<StudentAttendanceDto>>> GetAttendanceRecordsAsync(
        Guid studentId,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        CancellationToken cancellationToken = default)
    {
        var attendances = await _attendanceRepository.GetAllAsync(
            a => a.ClassStudentId == studentId &&
                 (!fromDate.HasValue || a.StartDate >= fromDate.Value) &&
                 (!toDate.HasValue || a.EndDate <= toDate.Value));

        if (attendances.IsError) return attendances.Errors;

        var attendanceDtos = attendances.Value
            .Select(a => new StudentAttendanceDto(
                a.Id, a.StartDate, a.EndDate, a.ClassStudentId)).ToList();

        return attendanceDtos;
    }

    public async Task<Result<List<PointDto>>> GetPointsAsync(
        Guid studentId,
        Guid? semesterId = null,
        CancellationToken cancellationToken = default)
    {
        var pointsQuery = await _pointRepository.GetAllAsync(
            p => p.StudentId == studentId &&
                 (!semesterId.HasValue || p.SmesterId == semesterId.Value),
            q => q.Include(p => p.Category));

        if (pointsQuery.IsError) return pointsQuery.Errors;

        var classEventsQuery = await _classEventsRepository.GetAllAsync(
            p => p.StudentId == studentId &&
                 (!semesterId.HasValue || p.SmesterId == semesterId.Value));

        if (classEventsQuery.IsError) return classEventsQuery.Errors;

        var pointDtos = pointsQuery.Value
            .Select(p => new PointDto(
                p.Id, p.StudentId, p.EventId, p.ClassId, p.SmesterId,
                p.PointValue, p.CategoryId,
                p.Category != null ? new PointCategoryDto(p.Category.Id, p.Category.Type) : null,
                p.GivenByTeacherId)).ToList();

        return pointDtos;
    }

    public async Task<Result<Success>> EnrollInClassAsync(
        Guid studentId,
        Guid classId,
        CancellationToken cancellationToken = default)
    {
        var existingEnrollment = await _enrollmentRepository.GetAllAsync(
            e => e.StudentId == studentId && e.ClassId == classId);

        if (existingEnrollment.IsError) return existingEnrollment.Errors;
        if (existingEnrollment.Value.Any())
            return ApplicationErrors.StudentAlreadyEnrolledInClass;

        var enrollment = new ClassStudentEnrollment
        {
            StudentId = studentId,
            ClassId = classId
        };

        var addResult = await _enrollmentRepository.AddAsync(enrollment);
        if (addResult.IsError) return addResult.Errors;

        return Result.Success;
    }

    public async Task<Result<List<ClassStudentEnrollmentDto>>> GetClassEnrollmentsAsync(
        Guid studentId,
        CancellationToken cancellationToken = default)
    {
        var enrollments = await _enrollmentRepository.GetAllAsync(
            e => e.StudentId == studentId);

        if (enrollments.IsError) return enrollments.Errors;

        var enrollmentDtos = enrollments.Value
            .Select(e => new ClassStudentEnrollmentDto(
                e.Id, e.StudentId, null, e.ClassId)).ToList();

        return enrollmentDtos;
    }

    public async Task<Result<Success>> AddAttachmentAsync(
        Guid studentId,
        string path,
        string type,
        string safeName,
        string originalName,
        Guid? extensionId = null,
        CancellationToken cancellationToken = default)
    {
        var student = await _studentRepository.GetByIdAsync(studentId);
        if (student.Value == null)
            return ApplicationErrors.StudentNotFound;

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

        var studentAttachment = new StudentAttachment
        {
            StudentId = studentId,
            AttachmentId = attachment.Id
        };

        student.Value.StudentAttachments.Add(studentAttachment);

        var updateResult = await _studentRepository.UpdateAsync(student.Value);
        if (updateResult.IsError)
            return updateResult.Errors;

        return Result.Success;
    }

    public async Task<Result<List<StudentAttachmentDto>>> GetAttachmentsAsync(
        Guid studentId,
        CancellationToken cancellationToken = default)
    {
        var student = await _studentRepository.GetAsync(
            s => s.Id == studentId,
            q => q.Include(s => s.StudentAttachments).ThenInclude(sa => sa.Attachment));

        if (student.IsError) return student.Errors;
        if (student.Value == null)
            return ApplicationErrors.StudentNotFound;

        var attachmentDtos = student.Value.StudentAttachments
            .Select(sa => new StudentAttachmentDto(
                sa.StudentId,
                sa.AttachmentId,
                null,
                sa.Attachment != null ? new AttacmentDto(
                    sa.Attachment.Id,
                    sa.Attachment.Path,
                    sa.Attachment.Type,
                    sa.Attachment.SafeName,
                    sa.Attachment.OriginalName,
                    sa.Attachment.ExtentionId,
                    null) : null)).ToList();

        return attachmentDtos;
    }

    private static System.Linq.Expressions.Expression<Func<Student, bool>>? BuildFilterExpression(
        Guid? classId,
        Guid? semesterId,
        Guid? eventId,
        Guid? teacherId)
    {
        var expressions = new List<System.Linq.Expressions.Expression<Func<Student, bool>>>();

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

        if (teacherId.HasValue)
        {
            expressions.Add(BuildTeacherFilter(teacherId.Value));
        }

        return expressions.Count > 0 ? ExpressionBuilderLib.src.Core.ExpressionCombiner.OrAll(expressions.ToArray()) : null;
    }

    private static System.Linq.Expressions.Expression<Func<Student, bool>> BuildClassFilter(Guid classId)
    {
        return s => s.StudentClassEventsPoints.Any(p => p.ClassId == classId)
                 || s.StudentHadiths.Any(h => h.ClassId == classId)
                 || s.StudentQuraanPages.Any(q => q.ClassId == classId);
    }

    private static System.Linq.Expressions.Expression<Func<Student, bool>> BuildSemesterFilter(Guid semesterId)
    {
        return s => s.StudentClassEventsPoints.Any(p => p.SmesterId == semesterId)
                 || s.Points.Any(p => p.SmesterId == semesterId);
    }

    private static System.Linq.Expressions.Expression<Func<Student, bool>> BuildEventFilter(Guid eventId)
    {
        return s => s.StudentClassEventsPoints.Any(p => p.EventId == eventId)
                 || s.Points.Any(p => p.EventId == eventId);
    }

    private static System.Linq.Expressions.Expression<Func<Student, bool>> BuildTeacherFilter(Guid teacherId)
    {
        return s => s.StudentHadiths.Any(h => h.TeacherId == teacherId)
                 || s.StudentQuraanPages.Any(q => q.TeacherId == teacherId)
                 || s.Points.Any(p => p.GivenByTeacherId == teacherId);
    }
}
