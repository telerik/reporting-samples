namespace CSharp.Net8.Html5IntegrationDemo.Controllers
{
    using System.Net;
    using System.Net.Mail;
    using Microsoft.AspNetCore.Mvc;
    using Telerik.Reporting.Services;
    using Telerik.Reporting.Services.AspNetCore;

    [Route("api/reports")]
    public class ReportsController : ReportsControllerSTJBase {
        public ReportsController(IReportServiceConfiguration reportServiceConfiguration)
            : base(reportServiceConfiguration)
        {
        }

        protected override HttpStatusCode SendMailMessage(MailMessage mailMessage)
        {
            throw new System.NotImplementedException("This method should be implemented in order to send mail messages");

            // using (var smtpClient = new SmtpClient("smtp01.mycompany.com", 25))
            // {
            //    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            //    smtpClient.EnableSsl = false;

            // smtpClient.Send(mailMessage);
            // }
            // return HttpStatusCode.OK;
        }
    }
}
