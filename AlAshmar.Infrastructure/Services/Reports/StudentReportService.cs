using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Interfaces.Reports;
using AlAshmar.Application.Repos;
using AlAshmar.Domain.Entities.Academic;
using AlAshmar.Domain.Entities.Students;
using AlAshmar.Domain.Entities.Teachers;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Infrastructure.Services.Reports;

public class StudentReportService : IStudentReportService
{
    private readonly IRepositoryBase<Student, Guid> _studentRepository;
    private readonly IRepositoryBase<StudentAttendance, Guid> _attendanceRepository;
    private readonly IRepositoryBase<StudentHadith, Guid> _hadithRepository;
    private readonly IRepositoryBase<StudentQuraanPage, Guid> _quraanPageRepository;
    private readonly IRepositoryBase<StudentClassEventsPoint, Guid> _pointsRepository;

    public StudentReportService(
        IRepositoryBase<Student, Guid> studentRepository,
        IRepositoryBase<StudentAttendance, Guid> attendanceRepository,
        IRepositoryBase<StudentHadith, Guid> hadithRepository,
        IRepositoryBase<StudentQuraanPage, Guid> quraanPageRepository,
        IRepositoryBase<StudentClassEventsPoint, Guid> pointsRepository)
    {
        _studentRepository = studentRepository;
        _attendanceRepository = attendanceRepository;
        _hadithRepository = hadithRepository;
        _quraanPageRepository = quraanPageRepository;
        _pointsRepository = pointsRepository;
    }

    public async Task<Result<StudentDailyReportDto>> GetDailyReportAsync(Guid studentId, DateTime date, CancellationToken cancellationToken = default)
    {
        var studentResult = await _studentRepository.GetByIdAsync(studentId);
        if (studentResult.Value == null)
            return ApplicationErrors.StudentNotFound;

        var student = studentResult.Value;

        // Get memorization for the day
        var hadithsToday = await _hadithRepository.GetAllAsync(
            filter: h => h.StudentId == studentId && h.MemorizedAt.HasValue &&
                        h.MemorizedAt.Value.Date == date.Date);

        var quraanPagesToday = await _quraanPageRepository.GetAllAsync(
            filter: q => q.StudentId == studentId && q.MemorizedAt.HasValue &&
                      q.MemorizedAt.Value.Date == date.Date);

        var hadithList = hadithsToday.Value?.ToList() ?? new List<StudentHadith>();
        var quraanPageList = quraanPagesToday.Value?.ToList() ?? new List<StudentQuraanPage>();

        var teacherNotes = new List<TeacherNoteDto>();
        teacherNotes.AddRange(hadithList.Where(h => !string.IsNullOrEmpty(h.Notes)).Select(h =>
            new TeacherNoteDto(h.TeacherId, h.Teacher?.Name ?? "", h.MemorizedAt ?? date, "Hadith", h.Notes)));
        teacherNotes.AddRange(quraanPageList.Where(q => !string.IsNullOrEmpty(q.Notes)).Select(q =>
            new TeacherNoteDto(q.TeacherId, q.Teacher?.Name ?? "", q.MemorizedAt ?? date, "Quran", q.Notes)));

        return new StudentDailyReportDto(
            student.Id,
            student.Name,
            date,
            new StudentAttendanceSummary(0, 0, 0, 0, new List<StudentAttendancePeriodDto>()),
            new StudentMemorizationSummary(
                quraanPageList.Count,
                hadithList.Count,
                quraanPageList.Select(q => new QuranProgressDto(q.PageNumber, q.MemorizedAt, q.Status, q.Notes, q.TeacherId, q.Teacher?.Name)).ToList(),
                hadithList.Select(h => new HadithProgressDto(h.HadithId, h.Hadith?.Text ?? "", h.MemorizedAt, h.Status, h.Notes, h.TeacherId, h.Teacher?.Name)).ToList()),
            new StudentPointsSummary(0, 0, 0, 0, 0, new List<PointBreakdownDto>()),
            teacherNotes
        );
    }

    public async Task<Result<StudentWeeklyReportDto>> GetWeeklyReportAsync(Guid studentId, DateTime weekStart, CancellationToken cancellationToken = default)
    {
        var weekEnd = weekStart.AddDays(6);
        var studentResult = await _studentRepository.GetByIdAsync(studentId);
        if (studentResult.Value == null)
            return ApplicationErrors.StudentNotFound;

        var student = studentResult.Value;

        var hadithsWeek = await _hadithRepository.GetAllAsync(
            filter: h => h.StudentId == studentId && h.MemorizedAt.HasValue &&
                        h.MemorizedAt.Value.Date >= weekStart.Date && h.MemorizedAt.Value.Date <= weekEnd.Date);

        var quraanPagesWeek = await _quraanPageRepository.GetAllAsync(
            filter: q => q.StudentId == studentId && q.MemorizedAt.HasValue &&
                      q.MemorizedAt.Value.Date >= weekStart.Date && q.MemorizedAt.Value.Date <= weekEnd.Date);

        var hadithList = hadithsWeek.Value?.ToList() ?? new List<StudentHadith>();
        var quraanPageList = quraanPagesWeek.Value?.ToList() ?? new List<StudentQuraanPage>();

        return new StudentWeeklyReportDto(
            student.Id,
            student.Name,
            weekStart,
            weekEnd,
            new StudentAttendanceSummary(0, 0, 0, 0, new List<StudentAttendancePeriodDto>()),
            new StudentMemorizationSummary(
                quraanPageList.Count,
                hadithList.Count,
                quraanPageList.Select(q => new QuranProgressDto(q.PageNumber, q.MemorizedAt, q.Status, q.Notes, q.TeacherId, q.Teacher?.Name)).ToList(),
                hadithList.Select(h => new HadithProgressDto(h.HadithId, h.Hadith?.Text ?? "", h.MemorizedAt, h.Status, h.Notes, h.TeacherId, h.Teacher?.Name)).ToList()),
            new StudentPointsSummary(0, 0, 0, 0, 0, new List<PointBreakdownDto>()),
            new List<TeacherNoteDto>()
        );
    }

