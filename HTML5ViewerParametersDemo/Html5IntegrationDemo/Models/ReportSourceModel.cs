using System.Collections.Generic;

namespace HTML5ViewerParametersDemo.Models
{
    public class ReportSourceModel
    {
        public string Report { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
    }
}
