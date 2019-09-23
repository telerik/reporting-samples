using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassParameter.Models
{
    public class ReportModel
    {
        public string ReportName { get; set; }
        public Dictionary<string,object> Parameters { get; set; }

    }
}