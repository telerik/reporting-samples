using Microsoft.AspNetCore.Components.WebView.Maui;
using ReportingMauiBlazor.Data;

namespace ReportingMauiBlazor;

public static class MauiProgram
{
	// You may preview but cannot print or export the reports with this solution.
	// The reason for the limitations is that the MAUI WebView is not a full-blown browser and printing and downloading do not come out-of-the-box.
	// For the supported functionality you can check the WebView description in the MS documentation linked below:
	// https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/webview?pivots=devices-android
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();
#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif

		builder.Services.AddSingleton<WeatherForecastService>();

		return builder.Build();
	}
}
