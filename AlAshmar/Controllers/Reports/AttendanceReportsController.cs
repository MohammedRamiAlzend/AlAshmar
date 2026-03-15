using AlAshmar.Application.Interfaces.Reports;

namespace AlAshmar.Controllers.Reports;

public class AttendanceReportsController : ReportsBaseController
{
    private readonly IAttendanceReportService _attendanceReportService;

    public AttendanceReportsController(IAttendanceReportService attendanceReportService)
    {
        _attendanceReportService = attendanceReportService;
    }

    [HttpGet("attendance/overview")]
    public async Task<ActionResult<AttendanceOverviewReportDto>> GetOverviewReport(
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken cancellationToken = default)
    {
        var reportFromDate = fromDate ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var reportToDate = toDate ?? DateTime.Today;

        if (!ValidateDateRange(reportFromDate, reportToDate, out var errorMessage))
            return BadRequest(errorMessage);

        var result = await _attendanceReportService.GetOverviewReportAsync(reportFromDate, reportToDate, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpGet("attendance/students")]
    public async Task<ActionResult<PagedList<StudentAttendanceDetailDto>>> GetStudentAttendanceReport(
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var reportFromDate = fromDate ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var reportToDate = toDate ?? DateTime.Today;

        if (!ValidateDateRange(reportFromDate, reportToDate, out var errorMessage))
            return BadRequest(errorMessage);

        var result = await _attendanceReportService.GetStudentAttendanceReportAsync(
            reportFromDate, reportToDate, page, pageSize, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return CreatePagedResponse(result.Value);
    }

    [HttpGet("attendance/teachers")]
    public async Task<ActionResult<PagedList<TeacherAttendanceDetailDto>>> GetTeacherAttendanceReport(
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var reportFromDate = fromDate ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var reportToDate = toDate ?? DateTime.Today;

        if (!ValidateDateRange(reportFromDate, reportToDate, out var errorMessage))
            return BadRequest(errorMessage);

        var result = await _attendanceReportService.GetTeacherAttendanceReportAsync(
            reportFromDate, reportToDate, page, pageSize, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return CreatePagedResponse(result.Value);
    }

    [HttpGet("attendance/summary")]
    public async Task<ActionResult<AttendanceSummaryDto>> GetAttendanceSummary(
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken cancellationToken = default)
    {
        var reportFromDate = fromDate ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var reportToDate = toDate ?? DateTime.Today;

        if (!ValidateDateRange(reportFromDate, reportToDate, out var errorMessage))
            return BadRequest(errorMessage);

        var result = await _attendanceReportService.GetOverviewReportAsync(reportFromDate, reportToDate, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok(result.Value.OverallSummary);
    }

    [HttpGet("attendance/daily")]
    public async Task<ActionResult<AttendanceOverviewReportDto>> GetDailyAttendance(
        [FromQuery] DateTime? date = null,
        CancellationToken cancellationToken = default)
    {
        var reportDate = date ?? DateTime.Today;
        var result = await _attendanceReportService.GetOverviewReportAsync(reportDate, reportDate, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpGet("attendance/weekly")]
    public async Task<ActionResult<AttendanceOverviewReportDto>> GetWeeklyAttendance(
        [FromQuery] DateTime? weekStart = null,
        CancellationToken cancellationToken = default)
    {
        var weekStartDate = weekStart ?? GetWeekStart(DateTime.Today);
        var weekEndDate = GetWeekEnd(weekStartDate);

        var result = await _attendanceReportService.GetOverviewReportAsync(weekStartDate, weekEndDate, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    [HttpGet("attendance/monthly")]
    public async Task<ActionResult<AttendanceOverviewReportDto>> GetMonthlyAttendance(
        [FromQuery] int? month = null,
        [FromQuery] int? year = null,
        CancellationToken cancellationToken = default)
    {
        var reportMonth = month ?? DateTime.Now.Month;
        var reportYear = year ?? DateTime.Now.Year;

        var fromDate = new DateTime(reportYear, reportMonth, 1);
        var toDate = fromDate.AddMonths(1).AddDays(-1);

        var result = await _attendanceReportService.GetOverviewReportAsync(fromDate, toDate, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }
}
