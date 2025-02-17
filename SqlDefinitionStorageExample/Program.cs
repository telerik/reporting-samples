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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddRazorPages()
                .AddNewtonsoftJson();

builder.Services.AddDbContext<SqlDefinitionStorageContext>();
builder.Services.AddScoped<IDefinitionStorage, CustomDefinitionStorage>();
builder.Services.AddScoped<ISharedDataSourceStorage, CustomSharedDataSourceStorage>();
builder.Services.AddScoped<IResourceStorage, CustomResourceStorage>();
builder.Services.AddScoped<IReportSourceResolver, CustomReportSourceResolver>();
builder.Services.AddScoped<IReportDocumentResolver, CustomReportDocumentResolver>();

// Configure dependencies for ReportsController.
builder.Services.TryAddScoped<IReportServiceConfiguration>(sp =>
    new ReportServiceConfiguration
    {
        ReportingEngineConfiguration = sp.GetService<IConfiguration>(),
        HostAppId = "WebDesignerMsSqlStorage",
        Storage = new FileStorage(),
        ReportSourceResolver = sp.GetRequiredService<IReportSourceResolver>(),
        ReportDocumentResolver = sp.GetRequiredService<IReportDocumentResolver>()
    });

// Configure dependencies for ReportDesignerController.
builder.Services.TryAddScoped<IReportDesignerServiceConfiguration>(sp => new ReportDesignerServiceConfiguration
{
    DefinitionStorage = sp.GetRequiredService<IDefinitionStorage>(),
    ResourceStorage = sp.GetRequiredService<IResourceStorage>(),
    SharedDataSourceStorage = sp.GetRequiredService<ISharedDataSourceStorage>(),
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

// Add initial data to database
app.Seed();

app.Run();
