using PdfGenerator.Interfaces;
using PdfGenerator.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PdfGenerator.Templates;

/// <summary>
/// Base document template for all reports.
/// </summary>
public class ReportDocumentBase : IDocument
{
    protected readonly ReportBaseModel _model;
    protected readonly IPdfOptions _options;

    public ReportDocumentBase(ReportBaseModel model, IPdfOptions options)
    {
        _model = model;
        _options = options;
    }

    public DocumentMetadata GetMetadata() => new()
    {
        Title = _model.ReportTitle,
        Author = _model.CompanyName ?? "AlAshmar",
        Subject = _model.ReportTitle,
        Keywords = "report, alashmar"
    };

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(_options.PageSize == "A4" ? PageSizes.A4 : PageSizes.Letter);
            page.MarginVertical(_options.MarginTop);
            page.MarginHorizontal(_options.MarginLeft);
            page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

            if (_options.IncludeHeader)
            {
                page.Header().Element(ComposeHeader);
            }

            page.Content().PaddingVertical(10).Element(ComposeContent);

            if (_options.IncludeFooter)
            {
                page.Footer().Element(ComposeFooter);
            }
        });
    }

    protected virtual void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            if (!string.IsNullOrEmpty(_model.LogoPath))
            {
                row.RelativeItem().AlignLeft().Text(_model.CompanyName ?? "AlAshmar")
                    .FontSize(16).Bold().FontColor(Colors.Blue.Medium);
            }
            else
            {
                row.RelativeItem();
            }

            row.ConstantItem(150).AlignRight().Column(col =>
            {
                col.Item().Text(_model.ReportTitle).FontSize(12).Bold();
                col.Item().Text($"Generated: {_model.GeneratedDate:yyyy-MM-dd HH:mm}").FontSize(8);
            });
        });

        container.PaddingTop(10).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
    }

    protected virtual void ComposeContent(IContainer container)
    {
        // Override in derived classes
    }

    protected virtual void ComposeFooter(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().Text(text =>
            {
                text.Span("Page ").FontSize(8);
                text.CurrentPageNumber().FontSize(8);
                text.Span(" of ").FontSize(8);
                text.TotalPages().FontSize(8);
            });

            row.RelativeItem().AlignCenter().Text(_model.CompanyName ?? "AlAshmar").FontSize(8).FontColor(Colors.Grey.Medium);
        });
    }
}

/// <summary>
/// Document template for student reports.
/// </summary>
public class StudentReportDocument : ReportDocumentBase
{
    private readonly StudentDailyReportModel _studentModel;

    public StudentReportDocument(StudentDailyReportModel model, IPdfOptions options)
        : base(model, options)
    {
        _studentModel = model;
    }

