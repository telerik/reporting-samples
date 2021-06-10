using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Reporting.Services;
using Telerik.ReportServer.HttpClient;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Telerik.Reporting;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            DownloadAllServerReports();
        }

        private static void DownloadAllServerReports()
        {
            var settings = new Telerik.ReportServer.HttpClient.Settings()
            {
                BaseAddress = "http://ntodorova:83/"
            };

            using (var rsClient = new ReportServerClient(settings))
            {
                rsClient.Login("username", "password");
                var categories = rsClient.GetCategories();
                foreach (var category in categories)
                {
                    var reportInfos = rsClient.GetReportInfosInCategory(category.Id);
                    foreach (var reportInfo in reportInfos)
                    {
                        var reportId = reportInfo.Id;
                        var reportDefinition = rsClient.GetLatestReportRevision(reportId);
                        SaveReportDefintion(reportDefinition, reportInfo.Name);
                    }
                }
            }
        }

        private static void SaveReportDefintion(Telerik.ReportServer.Services.Models.ReportRevisionContent reportDefinition, string name)
        {
            var extension =reportDefinition;
            string path = "filepath" + name;
            File.WriteAllBytes(path + reportDefinition.Extension, reportDefinition.Content);
        }
    }
}
