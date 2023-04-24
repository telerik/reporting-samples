using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SqlDefinitionStorageExample.EFCore;
using SqlDefinitionStorageExample.EFCore.Models;
using System;
using System.Linq;
using Telerik.WebReportDesigner.Services.Models;

namespace SqlDefinitionStorageExample
{
    public static class ReportsInitializer
    {
        public static WebApplication Seed(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context =  scope.ServiceProvider.GetRequiredService<SqlDefinitionStorageContext>();

            try {

                if (!context.Reports.Any())
                {
                        var saveResourceModel = new SaveResourceModel()
                        {
                            Name = "SampleReport.trdp",
                            ParentUri = string.Empty
                        };

                        var reportBytes = System.IO.File.ReadAllBytes("SampleReport.trdp");
                        var entity = saveResourceModel.ToDbReportModel(reportBytes);

                        context.Reports.Add(entity);
                        context.SaveChanges();
                }

                return app;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
