using MauiBlazorViewer.Models;
using System.Text;

namespace MauiBlazorViewer
{
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
