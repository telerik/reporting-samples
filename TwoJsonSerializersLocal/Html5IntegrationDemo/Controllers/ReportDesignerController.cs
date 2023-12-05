namespace CSharp.Net8.Html5IntegrationDemo.Controllers {
	using Microsoft.AspNetCore.Mvc;
	using Telerik.Reporting.Services;
	using Telerik.WebReportDesigner.Services;
	using Telerik.WebReportDesigner.Services.Controllers;

	[Route("api/reportdesigner")]
    public class ReportDesignerController : ReportDesignerControllerSTJBase {
        public ReportDesignerController(IReportDesignerServiceConfiguration reportDesignerServiceConfiguration, IReportServiceConfiguration reportServiceConfiguration)
            : base(reportDesignerServiceConfiguration, reportServiceConfiguration)
        {
        }
    }
}
