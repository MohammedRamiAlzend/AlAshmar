using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Interfaces.Reports;
using Microsoft.AspNetCore.Mvc;

namespace AlAshmar.Controllers.Reports;

/// <summary>
/// Controller for consolidated attendance reports across students and teachers.
/// </summary>
public class AttendanceReportsController : ReportsBaseController
{
    private readonly IAttendanceReportService _attendanceReportService;

    public AttendanceReportsController(IAttendanceReportService attendanceReportService)
    {
        _attendanceReportService = attendanceReportService;
    }

    /// <summary>
    /// Get overall attendance overview report for both students and teachers.
    /// </summary>
    /// <param name="fromDate">From date (defaults to start of current month)</param>
    /// <param name="toDate">To date (defaults to today)</param>
    /// <param name="cancellationToken">Cancellation token</param>
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

    /// <summary>
    /// Get student attendance report with pagination.
    /// </summary>
    /// <param name="fromDate">From date (defaults to start of current month)</param>
    /// <param name="toDate">To date (defaults to today)</param>
    /// <param name="page">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="cancellationToken">Cancellation token</param>
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

    /// <summary>
    /// Get teacher attendance report with pagination.
    /// </summary>
    /// <param name="fromDate">From date (defaults to start of current month)</param>
    /// <param name="toDate">To date (defaults to today)</param>
    /// <param name="page">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="cancellationToken">Cancellation token</param>
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

    /// <summary>
    /// Get attendance summary for a specific period.
    /// </summary>
    /// <param name="fromDate">From date</param>
    /// <param name="toDate">To date</param>
    /// <param name="cancellationToken">Cancellation token</param>
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

    /// <summary>
    /// Get daily attendance report for a specific date.
    /// </summary>
    /// <param name="date">The date (defaults to today)</param>
    /// <param name="cancellationToken">Cancellation token</param>
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

    /// <summary>
    /// Get weekly attendance report.
    /// </summary>
    /// <param name="weekStart">The start of the week (defaults to current week's Monday)</param>
    /// <param name="cancellationToken">Cancellation token</param>
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

    /// <summary>
    /// Get monthly attendance report.
    /// </summary>
    /// <param name="month">The month (defaults to current month)</param>
    /// <param name="year">The year (defaults to current year)</param>
    /// <param name="cancellationToken">Cancellation token</param>
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
