using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Interfaces.Reports;
using Microsoft.AspNetCore.Mvc;

namespace AlAshmar.Controllers.Reports;

/// <summary>
/// Controller for student-related reports including attendance, memorization, and points.
/// </summary>
public class StudentReportsController : ReportsBaseController
{
    private readonly IStudentReportService _studentReportService;

    public StudentReportsController(IStudentReportService studentReportService)
    {
        _studentReportService = studentReportService;
    }

    /// <summary>
    /// Get daily report for a student including attendance, memorization progress, and points.
    /// </summary>
    /// <param name="studentId">The student ID</param>
    /// <param name="date">The report date (defaults to today)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("students/{studentId:guid}/daily")]
    public async Task<ActionResult<StudentDailyReportDto>> GetDailyReport(
        [FromRoute] Guid studentId,
        [FromQuery] DateTime? date = null,
        CancellationToken cancellationToken = default)
    {
        var reportDate = date ?? DateTime.Today;
        var result = await _studentReportService.GetDailyReportAsync(studentId, reportDate, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    /// <summary>
    /// Get weekly report for a student including attendance, memorization progress, and points.
    /// </summary>
    /// <param name="studentId">The student ID</param>
    /// <param name="weekStart">The start of the week (defaults to current week's Monday)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("students/{studentId:guid}/weekly")]
    public async Task<ActionResult<StudentWeeklyReportDto>> GetWeeklyReport(
        [FromRoute] Guid studentId,
        [FromQuery] DateTime? weekStart = null,
        CancellationToken cancellationToken = default)
    {
        var weekStartDate = weekStart ?? GetWeekStart(DateTime.Today);
        var result = await _studentReportService.GetWeeklyReportAsync(studentId, weekStartDate, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    /// <summary>
    /// Get monthly report for a student including attendance, memorization progress, and points.
    /// </summary>
    /// <param name="studentId">The student ID</param>
    /// <param name="month">The month (defaults to current month)</param>
    /// <param name="year">The year (defaults to current year)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("students/{studentId:guid}/monthly")]
    public async Task<ActionResult<StudentMonthlyReportDto>> GetMonthlyReport(
        [FromRoute] Guid studentId,
        [FromQuery] int? month = null,
        [FromQuery] int? year = null,
        CancellationToken cancellationToken = default)
    {
        var reportMonth = month ?? DateTime.Now.Month;
        var reportYear = year ?? DateTime.Now.Year;

        var result = await _studentReportService.GetMonthlyReportAsync(studentId, reportMonth, reportYear, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    /// <summary>
    /// Get semester report for a student including attendance, memorization progress, and points.
    /// </summary>
    /// <param name="studentId">The student ID</param>
    /// <param name="semesterId">The semester ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("students/{studentId:guid}/semesters/{semesterId:guid}")]
    public async Task<ActionResult<StudentSemesterReportDto>> GetSemesterReport(
        [FromRoute] Guid studentId,
        [FromRoute] Guid semesterId,
        CancellationToken cancellationToken = default)
    {
        var result = await _studentReportService.GetSemesterReportAsync(studentId, semesterId, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    /// <summary>
    /// Get all reports for a student filtered by period.
    /// </summary>
    /// <param name="studentId">The student ID</param>
    /// <param name="periodType">The period type (All, Daily, Weekly, Monthly, Semester)</param>
    /// <param name="fromDate">Optional from date</param>
    /// <param name="toDate">Optional to date</param>
    /// <param name="semesterId">Optional semester ID for semester reports</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("students/{studentId:guid}/all")]
    public async Task<ActionResult<object>> GetAllReports(
        [FromRoute] Guid studentId,
        [FromQuery] ReportPeriodType periodType = ReportPeriodType.All,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] Guid? semesterId = null,
        CancellationToken cancellationToken = default)
    {
        if (!ValidateDateRange(fromDate, toDate, out var errorMessage))
            return BadRequest(errorMessage);

        var reports = new
        {
            Daily = periodType == ReportPeriodType.Daily || periodType == ReportPeriodType.All
                ? await _studentReportService.GetDailyReportAsync(studentId, fromDate ?? DateTime.Today, cancellationToken)
                : null,
            Weekly = periodType == ReportPeriodType.Weekly || periodType == ReportPeriodType.All
                ? await _studentReportService.GetWeeklyReportAsync(studentId, GetWeekStart(fromDate ?? DateTime.Today), cancellationToken)
                : null,
            Monthly = periodType == ReportPeriodType.Monthly || periodType == ReportPeriodType.All
                ? await _studentReportService.GetMonthlyReportAsync(studentId, fromDate?.Month ?? DateTime.Now.Month, fromDate?.Year ?? DateTime.Now.Year, cancellationToken)
                : null,
            Semester = periodType == ReportPeriodType.Semester && semesterId.HasValue
                ? await _studentReportService.GetSemesterReportAsync(studentId, semesterId.Value, cancellationToken)
                : null
        };

        return Ok(reports);
    }
}
