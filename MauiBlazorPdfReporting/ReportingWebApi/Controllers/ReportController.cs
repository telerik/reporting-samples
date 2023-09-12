using Microsoft.AspNetCore.Mvc;
using RenderReports;

namespace ReportingWebApi.Controllers
{
    public class ReportController : Controller
    {
        public ReportRenderer ReportRenderer { get; set; }
        public ReportController()
        {
            this.ReportRenderer = new ReportRenderer();
        }

        [HttpGet]
        [Route("{reportName}")]
        public IActionResult Index([FromRoute] string reportName)
        {
            if (reportName == null || reportName == string.Empty)
            {
                reportName = "SampleReport.trdp";
            }

            var reportBytes = this.ReportRenderer.RenderPdfReport(reportName);
            var ms = new MemoryStream(reportBytes);

            // If we remove the next sleep, the initial report may not render for ANDROID and the PDF Viewer shows the load animation instead
            // possibly related with bug https://feedback.telerik.com/blazor/1595340-the-pdf-viewer-does-not-render-the-document-as-expected-when-calling-the-readallbytesasync-method-from-grid-s-async-onrowclick-event-handler
            // Fixed in the 'MauiReporting' project by wrapping the PdfViewer in 'if (FileData != null)'
            //Thread.Sleep(1000);

            return new FileStreamResult(ms, "application/pdf");
        }

        [HttpGet]
        [Route("GetAvailableReports")]
        public ActionResult<string> GetAvailableReports()
        {

            string reportNames = string.Join("---", this.ReportRenderer.AvailableReports);

            return new ActionResult<string>(reportNames);
        }

        [HttpGet]
        [Route("test")]
        public IActionResult Test()
        {
            const string PdfBase64 = "JVBERi0xLjEKMSAwIG9iajw8L1R5cGUvQ2F0YWxvZy9QYWdlcyAyIDAgUj4+ZW5kb2JqCjIgMCBvYmo8PC9UeXBlL1BhZ2VzL0tpZHNbMyAwIFJdL0NvdW50IDEvTWVkaWFCb3ggWy00MCAtNjQgMjYwIDgwXSA+PmVuZG9iagozIDAgb2JqPDwvVHlwZS9QYWdlL1BhcmVudCAyIDAgUi9SZXNvdXJjZXM8PC9Gb250PDwvRjE8PC9UeXBlL0ZvbnQvU3VidHlwZS9UeXBlMS9CYXNlRm9udC9BcmlhbD4+ID4+ID4+L0NvbnRlbnRzIDQgMCBSPj5lbmRvYmoKNCAwIG9iajw8L0xlbmd0aCA1OT4+CnN0cmVhbQpCVAovRjEgMTggVGYKMCAwIFRkCihUZWxlcmlrIFBkZlZpZXdlciBmb3IgQmxhem9yKSBUagpFVAplbmRzdHJlYW0KZW5kb2JqCnhyZWYKMCA1CjAwMDAwMDAwMDAgNjU1MzUgZgowMDAwMDAwMDIxIDAwMDAwIG4KMDAwMDAwMDA4NiAwMDAwMCBuCjAwMDAwMDAxOTUgMDAwMDAgbgowMDAwMDAwNDkwIDAwMDAwIG4KdHJhaWxlciA8PCAgL1Jvb3QgMSAwIFIgL1NpemUgNSA+PgpzdGFydHhyZWYKNjA5CiUlRU9G";

            var reportBytes = Convert.FromBase64String(PdfBase64);
            var ms = new MemoryStream(reportBytes);

            return new FileStreamResult(ms, "application/pdf");
        }
    }
}