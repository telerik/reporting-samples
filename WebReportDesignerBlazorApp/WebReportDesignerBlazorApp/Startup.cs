namespace WebReportDesignerBlazorApp
{
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
    using Telerik.Reporting.Services.AspNetCore;
    using Telerik.Reporting.Services.Engine;
    using Telerik.WebReportDesigner.Services;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();

            services.AddRazorPages()
                .AddNewtonsoftJson();
            services.AddServerSideBlazor();

            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });


            services.TryAddSingleton<IReportDesignerServiceConfiguration>(sp => new ReportDesignerServiceConfiguration
            {
                DefinitionStorage = new FileDefinitionStorage(
                  System.IO.Path.Combine(sp.GetService<IWebHostEnvironment>().ContentRootPath, "Reports")),
                SettingsStorage = new FileSettingsStorage(
                   Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Telerik Reporting"))
            });

            services.TryAddScoped<IReportSourceResolver>(sp =>
                new TypeReportSourceResolver().AddFallbackResolver(new UriReportSourceResolver(
                    Path.Combine(sp.GetService<IWebHostEnvironment>().ContentRootPath, "Reports"))));
            services.TryAddScoped<IReportServiceConfiguration>(sp =>
                new ReportServiceConfiguration
                {
                    ReportingEngineConfiguration = ConfigurationHelper.ResolveConfiguration(sp.GetService<IWebHostEnvironment>()),
                    HostAppId = "Html5DemoAppCore",
                    Storage = new FileStorage(),
                    ReportSourceResolver = sp.GetRequiredService<IReportSourceResolver>()
                });

            // Configure dependencies for ReportDesignerServiceConfiguration.  
            services.TryAddScoped<IDefinitionStorage>(sp => new FileDefinitionStorage(Path.Combine(sp.GetService<IWebHostEnvironment>().ContentRootPath, "Reports")));
            services.TryAddScoped<IReportDesignerServiceConfiguration>(sp => new ReportDesignerServiceConfiguration { DefinitionStorage = sp.GetRequiredService<IDefinitionStorage>() });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }

        /// <summary>
        /// Loads a reporting configuration from a specific JSON-based configuration file.
        /// </summary>
        /// <param name="environment">The current web hosting environment used to obtain the content root path</param>
        /// <returns>IConfiguration instance used to initialize the Reporting engine</returns>
        static IConfiguration ResolveSpecificReportingConfiguration(IWebHostEnvironment environment)
        {
            // If a specific configuration needs to be passed to the reporting engine, add it through a new IConfiguration instance.
            var reportingConfigFileName = System.IO.Path.Combine(environment.ContentRootPath, "reportingAppSettings.json");
            return new ConfigurationBuilder()
                .AddJsonFile(reportingConfigFileName, true)
                .Build();
        }
    }

    static class ConfigurationHelper
    {
        public static IConfiguration ResolveConfiguration(IWebHostEnvironment environment)
        {
            var reportingConfigFileName = System.IO.Path.Combine(environment.ContentRootPath, "appsettings.json");
            return new ConfigurationBuilder()
                .AddJsonFile(reportingConfigFileName, true)
                .Build();
        }
    }
}
