namespace PdfGenerator.Interfaces;

/// <summary>
/// Interface for PDF document generation services.
/// </summary>
public interface IPdfGeneratorService
{
    /// <summary>
    /// Generates a PDF document from HTML content.
    /// </summary>
    /// <param name="htmlContent">The HTML content to convert to PDF</param>
    /// <param name="fileName">The output file name</param>
    /// <returns>Byte array of the generated PDF</returns>
    Task<byte[]> GenerateFromHtmlAsync(string htmlContent, string fileName);
    
    /// <summary>
    /// Generates a PDF document from a report model.
    /// </summary>
    /// <typeparam name="T">The report model type</typeparam>
    /// <param name="model">The report model data</param>
    /// <param name="reportType">The type of report</param>
    /// <param name="fileName">The output file name</param>
    /// <returns>Byte array of the generated PDF</returns>
    Task<byte[]> GenerateFromModelAsync<T>(T model, string reportType, string fileName);
}

/// <summary>
/// Interface for PDF document options.
/// </summary>
public interface IPdfOptions
{
    /// <summary>
    /// Page size (e.g., A4, Letter)
    /// </summary>
    string PageSize { get; set; }
    
    /// <summary>
    /// Page orientation (Portrait or Landscape)
    /// </summary>
    string Orientation { get; set; }
    
    /// <summary>
    /// Top margin in millimeters
    /// </summary>
    float MarginTop { get; set; }
    
    /// <summary>
    /// Bottom margin in millimeters
    /// </summary>
    float MarginBottom { get; set; }
    
    /// <summary>
    /// Left margin in millimeters
    /// </summary>
    float MarginLeft { get; set; }
    
    /// <summary>
    /// Right margin in millimeters
    /// </summary>
    float MarginRight { get; set; }
    
    /// <summary>
    /// Include header in the PDF
    /// </summary>
    bool IncludeHeader { get; set; }
    
    /// <summary>
    /// Include footer in the PDF
    /// </summary>
    bool IncludeFooter { get; set; }
    
    /// <summary>
    /// Company/organization logo path
    /// </summary>
    string? LogoPath { get; set; }
    
    /// <summary>
    /// Company/organization name
    /// </summary>
    string? CompanyName { get; set; }
}
