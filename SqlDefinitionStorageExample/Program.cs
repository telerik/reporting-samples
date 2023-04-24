using SqlDefinitionStorageExample;
using SqlDefinitionStorageExample.EFCore;
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

builder.Services.AddDbContext<SqlDefinitionStorageContext>();
builder.Services.AddScoped<IDefinitionStorage, CustomDefinitionStorage>();
builder.Services.AddScoped<IReportSourceResolver, CustomReportSourceResolver>();

var reportsPath = Path.Combine(builder.Environment.ContentRootPath, "Reports");

// Configure dependencies for ReportsController.
builder.Services.TryAddScoped<IReportServiceConfiguration>(sp =>
    new ReportServiceConfiguration
    {
        // The default ReportingEngineConfiguration will be initialized from appsettings.json or appsettings.{EnvironmentName}.json:
        ReportingEngineConfiguration = sp.GetService<IConfiguration>(),

        // In case the ReportingEngineConfiguration needs to be loaded from a specific configuration file, use the approach below:
        //ReportingEngineConfiguration = ResolveSpecificReportingConfiguration(sp.GetService<IWebHostEnvironment>()),
        HostAppId = "SqlDefinitionStorageExample",
        Storage = new FileStorage(),
        ReportSourceResolver = sp.GetRequiredService<IReportSourceResolver>(),
    });

// Configure dependencies for ReportDesignerController.
builder.Services.TryAddScoped<IReportDesignerServiceConfiguration>(sp => new ReportDesignerServiceConfiguration
{
    DefinitionStorage = sp.GetRequiredService<IDefinitionStorage>(),
    ResourceStorage = new ResourceStorage(Path.Combine(reportsPath, "Resources")),
    SharedDataSourceStorage = new FileSharedDataSourceStorage(Path.Combine(reportsPath, "Shared Data Sources")),
    SettingsStorage = new FileSettingsStorage(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Telerik Reporting"))
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

using (var serviceScope = app.Services.CreateScope())
{
    serviceScope.ServiceProvider
        .GetService<SqlDefinitionStorageContext>()
        .Database
        .EnsureCreated();
}

app.UseStaticFiles();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Initialize the paths for the custom resource resolver.            
// Shows how to initialize a custom IResourceResolver implementation with the folder used for report resources retrieval.
// This step is not mandatory and is added for demonstration purposes only.
CustomResourceResolver.Configuration.Instance.Init(reportsPath,
                                                   Path.Combine(reportsPath, "Resources"));

// Initialize the paths for the custom SharedDataSource resolver.            
// Shows how to initialize a custom ISharedDataSourceResolver implementation with the folder used for reports and shared data sources.
// This step is not mandatory and is added for demonstration purposes only.
CustomSharedDataSourceResolver.Configuration.Instance.Init(reportsPath,
                                                   Path.Combine(reportsPath, "Shared Data Sources"));

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