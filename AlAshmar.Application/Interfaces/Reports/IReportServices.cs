using AlAshmar.Application.DTOs.Domain;

namespace AlAshmar.Application.Interfaces.Reports;

public interface IStudentReportService
{
    Task<Result<StudentDailyReportDto>> GetDailyReportAsync(Guid studentId, DateTime date, CancellationToken cancellationToken = default);
    Task<Result<StudentWeeklyReportDto>> GetWeeklyReportAsync(Guid studentId, DateTime weekStart, CancellationToken cancellationToken = default);
    Task<Result<StudentMonthlyReportDto>> GetMonthlyReportAsync(Guid studentId, int month, int year, CancellationToken cancellationToken = default);
    Task<Result<StudentSemesterReportDto>> GetSemesterReportAsync(Guid studentId, Guid semesterId, CancellationToken cancellationToken = default);
}

public interface ITeacherReportService
{
    Task<Result<TeacherDailyReportDto>> GetDailyReportAsync(Guid teacherId, DateTime date, CancellationToken cancellationToken = default);
    Task<Result<TeacherWeeklyReportDto>> GetWeeklyReportAsync(Guid teacherId, DateTime weekStart, CancellationToken cancellationToken = default);
    Task<Result<TeacherMonthlyReportDto>> GetMonthlyReportAsync(Guid teacherId, int month, int year, CancellationToken cancellationToken = default);
    Task<Result<TeacherSemesterReportDto>> GetSemesterReportAsync(Guid teacherId, Guid semesterId, CancellationToken cancellationToken = default);
}

public interface IClassReportService
{
    Task<Result<ClassDailyReportDto>> GetDailyReportAsync(Guid classId, DateTime date, CancellationToken cancellationToken = default);
    Task<Result<ClassWeeklyReportDto>> GetWeeklyReportAsync(Guid classId, DateTime weekStart, CancellationToken cancellationToken = default);
    Task<Result<ClassMonthlyReportDto>> GetMonthlyReportAsync(Guid classId, int month, int year, CancellationToken cancellationToken = default);
    Task<Result<ClassSemesterReportDto>> GetSemesterReportAsync(Guid classId, Guid semesterId, CancellationToken cancellationToken = default);
}

public interface ISemesterReportService
{
    Task<Result<SemesterOverviewReportDto>> GetOverviewReportAsync(Guid semesterId, CancellationToken cancellationToken = default);
}

public interface IAttendanceReportService
{
    Task<Result<AttendanceOverviewReportDto>> GetOverviewReportAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default);
    Task<Result<PagedList<StudentAttendanceDetailDto>>> GetStudentAttendanceReportAsync(DateTime fromDate, DateTime toDate, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default);
    Task<Result<PagedList<TeacherAttendanceDetailDto>>> GetTeacherAttendanceReportAsync(DateTime fromDate, DateTime toDate, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default);
}

public interface IPointsReportService
{
    Task<Result<PointsOverviewReportDto>> GetOverviewReportAsync(Guid? semesterId = null, DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default);
    Task<Result<PagedList<StudentPointsDetailDto>>> GetStudentPointsReportAsync(Guid? semesterId = null, DateTime? fromDate = null, DateTime? toDate = null, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default);
    Task<Result<PagedList<TeacherPointsGivenDto>>> GetTeacherPointsReportAsync(Guid? semesterId = null, DateTime? fromDate = null, DateTime? toDate = null, int page = 1, int pageSize = 10, CancellationToken cancellationToken = default);
}
