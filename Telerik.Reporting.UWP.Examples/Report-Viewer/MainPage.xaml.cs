using RuntimeComponent;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Printing3D;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Report_Viewer
{
    public sealed partial class MainPage : Page
    {
        private const string viewerDomain = "localhost";
        private static readonly string viewerDomainUrl = string.Format("http://{0}:57615/", viewerDomain);
        private static readonly Uri viewerUri = new Uri(viewerDomainUrl + "viewer.html");

        public MainPage()
        {
            this.InitializeComponent();
            this.webView1.Source = viewerUri;
        }

        private void WebView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            if (string.Equals(args.Uri.Host, viewerDomain, StringComparison.Ordinal))
            {
                webView1.AddWebAllowedObject("nativeHandler", new RuntimeComponent.ViewerEventHandler(viewerDomainUrl));
            }
        }

        private async void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            await WebView.ClearTemporaryWebDataAsync();
            webView1.Navigate(viewerUri);
        }
    }
}
