using NativeBlazorRvLocalizationWebAssembly.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using System.Globalization;
using Telerik.Blazor.Services;
using Telerik.ReportViewer.BlazorNative.Services;

namespace NativeBlazorRvLocalizationWebAssembly
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
            builder.Services.AddTelerikBlazor();
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            
            builder.Services.AddSingleton(typeof(ITelerikStringLocalizer), typeof(TelerikBlazorUiStringLocalizer));
            builder.Services.AddSingleton(typeof(ITelerikReportingStringLocalizer), typeof(ReportViewerStringLocalizer));

            var host = builder.Build();

            const string defaultCulture = "bg-BG";
            var culture = CultureInfo.GetCultureInfo(defaultCulture);

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            await host.RunAsync();
        }
    }
}
