using System.Web.Mvc;
using Telerik.Reporting;
using TenantPortal.Models.Reports;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DisplayReportViewer(string selectedReport)
        {
            var ReportType = new UriReportSource();
            ReportType.Uri = selectedReport;

            ReportModel reportModel = new ReportModel()
            {
                SelectedReportType = ReportType
            };

            return View("ReportViewerView", reportModel);
        }
    }
}