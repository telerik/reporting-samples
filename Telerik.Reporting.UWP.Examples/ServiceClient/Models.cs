using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClient
{
    public class ClientIDModel
    {
        [JsonProperty("clientId")]
        public string ClientId { get; set; }
    }

    public class ReportSourceModel
    {
        [JsonProperty("Report")]
        public string Report { get; set; }
        [JsonProperty("ParameterValues")]
        public IDictionary<string, object> ParameterValues { get; set; }
    }

    public class InstanceIdModel
    {
        [JsonProperty("instanceId")]
        public string InstanceId { get; set; }
    }

    public class DocumentIdModel
    {
        [JsonProperty("documentId")]
        public string DocumentId { get; set; }
    }

    public class DocumentInfoModel
    {
        [JsonProperty("documentReady")]
        public bool DocumentReady { get; set; }

        [JsonProperty("PageCount")]
        public int PageCount { get; set; }

        [JsonProperty("DocumentMapAvailable")]
        public bool DocumentMapAvailable { get; set; }
    }

    public class ErrorModel
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("error_description")]
        public string Description { get; set; }
    }
}
