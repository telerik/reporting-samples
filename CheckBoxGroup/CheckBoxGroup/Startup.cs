using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Telerik.Reporting.Cache.File;
using Telerik.Reporting.Services;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace ASPCoreReportsService
{
    public class Startup
    {
        //Telerik.Reporting.Services.Engine.ResourcesEngine.
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddRazorPages().AddNewtonsoftJson();

            // Configure dependencies for ReportsController.
            services.TryAddSingleton<IReportServiceConfiguration>(sp =>
                  new ReportServiceConfiguration
                  {
                    // The default ReportingEngineConfiguration will be initialized from appsettings.json or appsettings.{EnvironmentName}.json:
                    ReportingEngineConfiguration = sp.GetService<IConfiguration>(),

                    // In case the ReportingEngineConfiguration needs to be loaded from a specific configuration file, use the approach below:
                    // ReportingEngineConfiguration = ResolveSpecificReportingConfiguration(sp.GetService<IHostingEnvironment>()),
                    HostAppId = "Html5DemoAppCore",
                      Storage = new FileStorage(),
                      ReportSourceResolver = new TypeReportSourceResolver()
                          .AddFallbackResolver(new UriReportSourceResolver(
                              System.IO.Path.Combine(sp.GetService<IHostingEnvironment>().ContentRootPath, "Reports"))),
                  });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles(); //for html5 viewer in .net core
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapGet("/", async context =>
                //{
                   // await context.Response.WriteAsync("Hello World!");
                //});
            });
        }
    }
}
