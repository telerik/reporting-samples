using Microsoft.AspNetCore.ResponseCompression;
using System.Collections;
using Telerik.Reporting.Processing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.MapGet("/report", (string reportName, string authToken) => {

    // Create the report processor
    ReportProcessor reportProcessor = new();

    // Report source with the report
    Telerik.Reporting.UriReportSource uriReportSource = new()
    {
        Uri = Path.Combine(app.Environment.ContentRootPath, "Reports", reportName)
    };

    // Passing an auth token to the WebServiceDataSource via a parameter
    uriReportSource.Parameters.Add("AuthParameter", $"Bearer {authToken}");

    // Render the report
    RenderingResult result = reportProcessor.RenderReport("PDF", uriReportSource, null);
    var PdfFile = Results.File(result.DocumentBytes, result.MimeType);

    // Check if the PDF save path exists
    var pdfPath = Path.Combine(app.Environment.ContentRootPath,"wwwroot", "pdf");
    if (!Directory.Exists(pdfPath))
    {
        Directory.CreateDirectory(pdfPath);
    }

    // Add the report name and change the extension to PDF
    pdfPath = Path.Combine(pdfPath, Path.GetFileNameWithoutExtension(reportName) + ".pdf");

    // If the file exists, try to delete it
    if (File.Exists(pdfPath))
    {
        File.Delete(pdfPath);
    }

    // Save the PDF to the PDF folder of wwwroot
    File.WriteAllBytes(pdfPath, result.DocumentBytes);

    // Return the PDF file
    return PdfFile;
});

app.Run();
