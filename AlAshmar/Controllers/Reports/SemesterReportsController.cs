using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Interfaces.Reports;
using Microsoft.AspNetCore.Mvc;

namespace AlAshmar.Controllers.Reports;

/// <summary>
/// Controller for semester-wide reports and statistics.
/// </summary>
public class SemesterReportsController : ReportsBaseController
{
    private readonly ISemesterReportService _semesterReportService;

    public SemesterReportsController(ISemesterReportService semesterReportService)
    {
        _semesterReportService = semesterReportService;
    }

    /// <summary>
    /// Get comprehensive overview report for a semester.
    /// </summary>
    /// <param name="semesterId">The semester ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("semesters/{semesterId:guid}/overview")]
    public async Task<ActionResult<SemesterOverviewReportDto>> GetOverviewReport(
        [FromRoute] Guid semesterId,
        CancellationToken cancellationToken = default)
    {
        var result = await _semesterReportService.GetOverviewReportAsync(semesterId, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok(result.Value);
    }

    /// <summary>
    /// Get statistics for a semester.
    /// </summary>
    /// <param name="semesterId">The semester ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("semesters/{semesterId:guid}/statistics")]
    public async Task<ActionResult<SemesterStatisticsDto>> GetStatistics(
        [FromRoute] Guid semesterId,
        CancellationToken cancellationToken = default)
    {
        var result = await _semesterReportService.GetOverviewReportAsync(semesterId, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok(result.Value.Statistics);
    }

    /// <summary>
    /// Get top performing students for a semester.
    /// </summary>
    /// <param name="semesterId">The semester ID</param>
    /// <param name="top">Number of top students to return (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("semesters/{semesterId:guid}/top-students")]
    public async Task<ActionResult<List<TopStudentDto>>> GetTopStudents(
        [FromRoute] Guid semesterId,
        [FromQuery] int top = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _semesterReportService.GetOverviewReportAsync(semesterId, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok(result.Value.TopStudents.Take(top).ToList());
    }

    /// <summary>
    /// Get top performing teachers for a semester.
    /// </summary>
    /// <param name="semesterId">The semester ID</param>
    /// <param name="top">Number of top teachers to return (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("semesters/{semesterId:guid}/top-teachers")]
    public async Task<ActionResult<List<TopTeacherDto>>> GetTopTeachers(
        [FromRoute] Guid semesterId,
        [FromQuery] int top = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _semesterReportService.GetOverviewReportAsync(semesterId, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok(result.Value.TopTeachers.Take(top).ToList());
    }

    /// <summary>
    /// Get class summaries for a semester.
    /// </summary>
    /// <param name="semesterId">The semester ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpGet("semesters/{semesterId:guid}/classes")]
    public async Task<ActionResult<List<ClassSummaryDto>>> GetClassSummaries(
        [FromRoute] Guid semesterId,
        CancellationToken cancellationToken = default)
    {
        var result = await _semesterReportService.GetOverviewReportAsync(semesterId, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok(result.Value.ClassSummaries);
    }
}
