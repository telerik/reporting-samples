using Telerik.Reporting.Cache.File;
using Telerik.Reporting.Services;
using StrictResourceResolver;

var builder = WebApplication.CreateBuilder(args);
System.Diagnostics.Trace.Listeners.Add(new System.Diagnostics.ConsoleTraceListener());
var directoryKey = "telerikReporting:processing:resourceResolver:parameters:1:value";
var directory = builder.Configuration[directoryKey]
    ?? throw new InvalidOperationException("Configure the resource directory.");
builder.Configuration[directoryKey] = Path.GetFullPath(directory, builder.Environment.ContentRootPath);
var trustedImageUrl = builder.Configuration["telerikReporting:processing:resourceResolver:parameters:2:value"]
    ?? throw new InvalidOperationException("Configure the trusted image URL.");

// Validate the same arguments the Reporting configuration factory will use.
_ = new TrustedResourceResolver(builder.Configuration[directoryKey]!, trustedImageUrl);

builder.Services.AddControllers();
builder.Services.AddSingleton<IReportServiceConfiguration>(sp => new ReportServiceConfiguration
{
    HostAppId = "StrictResourceResolverSample",
    ReportingEngineConfiguration = sp.GetRequiredService<IConfiguration>(),
    Storage = new FileStorage(),
    ReportSourceResolver = new UriReportSourceResolver(
        Path.Combine(builder.Environment.ContentRootPath, "Reports"))
});

var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();
app.Run();
