namespace AlAshmar.Application.DTOs.Domain;

public record ReportPeriodFilter(
    DateTime? FromDate = null,
    DateTime? ToDate = null,
    ReportPeriodType PeriodType = ReportPeriodType.All
);

public enum ReportPeriodType
{
    All,
    Daily,
    Weekly,
    Monthly,
    Semester
}

public record StudentDailyReportDto(
    Guid StudentId,
    string StudentName,
    DateTime ReportDate,
    StudentAttendanceSummary AttendanceSummary,
    StudentMemorizationSummary MemorizationSummary,
    StudentPointsSummary PointsSummary,
    List<TeacherNoteDto> TeacherNotes
);

public record StudentWeeklyReportDto(
    Guid StudentId,
    string StudentName,
    DateTime WeekStart,
    DateTime WeekEnd,
    StudentAttendanceSummary AttendanceSummary,
    StudentMemorizationSummary MemorizationSummary,
    StudentPointsSummary PointsSummary,
    List<TeacherNoteDto> TeacherNotes
);

public record StudentMonthlyReportDto(
    Guid StudentId,
    string StudentName,
    int Month,
    int Year,
    StudentAttendanceSummary AttendanceSummary,
    StudentMemorizationSummary MemorizationSummary,
    StudentPointsSummary PointsSummary,
    List<TeacherNoteDto> TeacherNotes
);

public record StudentSemesterReportDto(
    Guid StudentId,
    string StudentName,
    Guid SemesterId,
    string SemesterName,
    StudentAttendanceSummary AttendanceSummary,
    StudentMemorizationSummary MemorizationSummary,
    StudentPointsSummary PointsSummary,
    List<TeacherNoteDto> TeacherNotes
);

public record StudentAttendanceSummary(
    int TotalDays,
    int PresentDays,
    int AbsentDays,
    double AttendancePercentage,
    List<StudentAttendancePeriodDto> AttendancePeriods
);

public record StudentAttendancePeriodDto(
    DateTime StartDate,
    DateTime EndDate
);

public record StudentMemorizationSummary(
    int QuranPagesMemorized,
    int HadithsMemorized,
    List<QuranProgressDto> QuranProgress,
    List<HadithProgressDto> HadithProgress
);

public record QuranProgressDto(
    int PageNumber,
    DateTime? MemorizedAt,
    string? Status,
    string? Notes,
    Guid? TeacherId,
    string? TeacherName
);

public record HadithProgressDto(
    Guid HadithId,
    string HadithText,
    DateTime? MemorizedAt,
    string? Status,
    string? Notes,
    Guid? TeacherId,
    string? TeacherName
);

public record StudentPointsSummary(
    int TotalPoints,
    int QuranPoints,
    int HadithPoints,
    int AttendancePoints,
    int BehaviorPoints,
    List<PointBreakdownDto> PointBreakdowns
);

public record PointBreakdownDto(
    Guid EventId,
    DateTime EventDate,
    int QuranPoints,
    int HadithPoints,
    int AttendancePoints,
    int BehaviorPoints,
    int TotalPoints,
    string? Notes
);

public record TeacherNoteDto(
    Guid? TeacherId,
    string TeacherName,
    DateTime NoteDate,
    string NoteType,
    string? Notes
);

public record TeacherDailyReportDto(
    Guid TeacherId,
    string TeacherName,
    DateTime ReportDate,
    TeacherAttendanceSummary AttendanceSummary,
    TeacherPointsSummary PointsSummary,
    List<StudentProgressUnderTeacherDto> StudentProgress
);

public record TeacherWeeklyReportDto(
    Guid TeacherId,
    string TeacherName,
    DateTime WeekStart,
    DateTime WeekEnd,
    TeacherAttendanceSummary AttendanceSummary,
    TeacherPointsSummary PointsSummary,
    List<StudentProgressUnderTeacherDto> StudentProgress
);

public record TeacherMonthlyReportDto(
    Guid TeacherId,
    string TeacherName,
    int Month,
    int Year,
    TeacherAttendanceSummary AttendanceSummary,
    TeacherPointsSummary PointsSummary,
    List<StudentProgressUnderTeacherDto> StudentProgress
);

public record TeacherSemesterReportDto(
    Guid TeacherId,
    string TeacherName,
    Guid SemesterId,
    string SemesterName,
    TeacherAttendanceSummary AttendanceSummary,
    TeacherPointsSummary PointsSummary,
    List<StudentProgressUnderTeacherDto> StudentProgress
);

public record TeacherAttendanceSummary(
    int TotalTeachingDays,
    int PresentDays,
    double AttendancePercentage,
    List<ClassAssignmentDto> ClassAssignments
);

public record ClassAssignmentDto(
    Guid ClassId,
    bool IsMainTeacher,
    List<string> StudentNames
);

public record TeacherPointsSummary(
    int TotalPointsGiven,
    int PointsByCategory,
    List<PointCategoryBreakdownDto> CategoryBreakdowns
);

public record PointCategoryBreakdownDto(
    Guid? CategoryId,
    string? CategoryType,
    int PointsCount,
    int TotalPointsValue
);

public record StudentProgressUnderTeacherDto(
    Guid StudentId,
    string StudentName,
    int QuranPagesMemorized,
    int HadithsMemorized,
    int PointsReceived,
    DateTime? LastMemorizationDate
);

public record ClassDailyReportDto(
    Guid ClassId,
    string ClassName,
    DateTime ReportDate,
    ClassAttendanceSummary AttendanceSummary,
    ClassPointsSummary PointsSummary,
    ClassMemorizationSummary MemorizationSummary,
    List<StudentBriefDto> Students
);

