using Microsoft.AspNetCore.Mvc;
using Telerik.Reporting.Services;
using Telerik.Reporting.Services.AspNetCore;

namespace StrictResourceResolver.ViewerService.Controllers;

[ApiController]
[Route("api/reports")]
public class ReportsController : ReportsControllerBase
{
    public ReportsController(IReportServiceConfiguration configuration)
        : base(configuration)
    {
    }
}
