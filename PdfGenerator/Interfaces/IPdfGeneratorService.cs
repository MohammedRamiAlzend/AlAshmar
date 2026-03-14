namespace PdfGenerator.Interfaces;




public interface IPdfGeneratorService
{






    Task<byte[]> GenerateFromHtmlAsync(string htmlContent, string fileName);









    Task<byte[]> GenerateFromModelAsync<T>(T model, string reportType, string fileName);
}




public interface IPdfOptions
{



    string PageSize { get; set; }




    string Orientation { get; set; }




    float MarginTop { get; set; }




    float MarginBottom { get; set; }




    float MarginLeft { get; set; }




    float MarginRight { get; set; }




    bool IncludeHeader { get; set; }




    bool IncludeFooter { get; set; }




    string? LogoPath { get; set; }




    string? CompanyName { get; set; }
}
