using Telerik.WebReportDesigner.Services;
using Telerik.Reporting.Cache.File;
using Telerik.Reporting.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddMvc();
builder.Services.TryAddSingleton((Func<IServiceProvider, IReportServiceConfiguration>)(sp =>
	new ReportServiceConfiguration
	{
		ReportingEngineConfiguration = sp.GetService<IConfiguration>(),
		HostAppId = "WebReportDesignerApp",
		Storage = new FileStorage(),
		ReportSourceResolver = new UriReportSourceResolver(GetReportsDir(sp))
	}));
builder.Services.TryAddSingleton<IReportDesignerServiceConfiguration>(sp => new ReportDesignerServiceConfiguration
{
	DefinitionStorage = new FileDefinitionStorage(
		GetReportsDir(sp), new[] { "Resources" }),
	ResourceStorage = new ResourceStorage(
		Path.Combine(sp.GetService<IWebHostEnvironment>().ContentRootPath, "Resources")),
	SharedDataSourceStorage = new FileSharedDataSourceStorage(
		Path.Combine(sp.GetService<IWebHostEnvironment>().ContentRootPath, "Shared Data Sources")),
	SettingsStorage = new FileSettingsStorage(
		Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Telerik Reporting"))
});


// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddNewtonsoftJson();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
	endpoints.MapControllers();
});


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

static string GetReportsDir(IServiceProvider sp)
{
	return Path.Combine(sp.GetService<IWebHostEnvironment>().ContentRootPath, "Reports");
}
