using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Interfaces.Reports;
using Microsoft.AspNetCore.Mvc;

namespace AlAshmar.Controllers.Reports;

/// <summary>
/// Controller for teacher-related reports including attendance, given points, and student progress.
/// </summary>
public class TeacherReportsController : ReportsBaseController
{
    private readonly ITeacherReportService _teacherReportService;

    public TeacherReportsController(ITeacherReportService teacherReportService)
    {
        _teacherReportService = teacherReportService;
    }

    /// <summary>
    /// Get daily report for a teacher including attendance, points given, and student progress.
    /// </summary>
    /// <param name="teacherId">The teacher ID</param>
    /// <param name="date">The report date (defaults to today)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("teachers/{teacherId:guid}/daily")]
    public async Task<ActionResult<TeacherDailyReportDto>> GetDailyReport(
        [FromRoute] Guid teacherId,
        [FromQuery] DateTime? date = null,
        CancellationToken cancellationToken = default)
    {
        var reportDate = date ?? DateTime.Today;
        var result = await _teacherReportService.GetDailyReportAsync(teacherId, reportDate, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    /// <summary>
    /// Get weekly report for a teacher including attendance, points given, and student progress.
    /// </summary>
    /// <param name="teacherId">The teacher ID</param>
    /// <param name="weekStart">The start of the week (defaults to current week's Monday)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("teachers/{teacherId:guid}/weekly")]
    public async Task<ActionResult<TeacherWeeklyReportDto>> GetWeeklyReport(
        [FromRoute] Guid teacherId,
        [FromQuery] DateTime? weekStart = null,
        CancellationToken cancellationToken = default)
    {
        var weekStartDate = weekStart ?? GetWeekStart(DateTime.Today);
        var result = await _teacherReportService.GetWeeklyReportAsync(teacherId, weekStartDate, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    /// <summary>
    /// Get monthly report for a teacher including attendance, points given, and student progress.
    /// </summary>
    /// <param name="teacherId">The teacher ID</param>
    /// <param name="month">The month (defaults to current month)</param>
    /// <param name="year">The year (defaults to current year)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("teachers/{teacherId:guid}/monthly")]
    public async Task<ActionResult<TeacherMonthlyReportDto>> GetMonthlyReport(
        [FromRoute] Guid teacherId,
        [FromQuery] int? month = null,
        [FromQuery] int? year = null,
        CancellationToken cancellationToken = default)
    {
        var reportMonth = month ?? DateTime.Now.Month;
        var reportYear = year ?? DateTime.Now.Year;

        var result = await _teacherReportService.GetMonthlyReportAsync(teacherId, reportMonth, reportYear, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    /// <summary>
    /// Get semester report for a teacher including attendance, points given, and student progress.
    /// </summary>
    /// <param name="teacherId">The teacher ID</param>
    /// <param name="semesterId">The semester ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("teachers/{teacherId:guid}/semesters/{semesterId:guid}")]
    public async Task<ActionResult<TeacherSemesterReportDto>> GetSemesterReport(
        [FromRoute] Guid teacherId,
        [FromRoute] Guid semesterId,
        CancellationToken cancellationToken = default)
    {
        var result = await _teacherReportService.GetSemesterReportAsync(teacherId, semesterId, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    /// <summary>
    /// Get points given by a teacher with optional filtering.
    /// </summary>
    /// <param name="teacherId">The teacher ID</param>
    /// <param name="fromDate">Optional from date</param>
    /// <param name="toDate">Optional to date</param>
    /// <param name="semesterId">Optional semester ID</param>
    /// <param name="page">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("teachers/{teacherId:guid}/points")]
    public async Task<ActionResult<PagedList<PointCategoryBreakdownDto>>> GetPointsGiven(
        [FromRoute] Guid teacherId,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] Guid? semesterId = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        if (!ValidateDateRange(fromDate, toDate, out var errorMessage))
            return BadRequest(errorMessage);

        // Note: This would need a dedicated service method for full implementation
        return Ok(new PagedList<PointCategoryBreakdownDto>(new List<PointCategoryBreakdownDto>(), 0, page, pageSize));
    }

    /// <summary>
    /// Get students progress under a teacher.
    /// </summary>
    /// <param name="teacherId">The teacher ID</param>
    /// <param name="semesterId">Optional semester ID</param>
    /// <param name="page">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("teachers/{teacherId:guid}/students-progress")]
    public async Task<ActionResult<PagedList<StudentProgressUnderTeacherDto>>> GetStudentProgress(
        [FromRoute] Guid teacherId,
        [FromQuery] Guid? semesterId = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        // Note: This would need a dedicated service method for full implementation
        return Ok(new PagedList<StudentProgressUnderTeacherDto>(new List<StudentProgressUnderTeacherDto>(), 0, page, pageSize));
    }
}
