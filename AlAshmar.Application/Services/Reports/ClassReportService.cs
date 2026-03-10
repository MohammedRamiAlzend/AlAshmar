using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Interfaces.Reports;
using AlAshmar.Application.Repos;
using AlAshmar.Domain.Entities.Students;

namespace AlAshmar.Application.Services.Reports;

public class ClassReportService : IClassReportService
{
    private readonly IRepositoryBase<ClassStudentEnrollment, Guid> _enrollmentRepository;
    private readonly IRepositoryBase<Student, Guid> _studentRepository;

    public ClassReportService(
        IRepositoryBase<ClassStudentEnrollment, Guid> enrollmentRepository,
        IRepositoryBase<Student, Guid> studentRepository)
    {
        _enrollmentRepository = enrollmentRepository;
        _studentRepository = studentRepository;
    }

    public async Task<Result<ClassDailyReportDto>> GetDailyReportAsync(Guid classId, DateTime date, CancellationToken cancellationToken = default)
    {
        var enrollments = await _enrollmentRepository.GetAllAsync(filter: e => e.ClassId == classId);
        var studentIds = enrollments.Value?.Select(e => e.StudentId).ToList() ?? new List<Guid>();
        
        var students = new List<Student>();
        foreach (var studentId in studentIds)
        {
            var studentResult = await _studentRepository.GetByIdAsync(studentId);
            if (studentResult.Value != null)
                students.Add(studentResult.Value);
        }
        
        var studentBriefs = students.Select(s => new StudentBriefDto(s.Id, s.Name)).ToList();

        return new ClassDailyReportDto(
            classId,
            $"Class {classId.ToString().Substring(0, 8)}",
            date,
            new ClassAttendanceSummary(students.Count, 0, new List<StudentAttendanceRecordDto>()),
            new ClassPointsSummary(0, 0, new List<StudentPointsRecordDto>()),
            new ClassMemorizationSummary(0, 0, new List<StudentMemorizationRecordDto>()),
            studentBriefs
        );
    }

    public async Task<Result<ClassWeeklyReportDto>> GetWeeklyReportAsync(Guid classId, DateTime weekStart, CancellationToken cancellationToken = default)
    {
        var weekEnd = weekStart.AddDays(6);
        var result = await GetDailyReportAsync(classId, weekStart, cancellationToken);
        
        if (result.IsError)
            return Error.NotFound("Class", classId.ToString());

        return new ClassWeeklyReportDto(
            result.Value.ClassId,
            result.Value.ClassName,
            weekStart,
            weekEnd,
            result.Value.AttendanceSummary,
            result.Value.PointsSummary,
            result.Value.MemorizationSummary,
            result.Value.Students
        );
    }

    public async Task<Result<ClassMonthlyReportDto>> GetMonthlyReportAsync(Guid classId, int month, int year, CancellationToken cancellationToken = default)
    {
        var monthStart = new DateTime(year, month, 1);
        var result = await GetDailyReportAsync(classId, monthStart, cancellationToken);

        if (result.IsError)
            return Error.NotFound("Class", classId.ToString());

        return new ClassMonthlyReportDto(
            result.Value.ClassId,
            result.Value.ClassName,
            month,
            year,
            result.Value.AttendanceSummary,
            result.Value.PointsSummary,
            result.Value.MemorizationSummary,
            result.Value.Students
        );
    }

    public async Task<Result<ClassSemesterReportDto>> GetSemesterReportAsync(Guid classId, Guid semesterId, CancellationToken cancellationToken = default)
    {
        var result = await GetDailyReportAsync(classId, DateTime.Today, cancellationToken);

        if (result.IsError)
            return Error.NotFound("Class", classId.ToString());

        return new ClassSemesterReportDto(
            result.Value.ClassId,
            result.Value.ClassName,
            semesterId,
            "Semester",
            result.Value.AttendanceSummary,
            result.Value.PointsSummary,
            result.Value.MemorizationSummary,
            result.Value.Students
        );
    }
}