    protected override void ComposeContent(IContainer container)
    {
        container.Column(col =>
        {
            // Student Information
            col.Item().PaddingBottom(10).Row(row =>
            {
                row.RelativeItem().Column(info =>
                {
                    info.Item().Text($"Student: {_studentModel.StudentName}").FontSize(12).Bold();
                    info.Item().Text($"Report Period: {_studentModel.PeriodType}").FontSize(10);
                    info.Item().Text($"Date: {_studentModel.ReportDate:yyyy-MM-dd}").FontSize(10);
                });
            });

            // Summary Statistics
            col.Item().PaddingBottom(10).Border(1).BorderColor(Colors.Grey.Lighten1).Padding(10).Column(stats =>
            {
                stats.Item().Text("Summary").FontSize(12).Bold().FontColor(Colors.Blue.Medium);
                stats.Item().PaddingTop(5).Row(summary =>
                {
                    summary.RelativeItem().Column(quran =>
                    {
                        quran.Item().Text("Quran Pages").FontSize(9).FontColor(Colors.Grey.Medium);
                        quran.Item().Text(_studentModel.QuranPagesMemorized.ToString()).FontSize(14).Bold();
                    });
                    summary.RelativeItem().Column(hadith =>
                    {
                        hadith.Item().Text("Hadiths").FontSize(9).FontColor(Colors.Grey.Medium);
                        hadith.Item().Text(_studentModel.HadithsMemorized.ToString()).FontSize(14).Bold();
                    });
                    summary.RelativeItem().Column(points =>
                    {
                        points.Item().Text("Total Points").FontSize(9).FontColor(Colors.Grey.Medium);
                        points.Item().Text(_studentModel.TotalPoints.ToString()).FontSize(14).Bold();
                    });
                    summary.RelativeItem().Column(attendance =>
                    {
                        attendance.Item().Text("Attendance").FontSize(9).FontColor(Colors.Grey.Medium);
                        attendance.Item().Text($"{_studentModel.AttendancePercentage:F1}%").FontSize(14).Bold();
                    });
                });
            });

            // Teacher Notes
            if (_studentModel.TeacherNotes.Any())
            {
                col.Item().PaddingTop(10).Column(notes =>
                {
                    notes.Item().Text("Teacher Notes").FontSize(12).Bold().FontColor(Colors.Blue.Medium);
                    notes.Item().PaddingTop(5).Column(noteList =>
                    {
                        foreach (var note in _studentModel.TeacherNotes)
                        {
                            noteList.Item().PaddingBottom(3).Text($"• {note}").FontSize(10);
                        }
                    });
                });
            }
        });
    }
}

/// <summary>
/// Document template for teacher reports.
/// </summary>
public class TeacherReportDocument : ReportDocumentBase
{
    private readonly TeacherDailyReportModel _teacherModel;

    public TeacherReportDocument(TeacherDailyReportModel model, IPdfOptions options)
        : base(model, options)
    {
        _teacherModel = model;
    }

    protected override void ComposeContent(IContainer container)
    {
        container.Column(col =>
        {
            col.Item().PaddingBottom(10).Row(row =>
            {
                row.RelativeItem().Column(info =>
                {
                    info.Item().Text($"Teacher: {_teacherModel.TeacherName}").FontSize(12).Bold();
                    info.Item().Text($"Report Period: {_teacherModel.PeriodType}").FontSize(10);
                    info.Item().Text($"Date: {_teacherModel.ReportDate:yyyy-MM-dd}").FontSize(10);
                });
            });

            col.Item().PaddingBottom(10).Border(1).BorderColor(Colors.Grey.Lighten1).Padding(10).Column(stats =>
            {
                stats.Item().Text("Summary").FontSize(12).Bold().FontColor(Colors.Blue.Medium);
                stats.Item().PaddingTop(5).Row(summary =>
                {
                    summary.RelativeItem().Column(points =>
                    {
                        points.Item().Text("Points Given").FontSize(9).FontColor(Colors.Grey.Medium);
                        points.Item().Text(_teacherModel.TotalPointsGiven.ToString()).FontSize(14).Bold();
                    });
                    summary.RelativeItem().Column(students =>
                    {
                        students.Item().Text("Students").FontSize(9).FontColor(Colors.Grey.Medium);
                        students.Item().Text(_teacherModel.StudentsCount.ToString()).FontSize(14).Bold();
                    });
                    summary.RelativeItem().Column(attendance =>
                    {
                        attendance.Item().Text("Attendance").FontSize(9).FontColor(Colors.Grey.Medium);
                        attendance.Item().Text($"{_teacherModel.AttendancePercentage:F1}%").FontSize(14).Bold();
                    });
                });
            });
        });
    }
}

/// <summary>
/// Document template for class reports.
/// </summary>
public class ClassReportDocument : ReportDocumentBase
{
    private readonly ClassDailyReportModel _classModel;

    public ClassReportDocument(ClassDailyReportModel model, IPdfOptions options)
        : base(model, options)
    {
        _classModel = model;
    }

