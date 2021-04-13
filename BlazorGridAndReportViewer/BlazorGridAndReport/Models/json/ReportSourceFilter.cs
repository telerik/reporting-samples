using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorGridAndReport.Models.json
{
    public class ReportSourceFilter
    {
        public dynamic Value { get; set; }
        public string Member { get; set; }
        public string OperatorString { get; set; }
    }
}
