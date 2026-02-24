using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Interfaces.Reports;
using Microsoft.AspNetCore.Mvc;

namespace AlAshmar.Controllers.Reports;

/// <summary>
/// Controller for consolidated points reports across students and teachers.
/// </summary>
public class PointsReportsController : ReportsBaseController
{
    private readonly IPointsReportService _pointsReportService;

    public PointsReportsController(IPointsReportService pointsReportService)
    {
        _pointsReportService = pointsReportService;
    }

    /// <summary>
    /// Get overall points overview report.
    /// </summary>
    /// <param name="semesterId">Optional semester ID to filter by</param>
    /// <param name="fromDate">Optional from date</param>
    /// <param name="toDate">Optional to date</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("points/overview")]
    public async Task<ActionResult<PointsOverviewReportDto>> GetOverviewReport(
        [FromQuery] Guid? semesterId = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken cancellationToken = default)
    {
        if (!ValidateDateRange(fromDate, toDate, out var errorMessage))
            return BadRequest(errorMessage);

        var result = await _pointsReportService.GetOverviewReportAsync(semesterId, fromDate, toDate, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    /// <summary>
    /// Get points summary.
    /// </summary>
    /// <param name="semesterId">Optional semester ID to filter by</param>
    /// <param name="fromDate">Optional from date</param>
    /// <param name="toDate">Optional to date</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("points/summary")]
    public async Task<ActionResult<PointsSummaryDto>> GetPointsSummary(
        [FromQuery] Guid? semesterId = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken cancellationToken = default)
    {
        if (!ValidateDateRange(fromDate, toDate, out var errorMessage))
            return BadRequest(errorMessage);

        var result = await _pointsReportService.GetOverviewReportAsync(semesterId, fromDate, toDate, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok(result.Value.OverallSummary);
    }

    /// <summary>
    /// Get student points report with pagination.
    /// </summary>
    /// <param name="semesterId">Optional semester ID to filter by</param>
    /// <param name="fromDate">Optional from date</param>
    /// <param name="toDate">Optional to date</param>
    /// <param name="page">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("points/students")]
    public async Task<ActionResult<PagedList<StudentPointsDetailDto>>> GetStudentPointsReport(
        [FromQuery] Guid? semesterId = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        if (!ValidateDateRange(fromDate, toDate, out var errorMessage))
            return BadRequest(errorMessage);

        var result = await _pointsReportService.GetStudentPointsReportAsync(
            semesterId, fromDate, toDate, page, pageSize, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return CreatePagedResponse(result.Value);
    }

    /// <summary>
    /// Get teacher points report with pagination.
    /// </summary>
    /// <param name="semesterId">Optional semester ID to filter by</param>
    /// <param name="fromDate">Optional from date</param>
    /// <param name="toDate">Optional to date</param>
    /// <param name="page">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("points/teachers")]
    public async Task<ActionResult<PagedList<TeacherPointsGivenDto>>> GetTeacherPointsReport(
        [FromQuery] Guid? semesterId = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        if (!ValidateDateRange(fromDate, toDate, out var errorMessage))
            return BadRequest(errorMessage);

        var result = await _pointsReportService.GetTeacherPointsReportAsync(
            semesterId, fromDate, toDate, page, pageSize, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return CreatePagedResponse(result.Value);
    }

    /// <summary>
    /// Get points breakdown by category.
    /// </summary>
    /// <param name="semesterId">Optional semester ID to filter by</param>
    /// <param name="fromDate">Optional from date</param>
    /// <param name="toDate">Optional to date</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("points/by-category")]
    public async Task<ActionResult<object>> GetPointsByCategory(
        [FromQuery] Guid? semesterId = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken cancellationToken = default)
    {
        if (!ValidateDateRange(fromDate, toDate, out var errorMessage))
            return BadRequest(errorMessage);

        var result = await _pointsReportService.GetOverviewReportAsync(semesterId, fromDate, toDate, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        var summary = result.Value.OverallSummary;
        var breakdown = new
        {
            Quran = summary.QuranPoints,
            Hadith = summary.HadithPoints,
            Attendance = summary.AttendancePoints,
            Behavior = summary.BehaviorPoints,
            Total = summary.TotalPoints
        };

        return Ok(breakdown);
    }

    /// <summary>
    /// Get top students by points.
    /// </summary>
    /// <param name="semesterId">Optional semester ID to filter by</param>
    /// <param name="top">Number of top students to return (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("points/top-students")]
    public async Task<ActionResult<List<StudentPointsDetailDto>>> GetTopStudentsByPoints(
        [FromQuery] Guid? semesterId = null,
        [FromQuery] int top = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _pointsReportService.GetStudentPointsReportAsync(
            semesterId, null, null, 1, top, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok(result.Value.Items.ToList());
    }

    /// <summary>
    /// Get top teachers by points given.
    /// </summary>
    /// <param name="semesterId">Optional semester ID to filter by</param>
    /// <param name="top">Number of top teachers to return (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("points/top-teachers")]
    public async Task<ActionResult<List<TeacherPointsGivenDto>>> GetTopTeachersByPoints(
        [FromQuery] Guid? semesterId = null,
        [FromQuery] int top = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _pointsReportService.GetTeacherPointsReportAsync(
            semesterId, null, null, 1, top, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok(result.Value.Items.ToList());
    }

    /// <summary>
    /// Get daily points report.
    /// </summary>
    /// <param name="date">The date (defaults to today)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("points/daily")]
    public async Task<ActionResult<PointsOverviewReportDto>> GetDailyPoints(
        [FromQuery] DateTime? date = null,
        CancellationToken cancellationToken = default)
    {
        var reportDate = date ?? DateTime.Today;
        var result = await _pointsReportService.GetOverviewReportAsync(null, reportDate, reportDate, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    /// <summary>
    /// Get weekly points report.
    /// </summary>
    /// <param name="weekStart">The start of the week (defaults to current week's Monday)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("points/weekly")]
    public async Task<ActionResult<PointsOverviewReportDto>> GetWeeklyPoints(
        [FromQuery] DateTime? weekStart = null,
        CancellationToken cancellationToken = default)
    {
        var weekStartDate = weekStart ?? GetWeekStart(DateTime.Today);
        var weekEndDate = GetWeekEnd(weekStartDate);

        var result = await _pointsReportService.GetOverviewReportAsync(null, weekStartDate, weekEndDate, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    /// <summary>
    /// Get monthly points report.
    /// </summary>
    /// <param name="month">The month (defaults to current month)</param>
    /// <param name="year">The year (defaults to current year)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("points/monthly")]
    public async Task<ActionResult<PointsOverviewReportDto>> GetMonthlyPoints(
        [FromQuery] int? month = null,
        [FromQuery] int? year = null,
        CancellationToken cancellationToken = default)
    {
        var reportMonth = month ?? DateTime.Now.Month;
        var reportYear = year ?? DateTime.Now.Year;

        var fromDate = new DateTime(reportYear, reportMonth, 1);
        var toDate = fromDate.AddMonths(1).AddDays(-1);

        var result = await _pointsReportService.GetOverviewReportAsync(null, fromDate, toDate, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }
}
