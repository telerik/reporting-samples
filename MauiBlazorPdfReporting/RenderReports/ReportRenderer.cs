using Microsoft.Extensions.Configuration;
using Telerik.Reporting;
using Telerik.Reporting.Processing;

namespace RenderReports
{
    public class ReportRenderer
    {
        public string ReportsPath { get; set; }
        public string DefaultReport { get; set; }
        public string[] AvailableReports { get; set; }

        private IConfiguration configuration;

        public ReportRenderer()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            this.ReportsPath = Path.Combine(basePath, "Reports");
            this.configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", true)
                        .Build();

            var availableReports = Directory.GetFiles(this.ReportsPath);
            this.AvailableReports = availableReports.Select(r => Path.GetFileName(r)).ToArray();
            this.DefaultReport = this.AvailableReports[0];
        }

        public byte[] RenderPdfReport(string reportName = "")
        {
            string repName = ((reportName ?? string.Empty) == string.Empty) ? this.DefaultReport : reportName;

            var reportSource = new UriReportSource { Uri = Path.Combine(this.ReportsPath, repName) };

            var reportProcessor = new ReportProcessor(this.configuration);
            var deviceInfo = new System.Collections.Hashtable();
            RenderingResult result = reportProcessor.RenderReport("PDF", reportSource, deviceInfo);

            byte[]? fileData = null;
            if (!result.HasErrors)
            {
                fileData = result.DocumentBytes.ToArray();
            }

            if (null == fileData)
            {
                throw new NullReferenceException($"Cannot generate PDF document from the {repName}!");
            }

            return fileData;
        }
    }
}