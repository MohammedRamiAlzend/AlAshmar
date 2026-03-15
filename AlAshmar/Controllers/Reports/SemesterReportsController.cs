using AlAshmar.Application.Interfaces.Reports;

namespace AlAshmar.Controllers.Reports;

public class SemesterReportsController : ReportsBaseController
{
    private readonly ISemesterReportService _semesterReportService;

    public SemesterReportsController(ISemesterReportService semesterReportService)
    {
        _semesterReportService = semesterReportService;
    }

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
