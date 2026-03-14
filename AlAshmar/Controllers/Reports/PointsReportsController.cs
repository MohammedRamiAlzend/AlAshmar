using AlAshmar.Application.Interfaces.Reports;

namespace AlAshmar.Controllers.Reports;




public class PointsReportsController : ReportsBaseController
{
    private readonly IPointsReportService _pointsReportService;

    public PointsReportsController(IPointsReportService pointsReportService)
    {
        _pointsReportService = pointsReportService;
    }








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
