using SqlDefinitionStorageExample.EFCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Telerik.Reporting;
using Telerik.Reporting.Services;

namespace SqlDefinitionStorageExample
{
    public class CustomReportSourceResolver : IReportSourceResolver
    {

        private SqlDefinitionStorageContext _dbContext { get; }
        public CustomReportSourceResolver(SqlDefinitionStorageContext context) {
            this._dbContext = context;
        }

        public ReportSource Resolve(string uri, OperationOrigin operationOrigin, IDictionary<string, object> currentParameterValues)
        {
            var reportPackager = new ReportPackager();
            var report = this._dbContext.Reports.FirstOrDefault(r => r.Uri == uri.Replace("/", "\\"));

            if (report == null)
            {
                throw new FileNotFoundException();
            }

            MemoryStream stream = new(report.Bytes);
            Telerik.Reporting.Report report1 = (Telerik.Reporting.Report)reportPackager.UnpackageDocument(stream);
            
            var instanceReportSource = new InstanceReportSource
            {
                ReportDocument = report1
            };

            return instanceReportSource;
        }
    }
}
