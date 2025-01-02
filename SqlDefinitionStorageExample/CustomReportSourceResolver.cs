using SqlDefinitionStorageExample.EFCore;
using SqlDefinitionStorageExample.EFCore.Models;
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
            _dbContext = context;
        }

        public ReportSource Resolve(string uri, OperationOrigin operationOrigin, IDictionary<string, object> currentParameterValues)
        {
            var reportPackager = new ReportPackager();

            if (!uri.Contains("Reports\\"))
            {
                uri = $"Reports\\{uri}";
            }

            var report = _dbContext.Resources.FirstOrDefault(r => r.Uri == uri.Replace("/", "\\"));

            if (report == null)
            {
                throw new FileNotFoundException();
            }

            MemoryStream stream = new(report.Bytes);
            Telerik.Reporting.Report reportDocument = (Telerik.Reporting.Report)reportPackager.UnpackageDocument(stream);
            
            var instanceReportSource = new InstanceReportSource
            {
                ReportDocument = reportDocument
            };

            return instanceReportSource;
        }
    }
}
