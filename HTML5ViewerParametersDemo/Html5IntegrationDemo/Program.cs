using CSharp.Net6.Html5IntegrationDemo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using Telerik.Reporting.Cache.File;
using Telerik.Reporting.Services;
using Telerik.WebReportDesigner.Services;

EnableTracing();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddRazorPages()
                .AddNewtonsoftJson();


var reportsPath = Path.Combine(builder.Environment.ContentRootPath, "Reports");

// Configure dependencies for ReportsController.
builder.Services.TryAddSingleton<IReportServiceConfiguration>(sp =>
    new ReportServiceConfiguration
    {
        // The default ReportingEngineConfiguration will be initialized from appsettings.json or appsettings.{EnvironmentName}.json:
        ReportingEngineConfiguration = sp.GetService<IConfiguration>(),

        // In case the ReportingEngineConfiguration needs to be loaded from a specific configuration file, use the approach below:
        //ReportingEngineConfiguration = ResolveSpecificReportingConfiguration(sp.GetService<IWebHostEnvironment>()),
        HostAppId = "ReportingNet6",
        Storage = new FileStorage("C:\\FileStorage"),
        ReportSourceResolver = new TypeReportSourceResolver()
                                    .AddFallbackResolver(
                                        new UriReportSourceResolver(reportsPath))
    });

// Configure dependencies for ReportDesignerController.
builder.Services.TryAddSingleton<IReportDesignerServiceConfiguration>(sp => new ReportDesignerServiceConfiguration
{
    DefinitionStorage = new FileDefinitionStorage(reportsPath, new[] { "Resources" }),
    ResourceStorage = new ResourceStorage(Path.Combine(reportsPath, "Resources")),
    SettingsStorage = new FileSettingsStorage(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Telerik Reporting"))    
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Initialize the paths for the custom resource resolver.            
// Shows how to initialize a custom IResourceResolver implementation with the folder used for report resources retrieval.
// This step is not mandatory and is added for demonstration purposes only.
CustomResourceResolver.Configuration.Instance.Init(reportsPath,
                                                   Path.Combine(reportsPath, "Resources"));

app.Run();

/// <summary>
/// Uncomment the lines to enable tracing in the current application.
/// The trace log will be persisted in a file named log.txt in the application root directory.
/// </summary>
static void EnableTracing()
{
    // System.Diagnostics.Trace.Listeners.Add(new System.Diagnostics.TextWriterTraceListener(File.CreateText("log.txt")));
    // System.Diagnostics.Trace.AutoFlush = true;
}

/// <summary>
/// Loads a reporting configuration from a specific JSON-based configuration file.
/// </summary>
/// <param name="environment">The current web hosting environment used to obtain the content root path</param>
/// <returns>IConfiguration instance used to initialize the Reporting engine</returns>
static IConfiguration ResolveSpecificReportingConfiguration(IWebHostEnvironment environment)
{
    // If a specific configuration needs to be passed to the reporting engine, add it through a new IConfiguration instance.
    var reportingConfigFileName = Path.Combine(environment.ContentRootPath, "reportingAppSettings.json");
    return new ConfigurationBuilder()
        .AddJsonFile(reportingConfigFileName, true)
        .Build();
}