    public async Task<Result<StudentMonthlyReportDto>> GetMonthlyReportAsync(Guid studentId, int month, int year, CancellationToken cancellationToken = default)
    {
        var monthStart = new DateTime(year, month, 1);
        var monthEnd = monthStart.AddMonths(1).AddDays(-1);

        var studentResult = await _studentRepository.GetByIdAsync(studentId);
        if (studentResult.Value == null)
            return ApplicationErrors.StudentNotFound;

        var student = studentResult.Value;

        var hadithsMonth = await _hadithRepository.GetAllAsync(
            filter: h => h.StudentId == studentId && h.MemorizedAt.HasValue &&
                        h.MemorizedAt.Value.Date >= monthStart.Date && h.MemorizedAt.Value.Date <= monthEnd.Date);

        var quraanPagesMonth = await _quraanPageRepository.GetAllAsync(
            filter: q => q.StudentId == studentId && q.MemorizedAt.HasValue &&
                      q.MemorizedAt.Value.Date >= monthStart.Date && q.MemorizedAt.Value.Date <= monthEnd.Date);

        var hadithList = hadithsMonth.Value?.ToList() ?? new List<StudentHadith>();
        var quraanPageList = quraanPagesMonth.Value?.ToList() ?? new List<StudentQuraanPage>();

        return new StudentMonthlyReportDto(
            student.Id,
            student.Name,
            month,
            year,
            new StudentAttendanceSummary(0, 0, 0, 0, new List<StudentAttendancePeriodDto>()),
            new StudentMemorizationSummary(
                quraanPageList.Count,
                hadithList.Count,
                quraanPageList.Select(q => new QuranProgressDto(q.PageNumber, q.MemorizedAt, q.Status, q.Notes, q.TeacherId, q.Teacher?.Name)).ToList(),
                hadithList.Select(h => new HadithProgressDto(h.HadithId, h.Hadith?.Text ?? "", h.MemorizedAt, h.Status, h.Notes, h.TeacherId, h.Teacher?.Name)).ToList()),
            new StudentPointsSummary(0, 0, 0, 0, 0, new List<PointBreakdownDto>()),
            new List<TeacherNoteDto>()
        );
    }

    public async Task<Result<StudentSemesterReportDto>> GetSemesterReportAsync(Guid studentId, Guid semesterId, CancellationToken cancellationToken = default)
    {
        var studentResult = await _studentRepository.GetByIdAsync(studentId);
        if (studentResult.Value == null)
            return ApplicationErrors.StudentNotFound;

        var student = studentResult.Value;

        var pointsResult = await _pointsRepository.GetAllAsync(
            filter: p => p.StudentId == studentId && p.SmesterId == semesterId);

        var semester = pointsResult.Value?.FirstOrDefault()?.Semester;
        if (semester == null)
            return ApplicationErrors.SemesterNotFound;

        var hadithsSemester = await _hadithRepository.GetAllAsync(
            filter: h => h.StudentId == studentId && h.MemorizedAt.HasValue &&
                        h.MemorizedAt.Value.Date >= semester.StartDate.Date && h.MemorizedAt.Value.Date <= semester.EndDate.Date);

        var quraanPagesSemester = await _quraanPageRepository.GetAllAsync(
            filter: q => q.StudentId == studentId && q.MemorizedAt.HasValue &&
                      q.MemorizedAt.Value.Date >= semester.StartDate.Date && q.MemorizedAt.Value.Date <= semester.EndDate.Date);

        var hadithList = hadithsSemester.Value?.ToList() ?? new List<StudentHadith>();
        var quraanPageList = quraanPagesSemester.Value?.ToList() ?? new List<StudentQuraanPage>();

        return new StudentSemesterReportDto(
            student.Id,
            student.Name,
            semester.Id,
            semester.Name,
            new StudentAttendanceSummary(0, 0, 0, 0, new List<StudentAttendancePeriodDto>()),
            new StudentMemorizationSummary(
                quraanPageList.Count,
                hadithList.Count,
                quraanPageList.Select(q => new QuranProgressDto(q.PageNumber, q.MemorizedAt, q.Status, q.Notes, q.TeacherId, q.Teacher?.Name)).ToList(),
                hadithList.Select(h => new HadithProgressDto(h.HadithId, h.Hadith?.Text ?? "", h.MemorizedAt, h.Status, h.Notes, h.TeacherId, h.Teacher?.Name)).ToList()),
            new StudentPointsSummary(0, 0, 0, 0, 0, new List<PointBreakdownDto>()),
            new List<TeacherNoteDto>()
        );
    }
}
