using Telerik.WebReportDesigner.Services;
using Telerik.Reporting.Cache.File;
using Telerik.Reporting.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages().AddNewtonsoftJson();
            services.AddControllers();
            services.AddMvc();

            services.TryAddSingleton((Func<IServiceProvider, IReportServiceConfiguration>)(sp =>
                new ReportServiceConfiguration
                {
                    ReportingEngineConfiguration = sp.GetService<IConfiguration>(),
                    HostAppId = "WebReportDesignerApp",
                    Storage = new FileStorage(),
                    ReportSourceResolver = new UriReportSourceResolver(GetReportsDir(sp))
                }));

            services.TryAddSingleton<IReportDesignerServiceConfiguration>(sp => new ReportDesignerServiceConfiguration
            {
                DefinitionStorage = new FileDefinitionStorage(
                    GetReportsDir(sp)),
                SettingsStorage = new FileSettingsStorage(
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Telerik Reporting")),
            });
            services.AddControllersWithViews();
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
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
              endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        static string GetReportsDir(IServiceProvider sp)
        {
            return Path.Combine(sp.GetService<IWebHostEnvironment>().ContentRootPath, "Reports");
        }
    }
}
