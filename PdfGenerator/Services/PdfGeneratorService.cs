using PdfGenerator.Interfaces;
using PdfGenerator.Models;
using PdfGenerator.Options;
using PdfGenerator.Templates;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PdfGenerator.Services;

/// <summary>
/// Implementation of PDF generator service using QuestPDF.
/// </summary>
public class PdfGeneratorService : IPdfGeneratorService
{
    private readonly IPdfOptions _options;

    public PdfGeneratorService(IPdfOptions? options = null)
    {
        _options = options ?? new PdfOptions();
        
        // Configure QuestPDF
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public Task<byte[]> GenerateFromHtmlAsync(string htmlContent, string fileName)
    {
        // For HTML to PDF, we'd need a different approach as QuestPDF doesn't support HTML directly
        // In production, consider using Playwright or similar
        throw new NotImplementedException("HTML to PDF conversion requires additional setup. Use GenerateFromModelAsync instead.");
    }

    public Task<byte[]> GenerateFromModelAsync<T>(T model, string reportType, string fileName)
    {
        var result = reportType.ToLower() switch
        {
            "student-daily" => GenerateStudentDailyReport(model as StudentDailyReportModel ?? throw new InvalidCastException()),
            "student-weekly" => GenerateStudentWeeklyReport(model as StudentDailyReportModel ?? throw new InvalidCastException()),
            "student-monthly" => GenerateStudentMonthlyReport(model as StudentDailyReportModel ?? throw new InvalidCastException()),
            "student-semester" => GenerateStudentSemesterReport(model as StudentDailyReportModel ?? throw new InvalidCastException()),
            "teacher-daily" => GenerateTeacherDailyReport(model as TeacherDailyReportModel ?? throw new InvalidCastException()),
            "class-daily" => GenerateClassDailyReport(model as ClassDailyReportModel ?? throw new InvalidCastException()),
            "attendance" => GenerateAttendanceReport(model as AttendanceReportModel ?? throw new InvalidCastException()),
            "points" => GeneratePointsReport(model as PointsReportModel ?? throw new InvalidCastException()),
            "semester-overview" => GenerateSemesterOverviewReport(model as SemesterOverviewReportModel ?? throw new InvalidCastException()),
            _ => GenerateGenericReport(model, reportType)
        };
        
        return Task.FromResult(result);
    }

    private byte[] GenerateStudentDailyReport(StudentDailyReportModel model)
    {
        var document = new StudentReportDocument(model, _options);
        return document.GeneratePdf();
    }

    private byte[] GenerateStudentWeeklyReport(StudentDailyReportModel model)
    {
        var document = new StudentReportDocument(model, _options);
        return document.GeneratePdf();
    }

    private byte[] GenerateStudentMonthlyReport(StudentDailyReportModel model)
    {
        var document = new StudentReportDocument(model, _options);
        return document.GeneratePdf();
    }

    private byte[] GenerateStudentSemesterReport(StudentDailyReportModel model)
    {
        var document = new StudentReportDocument(model, _options);
        return document.GeneratePdf();
    }

    private byte[] GenerateTeacherDailyReport(TeacherDailyReportModel model)
    {
        var document = new TeacherReportDocument(model, _options);
        return document.GeneratePdf();
    }

    private byte[] GenerateClassDailyReport(ClassDailyReportModel model)
    {
        var document = new ClassReportDocument(model, _options);
        return document.GeneratePdf();
    }

    private byte[] GenerateAttendanceReport(AttendanceReportModel model)
    {
        var document = new AttendanceReportDocument(model, _options);
        return document.GeneratePdf();
    }

    private byte[] GeneratePointsReport(PointsReportModel model)
    {
        var document = new PointsReportDocument(model, _options);
        return document.GeneratePdf();
    }

    private byte[] GenerateSemesterOverviewReport(SemesterOverviewReportModel model)
    {
        var document = new SemesterOverviewReportDocument(model, _options);
        return document.GeneratePdf();
    }

    private byte[] GenerateGenericReport<T>(T model, string reportType)
    {
        var document = new GenericReportDocument(model, reportType, _options);
        return document.GeneratePdf();
    }
}
