namespace SqlDefinitionStorageExample
{
    using System;
    using System.IO;
    using Telerik.Reporting.Interfaces;
    using Telerik.Reporting.Processing;

    /// <summary>
    /// A custom implementation of IResourceResolver. 
    /// It uses two instances of PathResourceResolver class and tries to resolve resource consecutively from both of them.
    /// The class is registered in application's configuration file appsettings.json.
    /// </summary>
    class CustomResourceResolver : IResourceResolver
    {
        /// <summary>
        /// This resolver locates file resources in \Reports folder and returns their absolute path.
        /// </summary>
        readonly PathResourceResolver reportsFolderResourceResolver;

        /// <summary>
        /// This resolver locates file resources in \Resources folder and returns their absolute path.
        /// </summary>
        readonly PathResourceResolver resourcesFolderResourceResolver;

        /// <summary>
        /// This resolver returns the resource's raw data as byte[] from the provided URI.
        /// </summary>
        /// <remarks>
        /// It is declared and used here for demonstrational purposes only. 
        /// The reports in this example project should have their resources retrieved only by using both PathResourceResolver instances declared above.
        /// </remarks>
        readonly RawDataResourceResolver rawDataResourceResolver;

        public CustomResourceResolver()
        {
            this.reportsFolderResourceResolver = new PathResourceResolver(Configuration.Instance.ReportsPath);
            this.resourcesFolderResourceResolver = new PathResourceResolver(Configuration.Instance.ResourcesPath);
            this.rawDataResourceResolver = new RawDataResourceResolver();
        }

        public object Resolve(string resourceUri)
        {
            return ResolveUri(this.reportsFolderResourceResolver, resourceUri)
                ?? ResolveUri(this.resourcesFolderResourceResolver, resourceUri)
                ?? this.rawDataResourceResolver.Resolve(resourceUri);
        }

        static string ResolveUri(IResourceResolver resourceResolver, string resourceUri)
        {
            var resolvedUri = (string)resourceResolver.Resolve(resourceUri);

            if (!string.IsNullOrEmpty(resolvedUri))
            {
                if (Uri.TryCreate(resolvedUri, UriKind.RelativeOrAbsolute, out Uri uri))
                {
                    if (!uri.IsFile) // Handle the case when the resourceUri is a URL, like in the ListBoundReport example.
                    {
                        return uri.AbsoluteUri;
                    }
                    else if (File.Exists(uri.LocalPath)) // If the resourceUri points to a file, ensure it is retrievable.
                    {
                        return resolvedUri;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Class that stores the paths to Reports and Resources folders.
        /// </summary>
        internal class Configuration
        {
            public static Configuration Instance = new();

            public string ReportsPath { get; private set; }

            public string ResourcesPath { get; private set; }

            public void Init(string reportsPath, string resourcePath)
            {
                this.ReportsPath = reportsPath;
                this.ResourcesPath = resourcePath;
            }
        }

        /// <summary>
        /// Class retrieving the raw data as byte[] from a resource URI.
        /// </summary>
        internal class RawDataResourceResolver : IResourceResolver
        {
            public object Resolve(string resourceUri)
            {
                if (!string.IsNullOrEmpty(resourceUri))
                {
                    if (Uri.TryCreate(resourceUri, UriKind.RelativeOrAbsolute, out Uri uri))
                    {
                        if (!uri.IsFile) // Handle the case when the resourceUri is a URL, like in the ListBoundReport example.
                        {
                            using (var httpClient = new System.Net.Http.HttpClient())
                            {
                                return httpClient.GetByteArrayAsync(uri).GetAwaiter().GetResult();
                            }
                        }
                        else if (File.Exists(uri.LocalPath)) // If the resourceUri points to a file, ensure it is retrievable.
                        {
                            return File.ReadAllBytes(uri.LocalPath);
                        }
                    }
                }

                return null;
            }
        }
    }
}
