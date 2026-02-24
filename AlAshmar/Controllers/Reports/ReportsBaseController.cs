using AlAshmar.Domain.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Xml.Serialization;

namespace AlAshmar.Controllers.Reports;

/// <summary>
/// Base controller for report endpoints with common filtering and pagination support.
/// </summary>
[ApiController]
[Route("api/reports")]
[Authorize]
public abstract class ReportsBaseController : ControllerBase
{
    /// <summary>
    /// Validates the date range for reports.
    /// </summary>
    protected bool ValidateDateRange(DateTime? fromDate, DateTime? toDate, out string? errorMessage)
    {
        errorMessage = null;

        if (fromDate.HasValue && toDate.HasValue)
        {
            if (fromDate.Value > toDate.Value)
            {
                errorMessage = "From date must be less than or equal to to date.";
                return false;
            }

            if (toDate.Value - fromDate.Value > TimeSpan.FromDays(366))
            {
                errorMessage = "Date range cannot exceed 366 days.";
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Gets the start of the week for a given date.
    /// </summary>
    protected DateTime GetWeekStart(DateTime date)
    {
        int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
        return date.AddDays(-1 * diff).Date;
    }

    /// <summary>
    /// Gets the end of the week for a given date.
    /// </summary>
    protected DateTime GetWeekEnd(DateTime date)
    {
        return GetWeekStart(date).AddDays(6);
    }

    /// <summary>
    /// Creates a paged response with pagination headers.
    /// </summary>
    protected ObjectResult CreatePagedResponse<T>(PagedList<T> pagedList)
    {
        Response.Headers.Append("X-Pagination", 
            $"Page: {pagedList.Page}, PageSize: {pagedList.PageSize}, TotalItems: {pagedList.TotalItems}, TotalPages: {pagedList.TotalPages}");
        return Ok(pagedList);
    }

    /// <summary>
    /// Returns data as JSON file download.
    /// </summary>
    protected FileContentResult DownloadAsJson<T>(T data, string fileName)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(data, new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
        });
        return File(Encoding.UTF8.GetBytes(json), "application/json", $"{fileName}.json");
    }

    /// <summary>
    /// Returns data as XML file download.
    /// </summary>
    protected FileContentResult DownloadAsXml<T>(T data, string fileName)
    {
        var serializer = new XmlSerializer(typeof(T));
        using var writer = new StringWriter();
        serializer.Serialize(writer, data);
        return File(Encoding.UTF8.GetBytes(writer.ToString()), "application/xml", $"{fileName}.xml");
    }

    /// <summary>
    /// Returns data as CSV file download.
    /// </summary>
    protected FileContentResult DownloadAsCsv<T>(IEnumerable<T> data, string fileName, Func<T, string[]> rowSelector)
    {
        var sb = new StringBuilder();
        
        // Get headers from first object's property names
        var properties = typeof(T).GetProperties();
        sb.AppendLine(string.Join(",", properties.Select(p => p.Name)));
        
        // Add data rows
        foreach (var item in data)
        {
            var values = properties.Select(p =>
            {
                var value = p.GetValue(item)?.ToString() ?? string.Empty;
                return $"\"{value.Replace("\"", "\"\"")}\"";
            });
            sb.AppendLine(string.Join(",", values));
        }
        
        return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", $"{fileName}.csv");
    }

    /// <summary>
    /// Common pagination parameters for report queries.
    /// </summary>
    protected class PaginationParams
    {
        [FromQuery] public int Page { get; set; } = 1;
        [FromQuery] public int PageSize { get; set; } = 10;
    }

    /// <summary>
    /// Common date range parameters for report queries.
    /// </summary>
    protected class DateRangeParams
    {
        [FromQuery] public DateTime? FromDate { get; set; }
        [FromQuery] public DateTime? ToDate { get; set; }
    }

    /// <summary>
    /// Period filter parameters for reports.
    /// </summary>
    protected class PeriodFilterParams
    {
        [FromQuery] public DateTime? FromDate { get; set; }
        [FromQuery] public DateTime? ToDate { get; set; }
        [FromQuery] public ReportPeriodType PeriodType { get; set; } = ReportPeriodType.All;
    }

    /// <summary>
    /// Export format enum.
    /// </summary>
    protected enum ExportFormat
    {
        Json,
        Xml,
        Csv,
        Pdf
    }
}
