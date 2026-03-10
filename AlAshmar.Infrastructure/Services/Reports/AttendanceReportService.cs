using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Interfaces.Reports;
using AlAshmar.Application.Repos;
using AlAshmar.Domain.Entities.Students;
using AlAshmar.Domain.Entities.Teachers;

namespace AlAshmar.Infrastructure.Services.Reports;

public class AttendanceReportService : IAttendanceReportService
{
    private readonly IRepositoryBase<StudentAttendance, Guid> _studentAttendanceRepository;
    private readonly IRepositoryBase<TeacherAttencance, Guid> _teacherAttendanceRepository;

    public AttendanceReportService(
        IRepositoryBase<StudentAttendance, Guid> studentAttendanceRepository,
        IRepositoryBase<TeacherAttencance, Guid> teacherAttendanceRepository)
    {
        _studentAttendanceRepository = studentAttendanceRepository;
        _teacherAttendanceRepository = teacherAttendanceRepository;
    }

    public async Task<Result<AttendanceOverviewReportDto>> GetOverviewReportAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default)
    {
        var studentAttendanceResult = await _studentAttendanceRepository.GetAllAsync(
            filter: a => a.StartDate.Date <= toDate && a.EndDate.Date >= fromDate);

        var teacherAttendanceResult = await _teacherAttendanceRepository.GetAllAsync(
            filter: a => a.StartDate.Date <= toDate && a.EndDate.Date >= fromDate);

        var studentRecords = studentAttendanceResult.Value?.ToList() ?? new List<StudentAttendance>();
        var teacherRecords = teacherAttendanceResult.Value?.ToList() ?? new List<TeacherAttencance>();

        var overallSummary = new AttendanceSummaryDto(
            studentRecords.Count + teacherRecords.Count,
            0, 0, 0, 0
        );

        return new AttendanceOverviewReportDto(
            fromDate,
            toDate,
            overallSummary,
            new List<StudentAttendanceDetailDto>(),
            new List<TeacherAttendanceDetailDto>()
        );
    }

    public async Task<Result<PagedList<StudentAttendanceDetailDto>>> GetStudentAttendanceReportAsync(
        DateTime fromDate, DateTime toDate, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var studentAttendanceResult = await _studentAttendanceRepository.GetAllAsync(
            filter: a => a.StartDate.Date <= toDate && a.EndDate.Date >= fromDate);

        var studentRecords = studentAttendanceResult.Value?.ToList() ?? new List<StudentAttendance>();

        var studentDetails = studentRecords
            .Select(a => new StudentAttendanceDetailDto(
                a.ClassStudentId,
                "Student",
                1, 0, 100,
                new List<DateTime>()
            ))
            .ToList();

        var totalItems = studentDetails.Count;
        var pagedData = studentDetails
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return PagedList<StudentAttendanceDetailDto>.Create(pagedData, totalItems, page, pageSize);
    }

    public async Task<Result<PagedList<TeacherAttendanceDetailDto>>> GetTeacherAttendanceReportAsync(
        DateTime fromDate, DateTime toDate, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var teacherAttendanceResult = await _teacherAttendanceRepository.GetAllAsync(
            filter: a => a.StartDate.Date <= toDate && a.EndDate.Date >= fromDate);

        var teacherRecords = teacherAttendanceResult.Value?.ToList() ?? new List<TeacherAttencance>();

        var teacherDetails = teacherRecords
            .Select(a => new TeacherAttendanceDetailDto(
                a.ClassTeacherId,
                "Teacher",
                1, 0, 100,
                new List<DateTime>()
            ))
            .ToList();

        var totalItems = teacherDetails.Count;
        var pagedData = teacherDetails
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return PagedList<TeacherAttendanceDetailDto>.Create(pagedData, totalItems, page, pageSize);
    }
}