public record ClassWeeklyReportDto(
    Guid ClassId,
    string ClassName,
    DateTime WeekStart,
    DateTime WeekEnd,
    ClassAttendanceSummary AttendanceSummary,
    ClassPointsSummary PointsSummary,
    ClassMemorizationSummary MemorizationSummary,
    List<StudentBriefDto> Students
);

public record ClassMonthlyReportDto(
    Guid ClassId,
    string ClassName,
    int Month,
    int Year,
    ClassAttendanceSummary AttendanceSummary,
    ClassPointsSummary PointsSummary,
    ClassMemorizationSummary MemorizationSummary,
    List<StudentBriefDto> Students
);

public record ClassSemesterReportDto(
    Guid ClassId,
    string ClassName,
    Guid SemesterId,
    string SemesterName,
    ClassAttendanceSummary AttendanceSummary,
    ClassPointsSummary PointsSummary,
    ClassMemorizationSummary MemorizationSummary,
    List<StudentBriefDto> Students
);

public record ClassAttendanceSummary(
    int TotalStudents,
    int AverageAttendancePercentage,
    List<StudentAttendanceRecordDto> StudentAttendanceRecords
);

public record StudentAttendanceRecordDto(
    Guid StudentId,
    string StudentName,
    int PresentDays,
    int TotalDays,
    double AttendancePercentage
);

public record ClassPointsSummary(
    int TotalPoints,
    int AveragePointsPerStudent,
    List<StudentPointsRecordDto> StudentPointsRecords
);

public record StudentPointsRecordDto(
    Guid StudentId,
    string StudentName,
    int TotalPoints,
    int QuranPoints,
    int HadithPoints,
    int AttendancePoints,
    int BehaviorPoints
);

public record ClassMemorizationSummary(
    int TotalQuranPagesMemorized,
    int TotalHadithsMemorized,
    List<StudentMemorizationRecordDto> StudentMemorizationRecords
);

public record StudentMemorizationRecordDto(
    Guid StudentId,
    string StudentName,
    int QuranPagesCount,
    int HadithsCount,
    List<int> QuranPageNumbers,
    List<string> HadithTexts
);

public record StudentBriefDto(
    Guid Id,
    string Name
);

public record SemesterOverviewReportDto(
    Guid SemesterId,
    string SemesterName,
    DateTime StartDate,
    DateTime EndDate,
    SemesterStatisticsDto Statistics,
    List<ClassSummaryDto> ClassSummaries,
    List<TopStudentDto> TopStudents,
    List<TopTeacherDto> TopTeachers
);

public record SemesterStatisticsDto(
    int TotalStudents,
    int TotalTeachers,
    int TotalClasses,
    int TotalQuranPagesMemorized,
    int TotalHadithsMemorized,
    int TotalPointsGiven,
    double AverageAttendancePercentage
);

public record ClassSummaryDto(
    Guid ClassId,
    string ClassName,
    int StudentCount,
    double AverageAttendance,
    int TotalPoints
);

public record TopStudentDto(
    Guid StudentId,
    string StudentName,
    int TotalPoints,
    int QuranPagesMemorized,
    int HadithsMemorized,
    double AttendancePercentage
);

public record TopTeacherDto(
    Guid TeacherId,
    string TeacherName,
    int PointsGiven,
    int StudentsCount,
    double AttendancePercentage
);

public record AttendanceOverviewReportDto(
    DateTime FromDate,
    DateTime ToDate,
    AttendanceSummaryDto OverallSummary,
    List<StudentAttendanceDetailDto> StudentAttendanceDetails,
    List<TeacherAttendanceDetailDto> TeacherAttendanceDetails
);

public record AttendanceSummaryDto(
    int TotalDays,
    double StudentAverageAttendance,
    double TeacherAverageAttendance,
    int TotalStudentAbsences,
    int TotalTeacherAbsences
);

public record StudentAttendanceDetailDto(
    Guid StudentId,
    string StudentName,
    int PresentDays,
    int AbsentDays,
    double AttendancePercentage,
    List<DateTime> AbsenceDates
);

public record TeacherAttendanceDetailDto(
    Guid TeacherId,
    string TeacherName,
    int PresentDays,
    int AbsentDays,
    double AttendancePercentage,
    List<DateTime> AbsenceDates
);

public record PointsOverviewReportDto(
    Guid? SemesterId,
    string? SemesterName,
    DateTime? FromDate,
    DateTime? ToDate,
    PointsSummaryDto OverallSummary,
    List<StudentPointsDetailDto> StudentPointsDetails,
    List<TeacherPointsGivenDto> TeacherPointsGiven
);

public record PointsSummaryDto(
    int TotalPoints,
    int QuranPoints,
    int HadithPoints,
    int AttendancePoints,
    int BehaviorPoints,
    int TotalEvents
);

public record StudentPointsDetailDto(
    Guid StudentId,
    string StudentName,
    int TotalPoints,
    int QuranPoints,
    int HadithPoints,
    int AttendancePoints,
    int BehaviorPoints,
    List<PointEventDto> PointEvents
);

public record PointEventDto(
    Guid EventId,
    DateTime EventDate,
    int QuranPoints,
    int HadithPoints,
    int AttendancePoints,
    int BehaviorPoints,
    int TotalPoints,
    string? Notes
);

public record TeacherPointsGivenDto(
    Guid TeacherId,
    string TeacherName,
    int TotalPointsGiven,
    int PointsByCategory,
    int StudentsCount
);
