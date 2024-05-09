namespace CSharp.Net8.Html5IntegrationDemo
{
    using System;
    using System.IO;
    using Telerik.Reporting.Processing;
    using Telerik.WebReportDesigner.Services;
    using Data = Telerik.Reporting.Processing.Data;

    /// <summary>
    /// An example of a custom implementation of <see cref="Data.ISharedDataSourceResolver"/>.
    /// The relative sharedDataSourcePath is resolved to absolute path using the <see cref="Configuration.Instance"/> paths.
    /// If at any of the locations the requested .sdsx file, is found, the <see cref="CustomSharedDataSourceResolver"/> deserializes the file contents of the provided path and instantiates a DataSource class.
    /// The CustomSharedDataSourceResolver needs to be registered in application's configuration file appsettings.json.
    /// </summary>
    class CustomSharedDataSourceResolver : Data.ISharedDataSourceResolver
    {
        /// <summary>
        /// Resolves and returns a DataSource instance from the provided <paramref name="sharedDataSourcePath"/> parameter.
        /// </summary>
        /// <param name="sharedDataSourcePath">The value of the Path property obtained from the report definition. Might be relative or absolute.</param>
        /// <returns></returns>
        Telerik.Reporting.DataSource Data.ISharedDataSourceResolver.Resolve(string sharedDataSourcePath)
        {
            ValidateConfiguration();

            var absolutePathToSharedDataSourceDefinition =
                GetExistingFilePath(Configuration.Instance.ReportsPath, sharedDataSourcePath)
                ?? GetExistingFilePath(Configuration.Instance.SharedDataSourcesPath, sharedDataSourcePath);

            if (string.IsNullOrEmpty(absolutePathToSharedDataSourceDefinition))
            {
                throw new NullReferenceException($"The path {sharedDataSourcePath} cannot be resolved.");
            }

            using (var fs = new FileStream(absolutePathToSharedDataSourceDefinition, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return (Telerik.Reporting.DataSource)new Telerik.Reporting.XmlSerialization.ReportXmlSerializer()
                    .Deserialize(fs);
            }
        }

        static string GetExistingFilePath(string basePath, string sharedDataSourcePath)
        {
            var sharedDataSourceAbsolutePath = new PathResourceResolver(basePath)
                .Resolve(sharedDataSourcePath) as string;

            if (!string.IsNullOrEmpty(sharedDataSourceAbsolutePath) && File.Exists(sharedDataSourceAbsolutePath))
            {
                return sharedDataSourceAbsolutePath;
            }

            return null;
        }

        static void ValidateConfiguration()
        {
            if (string.IsNullOrEmpty(Configuration.Instance.ReportsPath) || string.IsNullOrEmpty(Configuration.Instance.SharedDataSourcesPath))
            {
                throw new NullReferenceException("The configuration of the CustomSharedDataSourceResolver is not initialized. Please make sure you've called \"Configuration.Instance.Init(string reportsPath, string sharedDataSourcesPath)\" method and have provided valid paths as arguments.");
            }
        }

        /// <summary>
        /// Class that stores the paths to Reports and Shared Data Sources folders.
        /// </summary>
        internal class Configuration
        {
            public static Configuration Instance = new();

            public string ReportsPath { get; private set; }

            public string SharedDataSourcesPath { get; private set; }

            public void Init(string reportsPath, string sharedDataSourcesPath)
            {
                this.ReportsPath = reportsPath;
                this.SharedDataSourcesPath = sharedDataSourcesPath;
            }
        }
    }
}
