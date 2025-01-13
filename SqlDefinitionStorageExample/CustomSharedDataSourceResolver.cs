namespace SqlDefinitionStorageExample
{
    using Microsoft.EntityFrameworkCore;
    using SqlDefinitionStorageExample.EFCore;
    using System.IO;
    using System.Linq;
    using Telerik.WebReportDesigner.Services;
    using Data = Telerik.Reporting.Processing.Data;

    class CustomSharedDataSourceResolver : Data.ISharedDataSourceResolver
    {

        readonly string _root = "Shared Data Sources";
        Telerik.Reporting.DataSource Data.ISharedDataSourceResolver.Resolve(string sharedDataSourcePath)
        {

            if (!sharedDataSourcePath.Contains($"{_root}\\"))
            {
                sharedDataSourcePath = $"{_root}\\{sharedDataSourcePath}";
            }

            sharedDataSourcePath = sharedDataSourcePath.Replace("/", "\\");

            var optionsBuilder = new DbContextOptionsBuilder<SqlDefinitionStorageContext>();
            // It is necessary to initialize a new dbContent because this code will be executed in a new thread
            using SqlDefinitionStorageContext dbContext = new(optionsBuilder.Options);

            var sds = dbContext.Resources.FirstOrDefault(m => m.Uri == sharedDataSourcePath) ?? throw new ResourceNotFoundException();
            using var ms = new MemoryStream(sds.Bytes);

            return (Telerik.Reporting.DataSource)new Telerik.Reporting.XmlSerialization.ReportXmlSerializer()
                .Deserialize(ms);
        }
    }
}
