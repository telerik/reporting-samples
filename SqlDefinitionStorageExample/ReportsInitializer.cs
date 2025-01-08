using Microsoft.AspNetCore.Builder;
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

                if (!context.Resources.Any())
                {
                    var sampleReport = CreateResource("SampleReport.trdp", "Reports");
                    var reportsFolder = CreateFolderModel("Reports", string.Empty);
                    var resourcesFolder = CreateFolderModel("Resources", string.Empty);

                    context.Resources.Add(sampleReport.ToDbResourceModel(System.IO.File.ReadAllBytes("SampleReport.trdp")));
                    context.ResourceFolders.Add(reportsFolder.ToDbResourceFolderModel());
                    context.ResourceFolders.Add(resourcesFolder.ToDbResourceFolderModel());

                    context.SaveChanges();
                }

                return app;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static CreateFolderModel CreateFolderModel(string name, string parentUri) => new()
        {
            Name = name,
            ParentUri = parentUri
        };

        public static SaveResourceModel CreateResource(string name, string parentUri) => new()
        {
            Name = name,
            ParentUri = parentUri
        };
    }
}
