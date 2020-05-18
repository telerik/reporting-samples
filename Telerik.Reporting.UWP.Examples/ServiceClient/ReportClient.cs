using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClient
{
    public class ReportClient : IDisposable
    {
        public string BaseAddress { get; set; }
        public HttpClient client;
        public string ClientId;

        public ReportClient(string uri)
        {
            this.client = new HttpClient();
            this.BaseAddress = uri;

            this.client.BaseAddress = new Uri(this.BaseAddress);
        }

        public void Dispose()
        {
            using (this.client) { }
        }

        public void RegisterClient()
        {
            var headers = new List<KeyValuePair<string, string>>();
            var content = new FormUrlEncodedContent(headers);

            var registerClientTask = this.client.PostAsync(this.BaseAddress + "/clients", content);
            var response = registerClientTask.Result;

            if (response.IsSuccessStatusCode)
            {
                var clientId = GetResut<ClientIDModel>(response);
                this.ClientId = clientId.ClientId;
            }
            else
            {
                var error = GetResut<ErrorModel>(response);
                throw new Exception(error.Description);
            }
        }

        public string CreateInstance(string reportSource)
        {
            HttpContent content = new StringContent(reportSource, Encoding.UTF8, "application/json");

            string route = $"{this.BaseAddress}/clients/{this.ClientId}/instances";
            var instanceTask = this.client.PostAsync(route, content);

            InstanceIdModel instanceId = null;
            var response = instanceTask.Result;
            if (response.IsSuccessStatusCode)
            {
                instanceId = GetResut<InstanceIdModel>(response);
            }
            else
            {
                var error = GetResut<ErrorModel>(response);
                throw new Exception(error.Description);
            }

            return instanceId.InstanceId;
        }

        public string CreateDocument(string instanceId, string format, string deviceInfo = null, string useCache = "true")
        {
            string contentBody = $"{{ \"useCache\": {useCache}, \"format\": \"{format}\" }}";
            HttpContent content = new StringContent(contentBody, Encoding.UTF8, "application/json");

            string route = $"{this.BaseAddress}/clients/{this.ClientId}/instances/{instanceId}/documents";
            var documentTask = this.client.PostAsync(route, content);

            DocumentIdModel documentId = null;
            var response = documentTask.Result;
            if (response.IsSuccessStatusCode)
            {
                documentId = GetResut<DocumentIdModel>(response);
            }
            else
            {
                var error = GetResut<ErrorModel>(response);
                throw new Exception(error.Description);
            }

            return documentId.DocumentId;
        }

        public bool DocumentIsProcessing(string instanceId, string documentId)
        {
            string route = $"{this.BaseAddress}/clients/{this.ClientId}/instances/{instanceId}/documents/{documentId}/Info";

            var documentInfoTask = this.client.GetAsync(route);

            DocumentInfoModel documentInfo = null;
            var response = documentInfoTask.Result;
            if (response.IsSuccessStatusCode)
            {
                documentInfo = GetResut<DocumentInfoModel>(response);
            }
            else
            {
                var error = GetResut<ErrorModel>(response);
                throw new Exception(error.Description);
            }

            return !documentInfo.DocumentReady;
        }

        public string GetDocumentUrl(string instanceId, string documentId)
        {
            return $"{this.BaseAddress}/clients/{this.ClientId}/instances/{instanceId}/documents/{documentId}";
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

        private static T GetResut<T>(HttpResponseMessage response)
        {
            var clientIdRaw = response.Content.ReadAsStringAsync().Result;
            var clientId = JsonConvert.DeserializeObject<T>(clientIdRaw);
            return clientId;
        }
    }
}
