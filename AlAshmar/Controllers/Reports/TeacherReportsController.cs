using AlAshmar.Application.Interfaces.Reports;

namespace AlAshmar.Controllers.Reports;




public class TeacherReportsController : ReportsBaseController
{
    private readonly ITeacherReportService _teacherReportService;

    public TeacherReportsController(ITeacherReportService teacherReportService)
    {
        _teacherReportService = teacherReportService;
    }







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


        return Ok(new PagedList<PointCategoryBreakdownDto>(new List<PointCategoryBreakdownDto>(), 0, page, pageSize));
    }









    [HttpGet("teachers/{teacherId:guid}/students-progress")]
    public async Task<ActionResult<PagedList<StudentProgressUnderTeacherDto>>> GetStudentProgress(
        [FromRoute] Guid teacherId,
        [FromQuery] Guid? semesterId = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {

        return Ok(new PagedList<StudentProgressUnderTeacherDto>(new List<StudentProgressUnderTeacherDto>(), 0, page, pageSize));
    }
}
