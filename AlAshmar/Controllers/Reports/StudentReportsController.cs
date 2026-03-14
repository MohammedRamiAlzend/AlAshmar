using AlAshmar.Application.Interfaces.Reports;

namespace AlAshmar.Controllers.Reports;




public class StudentReportsController : ReportsBaseController
{
    private readonly IStudentReportService _studentReportService;

    public StudentReportsController(IStudentReportService studentReportService)
    {
        _studentReportService = studentReportService;
    }







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








    [HttpGet("students/{studentId:guid}/daily/export")]
    public async Task<IActionResult> ExportDailyReport(
        [FromRoute] Guid studentId,
        [FromQuery] DateTime? date = null,
        [FromQuery] string format = "json",
        CancellationToken cancellationToken = default)
    {
        var reportDate = date ?? DateTime.Today;
        var result = await _studentReportService.GetDailyReportAsync(studentId, reportDate, cancellationToken);

        if (result.IsError)
            return BadRequest(result.Errors);

        var fileName = $"StudentReport_{studentId}_{reportDate:yyyyMMdd}";

        return format.ToLower() switch
        {
            "json" => DownloadAsJson(result.Value, fileName),
            "xml" => DownloadAsXml(result.Value, fileName),
            _ => DownloadAsJson(result.Value, fileName)
        };
    }
}
