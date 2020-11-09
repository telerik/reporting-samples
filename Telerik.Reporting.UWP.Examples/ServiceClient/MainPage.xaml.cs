using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ServiceClient
{
    public sealed partial class MainPage : Page
    {
        private const string serviceUrl = "http://localhost:57615/api/reports";
        private object btnOriginalContent;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            this.btnOriginalContent = this.BtnExport.Content;

            this.BtnExport.Content = "Exporting...";
            this.BtnExport.IsEnabled = false;

            var format = "pdf";
            var file = await Utilities.PickFileName(format);
            var documentUrl = await CreateDocument(format);
            try
            {
                Utilities.SaveFile(new Uri(documentUrl), file);
            }
            finally
            {
                this.BtnExport.Content = btnOriginalContent;
                this.BtnExport.IsEnabled = true;
            }
        }

        private static async Task<string> CreateDocument(string format)
        {
            return await Task.Factory.StartNew(() =>
            {
                var client = new ReportClient(serviceUrl);
                client.RegisterClient();
                ReportSourceModel reportSourceModel = new ReportSourceModel()
                {
                    Report = "Report Catalog.trdp",
                };

                var reportSource = JsonConvert.SerializeObject(reportSourceModel);
                var instanceId = client.CreateInstance(reportSource);
                var documentId = client.CreateDocument(instanceId, format);
                bool documentProcessing;
                do
                {
                    Thread.Sleep(500);// wait before next Info request
                    documentProcessing = client.DocumentIsProcessing(instanceId, documentId);
                } while (documentProcessing);

                return client.GetDocumentUrl(instanceId, documentId);
            });
        }
    }
}
