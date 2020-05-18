using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.Foundation.Metadata;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.System;
using Common;

namespace RuntimeComponent
{
    [AllowForWeb]
    public sealed class ViewerEventHandler
    {
        readonly string serviceUrl;

        public ViewerEventHandler(string serviceUrl)
        {
            this.serviceUrl = serviceUrl;
        }

        public void Print(string documentUrl)
        {
            var options = new LauncherOptions();
            options.ContentType = "application/pdf";
            Launcher.LaunchUriAsync(this.CreateUri(documentUrl), options)
                .AsTask()
                .ContinueWith(r => {
                    Debug.WriteLine("Print ready. Success: " + !r.IsFaulted);
                });
        }

        public void Export(string documentUrl, string format)
        {
            Utilities.PickFileName(format).AsTask().ContinueWith(a =>
            {
                Utilities.SaveFile(CreateUri(documentUrl), a.Result);
            });
        }

        Uri CreateUri(string uri)
        {
            return new Uri(this.serviceUrl + uri);
        }
    }
}