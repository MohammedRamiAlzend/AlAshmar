using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Interfaces.Reports;
using AlAshmar.Application.Repos;
using AlAshmar.Domain.Entities.Academic;

namespace AlAshmar.Application.Services.Reports;

public class SemesterReportService : ISemesterReportService
{
    private readonly IRepositoryBase<Semester, Guid> _semesterRepository;

    public SemesterReportService(IRepositoryBase<Semester, Guid> semesterRepository)
    {
        _semesterRepository = semesterRepository;
    }

    public async Task<Result<SemesterOverviewReportDto>> GetOverviewReportAsync(Guid semesterId, CancellationToken cancellationToken = default)
    {
        var semesterResult = await _semesterRepository.GetByIdAsync(semesterId);
        if (semesterResult.Value == null)
            return Error.NotFound("Semester", semesterId.ToString());

        var semester = semesterResult.Value;

        var statistics = new SemesterStatisticsDto(
            0, 0, 0, 0, 0, 0, 0
        );

        return new SemesterOverviewReportDto(
            semester.Id,
            semester.Name,
            semester.StartDate,
            semester.EndDate,
            statistics,
            new List<ClassSummaryDto>(),
            new List<TopStudentDto>(),
            new List<TopTeacherDto>()
        );
    }
}
