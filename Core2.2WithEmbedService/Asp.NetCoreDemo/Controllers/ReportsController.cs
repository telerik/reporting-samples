namespace CSharp.AspNetCoreDemo.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Telerik.Reporting.Services;
    using Telerik.Reporting.Services.AspNetCore;
    using System.Net;
    using System.Net.Mail;
    using Telerik.Reporting.Cache.File;

    [Route("api/reports")]
    public class ReportsController : ReportsControllerBase
    {
        readonly string reportsPath = string.Empty;

        public ReportsController(ConfigurationService configSvc)
        {
            this.reportsPath = Path.Combine(configSvc.Environment.WebRootPath, "Reports");

            //this.reportsPath = Path.Combine(configSvc.Environment.WebRootPath, "..\\..\\..\\ReportDesigner");

            this.ReportServiceConfiguration = new ReportServiceConfiguration
            {
                ReportingEngineConfiguration = configSvc.Configuration,
                HostAppId = "Html5DemoAppCore",
                Storage = new FileStorage(),
                ReportResolver = new ReportTypeResolver()
                                    .AddFallbackResolver(new ReportFileResolver(this.reportsPath)),
            };
        }

        [HttpGet("reportlist")]
        public IEnumerable<string> GetReports()
        {
            return Directory
                .GetFiles(this.reportsPath)
                .Select(path =>
                    Path.GetFileName(path));
        }
        protected override HttpStatusCode SendMailMessage(MailMessage mailMessage)
        {
            throw new System.NotImplementedException("This method should be implemented in order to send mail messages");
            //using (var smtpClient = new SmtpClient("smtp01.mycompany.com", 25))
            //{
            //    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            //    smtpClient.EnableSsl = false;

            //    smtpClient.Send(mailMessage);
            //}
            //return HttpStatusCode.OK;
        }
    }
}
