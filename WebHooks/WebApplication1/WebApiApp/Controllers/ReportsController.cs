namespace WebApiApp.Controllers
{
    using System.IO;
    using System.Web;
    using Telerik.Reporting.Cache.File;
    using Telerik.Reporting.Services;
    using Telerik.Reporting.Services.WebApi;

    public class ReportsController : ReportsControllerBase
    {
        static ReportServiceConfiguration configurationInstance;

        static ReportsController()
        {
            var appPath = HttpContext.Current.Server.MapPath("~/");
            var reportsPath = Path.Combine(appPath, "Reports");

            var resolver = new UriReportSourceResolver(reportsPath)
                .AddFallbackResolver(new TypeReportSourceResolver());

            configurationInstance = new ReportServiceConfiguration
            {
                HostAppId = "Html5App",
                Storage = new FileStorage("c:\\temp"),
                ReportSourceResolver = resolver,
                // ReportSharingTimeout = 0,
                // ClientSessionTimeout = 15,
            };
        }

        public ReportsController()
        {
            this.ReportServiceConfiguration = configurationInstance;
        }
    }
}