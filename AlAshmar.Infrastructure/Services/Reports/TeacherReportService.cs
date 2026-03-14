using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Interfaces.Reports;
using AlAshmar.Application.Repos;
using AlAshmar.Domain.Entities.Academic;
using AlAshmar.Domain.Entities.Teachers;

namespace AlAshmar.Infrastructure.Services.Reports;

public class TeacherReportService : ITeacherReportService
{
    private readonly IRepositoryBase<Teacher, Guid> _teacherRepository;
    private readonly IRepositoryBase<Point, Guid> _pointRepository;

    public TeacherReportService(
        IRepositoryBase<Teacher, Guid> teacherRepository,
        IRepositoryBase<Point, Guid> pointRepository)
    {
        _teacherRepository = teacherRepository;
        _pointRepository = pointRepository;
    }

    public async Task<Result<TeacherDailyReportDto>> GetDailyReportAsync(Guid teacherId, DateTime date, CancellationToken cancellationToken = default)
    {
        var teacherResult = await _teacherRepository.GetByIdAsync(teacherId);
        if (teacherResult.Value == null)
            return ApplicationErrors.TeacherNotFound;

        var teacher = teacherResult.Value;

        return new TeacherDailyReportDto(
            teacher.Id,
            teacher.Name,
            date,
            new TeacherAttendanceSummary(0, 0, 0, new List<ClassAssignmentDto>()),
            new TeacherPointsSummary(0, 0, new List<PointCategoryBreakdownDto>()),
            new List<StudentProgressUnderTeacherDto>()
        );
    }

    public async Task<Result<TeacherWeeklyReportDto>> GetWeeklyReportAsync(Guid teacherId, DateTime weekStart, CancellationToken cancellationToken = default)
    {
        var teacherResult = await _teacherRepository.GetByIdAsync(teacherId);
        if (teacherResult.Value == null)
            return ApplicationErrors.TeacherNotFound;

        var teacher = teacherResult.Value;
        var weekEnd = weekStart.AddDays(6);

        return new TeacherWeeklyReportDto(
            teacher.Id,
            teacher.Name,
            weekStart,
            weekEnd,
            new TeacherAttendanceSummary(0, 0, 0, new List<ClassAssignmentDto>()),
            new TeacherPointsSummary(0, 0, new List<PointCategoryBreakdownDto>()),
            new List<StudentProgressUnderTeacherDto>()
        );
    }

    public async Task<Result<TeacherMonthlyReportDto>> GetMonthlyReportAsync(Guid teacherId, int month, int year, CancellationToken cancellationToken = default)
    {
        var teacherResult = await _teacherRepository.GetByIdAsync(teacherId);
        if (teacherResult.Value == null)
            return ApplicationErrors.TeacherNotFound;

        var teacher = teacherResult.Value;

        return new TeacherMonthlyReportDto(
            teacher.Id,
            teacher.Name,
            month,
            year,
            new TeacherAttendanceSummary(0, 0, 0, new List<ClassAssignmentDto>()),
            new TeacherPointsSummary(0, 0, new List<PointCategoryBreakdownDto>()),
            new List<StudentProgressUnderTeacherDto>()
        );
    }

    public async Task<Result<TeacherSemesterReportDto>> GetSemesterReportAsync(Guid teacherId, Guid semesterId, CancellationToken cancellationToken = default)
    {
        var teacherResult = await _teacherRepository.GetByIdAsync(teacherId);
        if (teacherResult.Value == null)
            return ApplicationErrors.TeacherNotFound;

        var teacher = teacherResult.Value;

        return new TeacherSemesterReportDto(
            teacher.Id,
            teacher.Name,
            semesterId,
            "Semester",
            new TeacherAttendanceSummary(0, 0, 0, new List<ClassAssignmentDto>()),
            new TeacherPointsSummary(0, 0, new List<PointCategoryBreakdownDto>()),
            new List<StudentProgressUnderTeacherDto>()
        );
    }
}
