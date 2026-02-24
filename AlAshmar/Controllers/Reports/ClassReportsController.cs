using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Interfaces.Reports;
using Microsoft.AspNetCore.Mvc;

namespace AlAshmar.Controllers.Reports;

/// <summary>
/// Controller for class-related reports including attendance, points, and memorization summaries.
/// </summary>
public class ClassReportsController : ReportsBaseController
{
    private readonly IClassReportService _classReportService;

    public ClassReportsController(IClassReportService classReportService)
    {
        _classReportService = classReportService;
    }

    /// <summary>
    /// Get daily report for a class including attendance, points, and memorization.
    /// </summary>
    /// <param name="classId">The class ID</param>
    /// <param name="date">The report date (defaults to today)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("classes/{classId:guid}/daily")]
    public async Task<ActionResult<ClassDailyReportDto>> GetDailyReport(
        [FromRoute] Guid classId,
        [FromQuery] DateTime? date = null,
        CancellationToken cancellationToken = default)
    {
        var reportDate = date ?? DateTime.Today;
        var result = await _classReportService.GetDailyReportAsync(classId, reportDate, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    /// <summary>
    /// Get weekly report for a class including attendance, points, and memorization.
    /// </summary>
    /// <param name="classId">The class ID</param>
    /// <param name="weekStart">The start of the week (defaults to current week's Monday)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("classes/{classId:guid}/weekly")]
    public async Task<ActionResult<ClassWeeklyReportDto>> GetWeeklyReport(
        [FromRoute] Guid classId,
        [FromQuery] DateTime? weekStart = null,
        CancellationToken cancellationToken = default)
    {
        var weekStartDate = weekStart ?? GetWeekStart(DateTime.Today);
        var result = await _classReportService.GetWeeklyReportAsync(classId, weekStartDate, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    /// <summary>
    /// Get monthly report for a class including attendance, points, and memorization.
    /// </summary>
    /// <param name="classId">The class ID</param>
    /// <param name="month">The month (defaults to current month)</param>
    /// <param name="year">The year (defaults to current year)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("classes/{classId:guid}/monthly")]
    public async Task<ActionResult<ClassMonthlyReportDto>> GetMonthlyReport(
        [FromRoute] Guid classId,
        [FromQuery] int? month = null,
        [FromQuery] int? year = null,
        CancellationToken cancellationToken = default)
    {
        var reportMonth = month ?? DateTime.Now.Month;
        var reportYear = year ?? DateTime.Now.Year;

        var result = await _classReportService.GetMonthlyReportAsync(classId, reportMonth, reportYear, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    /// <summary>
    /// Get semester report for a class including attendance, points, and memorization.
    /// </summary>
    /// <param name="classId">The class ID</param>
    /// <param name="semesterId">The semester ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("classes/{classId:guid}/semesters/{semesterId:guid}")]
    public async Task<ActionResult<ClassSemesterReportDto>> GetSemesterReport(
        [FromRoute] Guid classId,
        [FromRoute] Guid semesterId,
        CancellationToken cancellationToken = default)
    {
        var result = await _classReportService.GetSemesterReportAsync(classId, semesterId, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    /// <summary>
    /// Get attendance summary for a class.
    /// </summary>
    /// <param name="classId">The class ID</param>
    /// <param name="fromDate">From date</param>
    /// <param name="toDate">To date</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("classes/{classId:guid}/attendance")]
    public async Task<ActionResult<ClassAttendanceSummary>> GetAttendanceSummary(
        [FromRoute] Guid classId,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken cancellationToken = default)
    {
        if (!ValidateDateRange(fromDate, toDate, out var errorMessage))
            return BadRequest(errorMessage);

        var date = fromDate ?? DateTime.Today;
        var result = await _classReportService.GetDailyReportAsync(classId, date, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok(result.Value.AttendanceSummary);
    }

    /// <summary>
    /// Get points summary for a class.
    /// </summary>
    /// <param name="classId">The class ID</param>
    /// <param name="semesterId">Optional semester ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("classes/{classId:guid}/points")]
    public async Task<ActionResult<ClassPointsSummary>> GetPointsSummary(
        [FromRoute] Guid classId,
        [FromQuery] Guid? semesterId = null,
        CancellationToken cancellationToken = default)
    {
        var date = DateTime.Today;
        var result = await _classReportService.GetDailyReportAsync(classId, date, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok(result.Value.PointsSummary);
    }

    /// <summary>
    /// Get memorization summary for a class.
    /// </summary>
    /// <param name="classId">The class ID</param>
    /// <param name="semesterId">Optional semester ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("classes/{classId:guid}/memorization")]
    public async Task<ActionResult<ClassMemorizationSummary>> GetMemorizationSummary(
        [FromRoute] Guid classId,
        [FromQuery] Guid? semesterId = null,
        CancellationToken cancellationToken = default)
    {
        var date = DateTime.Today;
        var result = await _classReportService.GetDailyReportAsync(classId, date, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok(result.Value.MemorizationSummary);
    }
}
