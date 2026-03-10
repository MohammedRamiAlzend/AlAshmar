using PdfGenerator.Interfaces;

namespace PdfGenerator.Options;

/// <summary>
/// Default implementation of PDF options.
/// </summary>
public class PdfOptions : IPdfOptions
{
    public string PageSize { get; set; } = "A4";
    public string Orientation { get; set; } = "Portrait";
    public float MarginTop { get; set; } = 20;
    public float MarginBottom { get; set; } = 20;
    public float MarginLeft { get; set; } = 20;
    public float MarginRight { get; set; } = 20;
    public bool IncludeHeader { get; set; } = true;
    public bool IncludeFooter { get; set; } = true;
    public string? LogoPath { get; set; }
    public string? CompanyName { get; set; }
    
    /// <summary>
    /// Creates a copy of the current options.
    /// </summary>
    public PdfOptions Clone()
    {
        return new PdfOptions
        {
            PageSize = this.PageSize,
            Orientation = this.Orientation,
            MarginTop = this.MarginTop,
            MarginBottom = this.MarginBottom,
            MarginLeft = this.MarginLeft,
            MarginRight = this.MarginRight,
            IncludeHeader = this.IncludeHeader,
            IncludeFooter = this.IncludeFooter,
            LogoPath = this.LogoPath,
            CompanyName = this.CompanyName
        };
    }
}