    protected override void ComposeContent(IContainer container)
    {
        container.Column(col =>
        {
            col.Item().PaddingBottom(10).Row(row =>
            {
                row.RelativeItem().Column(info =>
                {
                    info.Item().Text($"Class: {_classModel.ClassName}").FontSize(12).Bold();
                    info.Item().Text($"Report Period: {_classModel.PeriodType}").FontSize(10);
                    info.Item().Text($"Date: {_classModel.ReportDate:yyyy-MM-dd}").FontSize(10);
                });
            });

            col.Item().PaddingBottom(10).Border(1).BorderColor(Colors.Grey.Lighten1).Padding(10).Column(stats =>
            {
                stats.Item().Text("Summary").FontSize(12).Bold().FontColor(Colors.Blue.Medium);
                stats.Item().PaddingTop(5).Row(summary =>
                {
                    summary.RelativeItem().Column(students =>
                    {
                        students.Item().Text("Total Students").FontSize(9).FontColor(Colors.Grey.Medium);
                        students.Item().Text(_classModel.TotalStudents.ToString()).FontSize(14).Bold();
                    });
                    summary.RelativeItem().Column(quran =>
                    {
                        quran.Item().Text("Quran Pages").FontSize(9).FontColor(Colors.Grey.Medium);
                        quran.Item().Text(_classModel.TotalQuranPagesMemorized.ToString()).FontSize(14).Bold();
                    });
                    summary.RelativeItem().Column(hadith =>
                    {
                        hadith.Item().Text("Hadiths").FontSize(9).FontColor(Colors.Grey.Medium);
                        hadith.Item().Text(_classModel.TotalHadithsMemorized.ToString()).FontSize(14).Bold();
                    });
                    summary.RelativeItem().Column(points =>
                    {
                        points.Item().Text("Total Points").FontSize(9).FontColor(Colors.Grey.Medium);
                        points.Item().Text(_classModel.TotalPoints.ToString()).FontSize(14).Bold();
                    });
                });
            });
        });
    }
}

/// <summary>
/// Document template for attendance reports.
/// </summary>
public class AttendanceReportDocument : ReportDocumentBase
{
    private readonly AttendanceReportModel _attendanceModel;

    public AttendanceReportDocument(AttendanceReportModel model, IPdfOptions options)
        : base(model, options)
    {
        _attendanceModel = model;
    }

    protected override void ComposeContent(IContainer container)
    {
        container.Column(col =>
        {
            col.Item().PaddingBottom(10).Row(row =>
            {
                row.RelativeItem().Column(info =>
                {
                    info.Item().Text("Attendance Report").FontSize(12).Bold();
                    info.Item().Text($"Period: {_attendanceModel.FromDate:yyyy-MM-dd} to {_attendanceModel.ToDate:yyyy-MM-dd}").FontSize(10);
                    info.Item().Text($"Total Days: {_attendanceModel.TotalDays}").FontSize(10);
                });
            });

            col.Item().PaddingBottom(10).Border(1).BorderColor(Colors.Grey.Lighten1).Padding(10).Column(stats =>
            {
                stats.Item().Text("Summary").FontSize(12).Bold().FontColor(Colors.Blue.Medium);
                stats.Item().PaddingTop(5).Row(summary =>
                {
                    summary.RelativeItem().Column(student =>
                    {
                        student.Item().Text("Student Attendance").FontSize(9).FontColor(Colors.Grey.Medium);
                        student.Item().Text($"{_attendanceModel.StudentAverageAttendance:F1}%").FontSize(14).Bold();
                    });
                    summary.RelativeItem().Column(teacher =>
                    {
                        teacher.Item().Text("Teacher Attendance").FontSize(9).FontColor(Colors.Grey.Medium);
                        teacher.Item().Text($"{_attendanceModel.TeacherAverageAttendance:F1}%").FontSize(14).Bold();
                    });
                    summary.RelativeItem().Column(studentAbsences =>
                    {
                        studentAbsences.Item().Text("Student Absences").FontSize(9).FontColor(Colors.Grey.Medium);
                        studentAbsences.Item().Text(_attendanceModel.TotalStudentAbsences.ToString()).FontSize(14).Bold();
                    });
                    summary.RelativeItem().Column(teacherAbsences =>
                    {
                        teacherAbsences.Item().Text("Teacher Absences").FontSize(9).FontColor(Colors.Grey.Medium);
                        teacherAbsences.Item().Text(_attendanceModel.TotalTeacherAbsences.ToString()).FontSize(14).Bold();
                    });
                });
            });
        });
    }
}

