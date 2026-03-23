using AlAshmar.Application.Interfaces.Reports;
using AlAshmar.Application.Repos;
using AlAshmar.Domain.DTOs.Domain;
using AlAshmar.Domain.Entities.Academic;
using AlAshmar.Domain.Entities.Students;
using AlAshmar.Domain.Entities.Teachers;

namespace AlAshmar.Infrastructure.Services.Reports;

public class PointsReportService : IPointsReportService
{
    private readonly IRepositoryBase<Point, Guid> _pointRepository;
    private readonly IRepositoryBase<StudentClassEventsPoint, Guid> _studentPointsRepository;
    private readonly IRepositoryBase<Student, Guid> _studentRepository;
    private readonly IRepositoryBase<Teacher, Guid> _teacherRepository;

    public PointsReportService(
        IRepositoryBase<Point, Guid> pointRepository,
        IRepositoryBase<StudentClassEventsPoint, Guid> studentPointsRepository,
        IRepositoryBase<Student, Guid> studentRepository,
        IRepositoryBase<Teacher, Guid> teacherRepository)
    {
        _pointRepository = pointRepository;
        _studentPointsRepository = studentPointsRepository;
        _studentRepository = studentRepository;
        _teacherRepository = teacherRepository;
    }

    public async Task<Result<PointsOverviewReportDto>> GetOverviewReportAsync(
        Guid? semesterId = null, DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default)
    {
        var pointsQuery = await _pointRepository.GetAllAsync(
            filter: p => (!semesterId.HasValue || p.SmesterId == semesterId.Value));

        var points = pointsQuery.Value?.ToList() ?? new List<Point>();

        var overallSummary = new PointsSummaryDto(
            points.Sum(p => p.PointValue),
            0, 0, 0, 0,
            points.Count
        );

        var studentPointsDetails = points
            .GroupBy(p => p.StudentId)
            .Select(g => new StudentPointsDetailDto(
                g.Key,
                "Student",
                g.Sum(p => p.PointValue),
                0, 0, 0, 0,
                new List<PointEventDto>()
            ))
            .ToList();

        var teacherPointsGiven = points
            .GroupBy(p => p.GivenByTeacherId)
            .Where(g => g.Key.HasValue)
            .Select(g => new TeacherPointsGivenDto(
                g.Key.Value,
                "Teacher",
                g.Sum(p => p.PointValue),
                g.Count(),
                g.Select(p => p.StudentId).Distinct().Count()
            ))
            .ToList();

        return new PointsOverviewReportDto(
            semesterId,
            semesterId.HasValue ? "Semester" : null,
            fromDate,
            toDate,
            overallSummary,
            studentPointsDetails,
            teacherPointsGiven
        );
    }

    public async Task<Result<PagedList<StudentPointsDetailDto>>> GetStudentPointsReportAsync(
        Guid? semesterId = null, DateTime? fromDate = null, DateTime? toDate = null,
        int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var pointsQuery = await _studentPointsRepository.GetAllAsync(
            filter: p => (!semesterId.HasValue || p.SmesterId == semesterId.Value));

        var studentPoints = pointsQuery.Value?.ToList() ?? new List<StudentClassEventsPoint>();

        var studentIds = studentPoints.Select(p => p.StudentId).Distinct().ToList();
        var studentsDict = new Dictionary<Guid, string>();
        foreach (var studentId in studentIds)
        {
            var studentResult = await _studentRepository.GetByIdAsync(studentId);
            if (studentResult.Value != null)
                studentsDict[studentId] = studentResult.Value.Name;
        }

        var studentPointsDetails = studentPoints
            .GroupBy(p => p.StudentId)
            .Select(g => new StudentPointsDetailDto(
                g.Key,
                studentsDict.TryGetValue(g.Key, out var name) ? name : "Student",
                g.Sum(p => p.TotalPoints),
                g.Sum(p => p.QuranPoints),
                g.Sum(p => p.HadithPoints),
                g.Sum(p => p.AttendancePoints),
                g.Sum(p => p.BehaviorPoints),
                g.Select(p => new PointEventDto(
                    p.EventId,
                    DateTime.Today,
                    p.QuranPoints,
                    p.HadithPoints,
                    p.AttendancePoints,
                    p.BehaviorPoints,
                    p.TotalPoints,
                    null
                )).ToList()
            ))
            .ToList();

        var totalItems = studentPointsDetails.Count;
        var pagedData = studentPointsDetails
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return PagedList<StudentPointsDetailDto>.Create(pagedData, totalItems, page, pageSize);
    }

    public async Task<Result<PagedList<TeacherPointsGivenDto>>> GetTeacherPointsReportAsync(
        Guid? semesterId = null, DateTime? fromDate = null, DateTime? toDate = null,
        int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var pointsQuery = await _pointRepository.GetAllAsync(
            filter: p => (!semesterId.HasValue || p.SmesterId == semesterId.Value) && p.GivenByTeacherId != null);

        var points = pointsQuery.Value?.ToList() ?? new List<Point>();

        var teacherIds = points.Where(p => p.GivenByTeacherId.HasValue).Select(p => p.GivenByTeacherId.Value).Distinct().ToList();
        var teachersDict = new Dictionary<Guid, string>();
        foreach (var teacherId in teacherIds)
        {
            var teacherResult = await _teacherRepository.GetByIdAsync(teacherId);
            if (teacherResult.Value != null)
                teachersDict[teacherId] = teacherResult.Value.Name;
        }

        var teacherPointsGiven = points
            .GroupBy(p => p.GivenByTeacherId)
            .Where(g => g.Key.HasValue)
            .Select(g => new TeacherPointsGivenDto(
                g.Key.Value,
                teachersDict.TryGetValue(g.Key.Value, out var name) ? name : "Teacher",
                g.Sum(p => p.PointValue),
                g.Count(),
                g.Select(p => p.StudentId).Distinct().Count()
            ))
            .ToList();

        var totalItems = teacherPointsGiven.Count;
        var pagedData = teacherPointsGiven
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return PagedList<TeacherPointsGivenDto>.Create(pagedData, totalItems, page, pageSize);
    }
}
