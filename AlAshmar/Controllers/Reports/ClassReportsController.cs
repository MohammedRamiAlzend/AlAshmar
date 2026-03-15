using AlAshmar.Application.Interfaces.Reports;

namespace AlAshmar.Controllers.Reports;

public class ClassReportsController : ReportsBaseController
{
    private readonly IClassReportService _classReportService;

    public ClassReportsController(IClassReportService classReportService)
    {
        _classReportService = classReportService;
    }

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
