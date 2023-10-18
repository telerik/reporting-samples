using CSharp.Net6.Html5IntegrationDemo.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using System.Buffers;
using System.IO;
using System.Linq;
using Telerik.Reporting.Cache.File;
using Telerik.Reporting.Services;

EnableTracing();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(opts => { })
    .AddNewtonsoftJson(opts => { /**/ })
    .AddJsonOptions(opts => { /**/ });

builder.Services.AddOptions<MvcOptions>()
    .PostConfigure<IOptions<JsonOptions>, IOptions<MvcNewtonsoftJsonOptions>, ArrayPool<char>, ObjectPoolProvider, ILoggerFactory>((opts, jsonOpts, newtonJsonOpts, charPool, objectPoolProvider, loggerFactory) => {
        // configure System.Text.Json formatters
        if (opts.InputFormatters.OfType<SystemTextJsonInputFormatter>().Count() == 0)
        {
            var systemInputlogger = loggerFactory.CreateLogger<SystemTextJsonInputFormatter>();
            opts.InputFormatters.Add(new SystemTextJsonInputFormatter(jsonOpts.Value, systemInputlogger));
        }
        if (opts.OutputFormatters.OfType<SystemTextJsonOutputFormatter>().Count() == 0)
        {
            opts.OutputFormatters.Add(new SystemTextJsonOutputFormatter(jsonOpts.Value.JsonSerializerOptions));
        }
        // configure Newtonjson formatters
        if (opts.InputFormatters.OfType<NewtonsoftJsonInputFormatter>().Count() == 0)
        {
            var inputLogger = loggerFactory.CreateLogger<NewtonsoftJsonInputFormatter>();
            opts.InputFormatters.Add(new NewtonsoftJsonInputFormatter(
                inputLogger, newtonJsonOpts.Value.SerializerSettings, charPool, objectPoolProvider, opts, newtonJsonOpts.Value
            ));
        }
        if (opts.OutputFormatters.OfType<NewtonsoftJsonOutputFormatter>().Count() == 0)
        {
            opts.OutputFormatters.Add(new NewtonsoftJsonOutputFormatter(newtonJsonOpts.Value.SerializerSettings, charPool, opts));
        }
        opts.InputFormatters.Insert(0, new MySuperJsonInputFormatter());
        opts.OutputFormatters.Insert(0, new MySuperJsonOutputFormatter());
    });

builder.Services.AddRazorPages();

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
        Storage = new FileStorage(),
        ReportSourceResolver = new TypeReportSourceResolver()
                                    .AddFallbackResolver(
                                        new UriReportSourceResolver(reportsPath))
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