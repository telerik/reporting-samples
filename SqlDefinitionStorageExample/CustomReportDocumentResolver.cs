using Microsoft.EntityFrameworkCore;
using SqlDefinitionStorageExample.EFCore;
using System;
using System.IO;
using System.Linq;
using Telerik.Reporting;
using Telerik.Reporting.Services;

namespace SqlDefinitionStorageExample
{
    public class CustomReportDocumentResolver : IReportDocumentResolver
    {
        public IReportDocument Resolve(ReportSource reportSource)
        {
            // The main report is wrapped in an InstanceReportSource by CustomReportSourceResolver
            if (reportSource is InstanceReportSource)
            {
                return (reportSource as InstanceReportSource).ReportDocument;
            }
            // the subreport is resolved in the context of the main report SubReport
            else if (reportSource is UriReportSource)
            {
                var reportPackager = new ReportPackager();
                var uri = (reportSource as UriReportSource).Uri.Replace(AppDomain.CurrentDomain.BaseDirectory, string.Empty);
                var optionsBuilder = new DbContextOptionsBuilder<SqlDefinitionStorageContext>();

                // It is necessary to initialize a new dbContent because this code will be executed in a new thread
                using SqlDefinitionStorageContext dbContext = new(optionsBuilder.Options);
                var report = dbContext.Reports.FirstOrDefault(r => r.Uri == uri) ?? throw new FileNotFoundException();
                MemoryStream stream = new(report.Bytes);
                return reportPackager.UnpackageDocument(stream);
            }
            return null;
        }
    }
}
