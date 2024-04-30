namespace RenderReportFromService
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    class Program
    {
        static async Task Main(string[] args)
        {
            await ExportReportFromServiceAsync("Barcodes Report.trdp", "PDF", "http://localhost:59656/api/reports");
        }

        static async Task ExportReportFromServiceAsync(string reportName, string format, string baseUrl)
        {
            // 1. Register client
            ReportClient reportClient = new ReportClient(baseUrl);
            await reportClient.RegisterClient();


            // 2. Create Report Instance
            ReportSourceModel reportSourceModel = new ReportSourceModel()
            {
                Report = reportName
            };

            string reportSource = System.Text.Json.JsonSerializer.Serialize(reportSourceModel);
            string reportInstanceId = await reportClient.CreateInstance(reportSource);


            // 3. Create Document
            string reportDocumentId = await reportClient.CreateDocument(reportInstanceId, format);

            bool documentProcessing;
            do
            {
                Thread.Sleep(500);// wait before next Info request
                documentProcessing = await reportClient.DocumentIsProcessing(reportInstanceId, reportDocumentId);
            } while (documentProcessing);

            // 4. Get Document
            byte[] result = await reportClient.GetDocument(reportInstanceId, reportDocumentId);
            File.WriteAllBytes($"C:\\temp\\{reportName}.{format.ToLower()}", result);
        }

    }


    public class ClientIDModel
    {
        [System.Text.Json.Serialization.JsonPropertyName("clientId")]
        public string ClientId { get; set; }
    }

    public class ReportSourceModel
    {
        [System.Text.Json.Serialization.JsonPropertyName("Report")]
        public string Report { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("ParameterValues")]
        public IDictionary<string, object> ParameterValues { get; set; }
    }

    public class InstanceIdModel
    {
        [System.Text.Json.Serialization.JsonPropertyName("instanceId")]
        public string InstanceId { get; set; }
    }

    public class DocumentIdModel
    {
        [System.Text.Json.Serialization.JsonPropertyName("documentId")]
        public string DocumentId { get; set; }
    }

    public class DocumentInfoModel
    {
        [System.Text.Json.Serialization.JsonPropertyName("documentReady")]
        public bool DocumentReady { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("PageCount")]
        public int PageCount { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("DocumentMapAvailable")]
        public bool DocumentMapAvailable { get; set; }
    }

    public class ErrorModel
    {
        [System.Text.Json.Serialization.JsonPropertyName("error")]
        public string Error { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("error_description")]
        public string Description { get; set; }
    }

    public static class HttpContentExtensions
    {
        public static async Task<T> ReadAsAsync<T>(this HttpContent content) =>
            await System.Text.Json.JsonSerializer.DeserializeAsync<T>(await content.ReadAsStreamAsync());
    }

    public class ReportClient : IDisposable
    {
        public string BaseAddress { get; set; }
        public HttpClient client;
        public string ClientId;

        public ReportClient(string uri)
        {
            this.client = HttpClientFactory.Create();
            this.BaseAddress = uri;

            this.client.BaseAddress = new Uri(this.BaseAddress);
        }

        public void Dispose()
        {
            using (this.client) { }
        }

        public async Task RegisterClient()
        {
            var headers = new List<KeyValuePair<string, string>>();
            var content = new FormUrlEncodedContent(headers);

            var response = await this.client.PostAsync(this.BaseAddress + "/clients", content);

            if (response.IsSuccessStatusCode)
            {
                var clientIdTask = await response.Content.ReadAsAsync<ClientIDModel>();
                this.ClientId = clientIdTask.ClientId;
            }
            else
            {
                var error = await response.Content.ReadAsAsync<ErrorModel>();
                throw new Exception(error.Description);
            }
        }

        public async Task<string> CreateInstance(string reportSource)
        {
            HttpContent content = new StringContent(reportSource, Encoding.UTF8, "application/json");

            string route = $"{this.BaseAddress}/clients/{this.ClientId}/instances";
            var response = await this.client.PostAsync(route, content);

            InstanceIdModel instanceId = null;
            if (response.IsSuccessStatusCode)
            {
                instanceId = await response.Content.ReadAsAsync<InstanceIdModel>();
            }
            else
            {
                var error = await response.Content.ReadAsAsync<ErrorModel>();
                throw new Exception(error.Description);
            }

            return instanceId.InstanceId;
        }

        public async Task<string> CreateDocument(string instanceId, string format, string deviceInfo = null, string useCache = "true")
        {
            string contentBody = $"{{ \"useCache\": {useCache}, \"format\": \"{format}\" }}";
            HttpContent content = new StringContent(contentBody, Encoding.UTF8, "application/json");

            string route = $"{this.BaseAddress}/clients/{this.ClientId}/instances/{instanceId}/documents";
            var response = await this.client.PostAsync(route, content);

            DocumentIdModel documentId = null;
            if (response.IsSuccessStatusCode)
            {
                documentId = await response.Content.ReadAsAsync<DocumentIdModel>();
            }
            else
            {
                var error = await response.Content.ReadAsAsync<ErrorModel>();
                throw new Exception(error.Description);
            }

            return documentId.DocumentId;
        }

        public async Task<bool> DocumentIsProcessing(string instanceId, string documentId)
        {
            string route = $"{this.BaseAddress}/clients/{this.ClientId}/instances/{instanceId}/documents/{documentId}/Info";

            var response = await this.client.GetAsync(route);

            DocumentInfoModel documentInfo = null;
            if (response.IsSuccessStatusCode)
            {
                documentInfo = await response.Content.ReadAsAsync<DocumentInfoModel>();
            }
            else
            {
                var error = await response.Content.ReadAsAsync<ErrorModel>();
                throw new Exception(error.Description);
            }

            return !documentInfo.DocumentReady;
        }

        public async Task<byte[]> GetDocument(string instanceId, string documentId)
        {
            string route = $"{this.BaseAddress}/clients/{this.ClientId}/instances/{instanceId}/documents/{documentId}";

            var response = await this.client.GetAsync(route);
            byte[] documentBytes;

            if (response.IsSuccessStatusCode)
            {
                documentBytes = await response.Content.ReadAsByteArrayAsync();
            }
            else
            {
                var error = await response.Content.ReadAsAsync<ErrorModel>();
                throw new Exception(error.Description);
            }

            return documentBytes;
        }

        private static void EnsureSuccessStatusCode(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw
                    new Exception(
                        response.ReasonPhrase +
                        Environment.NewLine +
                        response.RequestMessage.RequestUri);
            }
        }
    }
}
