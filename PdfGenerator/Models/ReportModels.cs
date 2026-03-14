namespace PdfGenerator.Models;




public class ReportBaseModel
{
    public string ReportTitle { get; set; } = string.Empty;
    public DateTime GeneratedDate { get; set; } = DateTime.Now;
    public string? CompanyName { get; set; }
    public string? LogoPath { get; set; }
}




public class StudentDailyReportModel : ReportBaseModel
{
    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public DateTime ReportDate { get; set; }
    public string PeriodType { get; set; } = string.Empty;
    public int QuranPagesMemorized { get; set; }
    public int HadithsMemorized { get; set; }
    public int TotalPoints { get; set; }
    public double AttendancePercentage { get; set; }
    public List<string> TeacherNotes { get; set; } = new();
}




public class TeacherDailyReportModel : ReportBaseModel
{
    public Guid TeacherId { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public DateTime ReportDate { get; set; }
    public string PeriodType { get; set; } = string.Empty;
    public int TotalPointsGiven { get; set; }
    public int StudentsCount { get; set; }
    public double AttendancePercentage { get; set; }
}




public class ClassDailyReportModel : ReportBaseModel
{
    public Guid ClassId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public DateTime ReportDate { get; set; }
    public string PeriodType { get; set; } = string.Empty;
    public int TotalStudents { get; set; }
    public int TotalQuranPagesMemorized { get; set; }
    public int TotalHadithsMemorized { get; set; }
    public int TotalPoints { get; set; }
    public double AverageAttendancePercentage { get; set; }
}




public class AttendanceReportModel : ReportBaseModel
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int TotalDays { get; set; }
    public double StudentAverageAttendance { get; set; }
    public double TeacherAverageAttendance { get; set; }
    public int TotalStudentAbsences { get; set; }
    public int TotalTeacherAbsences { get; set; }
}




public class PointsReportModel : ReportBaseModel
{
    public Guid? SemesterId { get; set; }
    public string? SemesterName { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int TotalPoints { get; set; }
    public int QuranPoints { get; set; }
    public int HadithPoints { get; set; }
    public int AttendancePoints { get; set; }
    public int BehaviorPoints { get; set; }
    public int TotalEvents { get; set; }
}




public class SemesterOverviewReportModel : ReportBaseModel
{
    public Guid SemesterId { get; set; }
    public string SemesterName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalStudents { get; set; }
    public int TotalTeachers { get; set; }
    public int TotalClasses { get; set; }
    public int TotalQuranPagesMemorized { get; set; }
    public int TotalHadithsMemorized { get; set; }
    public int TotalPointsGiven { get; set; }
    public double AverageAttendancePercentage { get; set; }
}
