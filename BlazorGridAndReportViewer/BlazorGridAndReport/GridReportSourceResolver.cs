using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telerik.Reporting;
using Telerik.Reporting.Services;
using System.Text.Json;
using System.Web;
using BlazorGridAndReport.Models.json;
using System.Text;

namespace BlazorGridAndReport
{
    public class GridReportSourceResolver : IReportSourceResolver
    {
        const string DATAGRID_EXAMPLE_REPORT = "DataGridExampleReport.trdx";
        const string SALES_EXAMPLE_REPORT = "productSalesExampleReport.trdx";
        const string PRESENTATION_EXAMPLE_REPORT = "PresentationExampleReport.trdx";
        Report gridReportInstance;
        string reportPath;

        public GridReportSourceResolver(string reportsPath)
        {
            this.reportPath = reportsPath;
        }

        public ReportSource Resolve(string report, OperationOrigin operationOrigin, IDictionary<string, object> currentParameterValues)
        {
            // Converts the JSON data into a dynamic object and reads the reportName field
            ReportSourceModel reportSourceData = Newtonsoft.Json.JsonConvert.DeserializeObject<ReportSourceModel>(report);
            var reportName = reportSourceData.Name;
            try
            {
                // If the passed report name is not DataGridExample.trdx, return null so the fallback file resolver will try to resolve the report.
                if (!string.Equals(reportName, DATAGRID_EXAMPLE_REPORT, System.StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(reportName, SALES_EXAMPLE_REPORT, System.StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(reportName, PRESENTATION_EXAMPLE_REPORT, System.StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }

                // Reads and deserialize the report file
                var gridReportPath = Path.Combine(this.reportPath, reportName);
                this.gridReportInstance = (Report)new Telerik.Reporting.XmlSerialization.ReportXmlSerializer().Deserialize(gridReportPath);

                // Set the JSON datasource
                if (string.Equals(reportName, DATAGRID_EXAMPLE_REPORT, System.StringComparison.OrdinalIgnoreCase))
                {
                    // This is only done for the DATAGRID_EXAMPLE_REPORT
                    // The two other reports have the JSON data embedded
                    this.BindData(Newtonsoft.Json.JsonConvert.SerializeObject(reportSourceData.Data));
                }

                // Set the filters
                if (reportSourceData.Filters != null)
                {
                    this.BindFilters(reportSourceData.Filters);
                }

                // Set the sorting
                if (reportSourceData.Sortings != null)
                {
                    this.BindSorts(reportSourceData.Sortings);
                }

                // The grouping is applied by updating the parameter. By default, the parameter is "1".
                // The groupheader in the DATAGRID report will be hidden if the parameter is still "1".
                this.BindGroupByParameter(reportSourceData.Grouping);

                return new InstanceReportSource()
                {
                    ReportDocument = this.gridReportInstance,
                };
            }
            catch (Newtonsoft.Json.JsonException jex)
            {
                Trace.TraceError("A problem occurred while deserializing the report source identifier: {0}.{1}Falling back to the next report resolver.", jex, Environment.NewLine);
                return null;
            }
        }

        private void BindData(string productJson)
        {
            // Get the JSON data-source that is already existing in the report and replacing the inline contents of it
            var report = this.gridReportInstance.Report;
            var reportDataSource = (JsonDataSource)report.DataSource;
            reportDataSource.Source = productJson;
        }

        private void BindFilters(List<ReportSourceFilter> filters)
        {
            // Clears any previous filters and adds them using the ToReportingFilter method
            var reportFilters = this.gridReportInstance.Filters;
            reportFilters.Clear();
            foreach (ReportSourceFilter filter in filters)
            {
                reportFilters.Add(this.ToReportingFilter(filter));
            }
        }

        private void BindSorts(ReportSourceSorting sort)
        {
            // Clears previous sortings and adds them
            var reportSorts = this.gridReportInstance.Sortings;
            reportSorts.Clear();
            var dir = "Ascending".Equals(sort.Direction) ? SortDirection.Asc : SortDirection.Desc;
            var field = "= Fields." + sort.Member;
            reportSorts.Add(field, dir);
        }

        private void BindGroupByParameter(string groupParameter)
        {
            if (groupParameter != null)
            {
                // The first parameter is used to tell the report what field the group should be based on
                var param = this.gridReportInstance.ReportParameters[0];
                param.Value = groupParameter;
                // A second parameter is passed where a space is added before each captial letter
                // This is to provide a value to display in the report for what field the grouping is set to
                var param2 = this.gridReportInstance.ReportParameters[1];
                param2.Value = Regex.Replace(groupParameter, "(\\B[A-Z])", " $1");
            }
            else
            {
                // In case no grouping is set, a static value 1 is returned so all data-rows end up in the same group
                this.gridReportInstance.ReportParameters[0].Value = "1";
            }
        }

        // Because the filters work differently in Blazor UI and Reporting, some translation is needed, which is done in this method
        private Filter ToReportingFilter(ReportSourceFilter filter)
        {
            switch (filter.OperatorString)
            {
                case "IsLessThan":
                    return new Filter("= Fields." + filter.Member, FilterOperator.LessThan, Convert.ToString(filter.Value));
                case "IsLessThanOrEqualTo":
                    return new Filter("= Fields." + filter.Member, FilterOperator.LessOrEqual, Convert.ToString(filter.Value));
                case "IsGreaterThan":
                    return new Filter("= Fields." + filter.Member, FilterOperator.GreaterThan, Convert.ToString(filter.Value));
                case "IsGreaterThanOrEqualTo":
                    return new Filter("= Fields." + filter.Member, FilterOperator.GreaterOrEqual, Convert.ToString(filter.Value));
                case "IsEqualTo":
                    return new Filter("= Fields." + filter.Member, FilterOperator.Equal, Convert.ToString(filter.Value));
                case "IsNotEqualTo":
                    return new Filter("= Fields." + filter.Member, FilterOperator.NotEqual, Convert.ToString(filter.Value));
                case "StartsWith":
                    return new Filter("= ToLower(Fields." + filter.Member + ")", FilterOperator.Like, Convert.ToString(filter.Value).ToLower() + "%");
                case "EndsWith":
                    return new Filter("= ToLower(Fields." + filter.Member + ")", FilterOperator.Like, "%" + Convert.ToString(filter.Value).ToLower());
                case "Contains":
                    return new Filter("= ToLower(Fields." + filter.Member + ")", FilterOperator.Like, "%" + Convert.ToString(filter.Value).ToLower() + "%");
                case "IsContainedIn":
                    return new Filter("= Fields." + filter.Member, FilterOperator.In, Convert.ToString(filter.Value));
                case "DoesNotContain":
                    return new Filter("= ToLower(Fields." + filter.Member + ")", FilterOperator.NotLike, "%" + Convert.ToString(filter.Value).ToLower() + "%");
                case "IsNull":
                    return new Filter("= Fields." + filter.Member, FilterOperator.Equal, "Null");
                case "IsNotNull":
                    return new Filter("= Fields." + filter.Member, FilterOperator.NotEqual, "Null");
                case "IsEmpty":
                    return new Filter("= Fields." + filter.Member, FilterOperator.Like, string.Empty);
                case "IsNotEmpty":
                    return new Filter("= Fields." + filter.Member, FilterOperator.NotLike, string.Empty);
                case "IsNullOrEmpty":
                    return new Filter("= (Fields." + filter.Member + " Is Null) ? True : (Fields." + filter.Member + " = \"\" ? True : False)", FilterOperator.Equal, "True");
                case "IsNotNullOrEmpty":
                    return new Filter("= (Fields." + filter.Member + " Is Null) ? True : (Fields." + filter.Member + " = \"\" ? True : False)", FilterOperator.Equal, "False");

                default:
                    throw new ArgumentException();
            }
        }
    }
}
