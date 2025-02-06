using NativeBlazorRvLocalizationWebAssembly.Resources;
using Telerik.ReportViewer.BlazorNative.Services;

namespace NativeBlazorRvLocalizationWebAssembly.Services
{
    public class ReportViewerStringLocalizer : ITelerikReportingStringLocalizer
    {
        public string this[string key]
        {
            get
            {                 
                return ReportViewerMessages.ResourceManager.GetString(key, ReportViewerMessages.Culture) ?? key;
            }
        }
    }
}
