using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telerik.Reporting.Processing;

namespace PrintReportDirectlyAtClientSide.Controllers
{
    public class HomeController : Controller
    {
        private IWebHostEnvironment _environment;

        public HomeController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public IActionResult GenerateReportPDF(string reportName)
        {
            ReportProcessor reportProcessor = new ReportProcessor();
            Telerik.Reporting.UriReportSource uriReportSource = new Telerik.Reporting.UriReportSource();
            uriReportSource.Uri = Path.Combine(_environment.ContentRootPath, "Reports", reportName);
            RenderingResult result = reportProcessor.RenderReport("PDF", uriReportSource, null);

            return File(result.DocumentBytes, result.MimeType);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}