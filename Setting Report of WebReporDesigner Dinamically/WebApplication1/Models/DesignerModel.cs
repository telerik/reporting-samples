using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class DesignerModel
    {
        private string reportName;
        public string ReportName { get; set; }

        public DesignerModel(string reportName)
        {
            this.ReportName = reportName;
        }
    }
}
