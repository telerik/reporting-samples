using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Common
{
    public static class Utilities
    {
        public static IAsyncOperation<StorageFile> PickFileName(string format)
        {
            var formatLowerCase = format.ToLowerInvariant();

            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                SuggestedFileName = "report",
            };

            savePicker.FileTypeChoices.Add(
                formatLowerCase,
                new List<string>() { "." + formatLowerCase });

            return savePicker.PickSaveFileAsync();
        }

        public static void SaveFile(Uri documentUrl, StorageFile file)
        {
            if (file != null)
            {
                var downloader = new BackgroundDownloader();
                downloader.CreateDownload(documentUrl, file)
                    .StartAsync()
                    .AsTask()
                    .ContinueWith(r =>
                    {
                        Debug.WriteLine("Download ready. Success: " + r.IsFaulted);
                    });
            }
        }
    }
}