/// <summary>
/// Document template for points reports.
/// </summary>
public class PointsReportDocument : ReportDocumentBase
{
    private readonly PointsReportModel _pointsModel;

    public PointsReportDocument(PointsReportModel model, IPdfOptions options)
        : base(model, options)
    {
        _pointsModel = model;
    }

    protected override void ComposeContent(IContainer container)
    {
        container.Column(col =>
        {
            col.Item().PaddingBottom(10).Row(row =>
            {
                row.RelativeItem().Column(info =>
                {
                    info.Item().Text("Points Report").FontSize(12).Bold();
                    if (!string.IsNullOrEmpty(_pointsModel.SemesterName))
                        info.Item().Text($"Semester: {_pointsModel.SemesterName}").FontSize(10);
                    if (_pointsModel.FromDate.HasValue && _pointsModel.ToDate.HasValue)
                        info.Item().Text($"Period: {_pointsModel.FromDate:yyyy-MM-dd} to {_pointsModel.ToDate:yyyy-MM-dd}").FontSize(10);
                });
            });

            col.Item().PaddingBottom(10).Border(1).BorderColor(Colors.Grey.Lighten1).Padding(10).Column(stats =>
            {
                stats.Item().Text("Points Summary").FontSize(12).Bold().FontColor(Colors.Blue.Medium);
                stats.Item().PaddingTop(5).Grid(grid =>
                {
                    grid.Columns(2);
                    
                    grid.Item().Column(total =>
                    {
                        total.Item().Text("Total Points").FontSize(9).FontColor(Colors.Grey.Medium);
                        total.Item().Text(_pointsModel.TotalPoints.ToString()).FontSize(14).Bold();
                    });
                    
                    grid.Item().Column(events =>
                    {
                        events.Item().Text("Total Events").FontSize(9).FontColor(Colors.Grey.Medium);
                        events.Item().Text(_pointsModel.TotalEvents.ToString()).FontSize(14).Bold();
                    });
                    
                    grid.Item().Column(quran =>
                    {
                        quran.Item().Text("Quran Points").FontSize(9).FontColor(Colors.Green.Medium);
                        quran.Item().Text(_pointsModel.QuranPoints.ToString()).FontSize(12).Bold();
                    });
                    
                    grid.Item().Column(hadith =>
                    {
                        hadith.Item().Text("Hadith Points").FontSize(9).FontColor(Colors.Blue.Medium);
                        hadith.Item().Text(_pointsModel.HadithPoints.ToString()).FontSize(12).Bold();
                    });
                    
                    grid.Item().Column(attendance =>
                    {
                        attendance.Item().Text("Attendance Points").FontSize(9).FontColor(Colors.Orange.Medium);
                        attendance.Item().Text(_pointsModel.AttendancePoints.ToString()).FontSize(12).Bold();
                    });
                    
                    grid.Item().Column(behavior =>
                    {
                        behavior.Item().Text("Behavior Points").FontSize(9).FontColor(Colors.Purple.Medium);
                        behavior.Item().Text(_pointsModel.BehaviorPoints.ToString()).FontSize(12).Bold();
                    });
                });
            });
        });
    }
}

/// <summary>
/// Document template for semester overview reports.
/// </summary>
public class SemesterOverviewReportDocument : ReportDocumentBase
{
    private readonly SemesterOverviewReportModel _semesterModel;

    public SemesterOverviewReportDocument(SemesterOverviewReportModel model, IPdfOptions options)
        : base(model, options)
    {
        _semesterModel = model;
    }

    protected override void ComposeContent(IContainer container)
    {
        container.Column(col =>
        {
            col.Item().PaddingBottom(10).Row(row =>
            {
                row.RelativeItem().Column(info =>
                {
                    info.Item().Text($"Semester: {_semesterModel.SemesterName}").FontSize(12).Bold();
                    info.Item().Text($"Period: {_semesterModel.StartDate:yyyy-MM-dd} to {_semesterModel.EndDate:yyyy-MM-dd}").FontSize(10);
                });
            });

            col.Item().PaddingBottom(10).Border(1).BorderColor(Colors.Grey.Lighten1).Padding(10).Column(stats =>
            {
                stats.Item().Text("Semester Overview").FontSize(12).Bold().FontColor(Colors.Blue.Medium);
                stats.Item().PaddingTop(5).Grid(grid =>
                {
                    grid.Columns(3);
                    
                    grid.Item().Column(students =>
                    {
                        students.Item().Text("Total Students").FontSize(9).FontColor(Colors.Grey.Medium);
                        students.Item().Text(_semesterModel.TotalStudents.ToString()).FontSize(14).Bold();
                    });
                    
                    grid.Item().Column(teachers =>
                    {
                        teachers.Item().Text("Total Teachers").FontSize(9).FontColor(Colors.Grey.Medium);
                        teachers.Item().Text(_semesterModel.TotalTeachers.ToString()).FontSize(14).Bold();
                    });
                    
                    grid.Item().Column(classes =>
                    {
                        classes.Item().Text("Total Classes").FontSize(9).FontColor(Colors.Grey.Medium);
                        classes.Item().Text(_semesterModel.TotalClasses.ToString()).FontSize(14).Bold();
                    });
                    
                    grid.Item().Column(quran =>
                    {
                        quran.Item().Text("Quran Pages").FontSize(9).FontColor(Colors.Green.Medium);
                        quran.Item().Text(_semesterModel.TotalQuranPagesMemorized.ToString()).FontSize(12).Bold();
                    });
                    
                    grid.Item().Column(hadith =>
                    {
                        hadith.Item().Text("Hadiths").FontSize(9).FontColor(Colors.Blue.Medium);
                        hadith.Item().Text(_semesterModel.TotalHadithsMemorized.ToString()).FontSize(12).Bold();
                    });
                    
                    grid.Item().Column(points =>
                    {
                        points.Item().Text("Total Points").FontSize(9).FontColor(Colors.Orange.Medium);
                        points.Item().Text(_semesterModel.TotalPointsGiven.ToString()).FontSize(12).Bold();
                    });
                });
                
                stats.Item().PaddingTop(10).Row(attendance =>
                {
                    attendance.RelativeItem().AlignCenter().Column(col =>
                    {
                        col.Item().Text("Average Attendance").FontSize(10).FontColor(Colors.Grey.Medium);
                        col.Item().Text($"{_semesterModel.AverageAttendancePercentage:F1}%").FontSize(18).Bold().FontColor(Colors.Green.Medium);
                    });
                });
            });
        });
    }
}

/// <summary>
/// Document template for generic reports.
/// </summary>
public class GenericReportDocument : ReportDocumentBase
{
    private readonly string _reportType;

    public GenericReportDocument(object model, string reportType, IPdfOptions options)
        : base(model as ReportBaseModel ?? new ReportBaseModel(), options)
    {
        _reportType = reportType;
    }

    protected override void ComposeContent(IContainer container)
    {
        container.Column(col =>
        {
            col.Item().PaddingBottom(10).Text($"Report Type: {_reportType}").FontSize(12).Bold();
            col.Item().Text("Generic report template - customize for specific report type.").FontSize(10);
        });
    }
}
