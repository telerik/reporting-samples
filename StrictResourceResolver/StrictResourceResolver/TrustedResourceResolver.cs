namespace StrictResourceResolver
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Telerik.Reporting.Interfaces;

    public sealed class TrustedResourceResolver : IStrictResourceResolver
    {
        const int MaximumImageBytes = 5 * 1024 * 1024;
        static readonly byte[] PngSignature = { 137, 80, 78, 71, 13, 10, 26, 10 };
        static readonly HttpClient SharedClient = new HttpClient(new HttpClientHandler
        {
            AllowAutoRedirect = false,
            UseCookies = false,
            UseDefaultCredentials = false
        });

        readonly string root;
        readonly string trustedImageUrl;
        readonly HttpClient client;
        readonly StringComparison pathComparison =
            Path.DirectorySeparatorChar == '\\'
                ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

        public TrustedResourceResolver(string directory, string trustedImageUrl)
            : this(directory, trustedImageUrl, SharedClient)
        {
        }

        internal TrustedResourceResolver(string directory, string trustedImageUrl, HttpClient client)
        {
            Uri imageUri;
            if (string.IsNullOrWhiteSpace(trustedImageUrl)
                || !Uri.TryCreate(trustedImageUrl, UriKind.Absolute, out imageUri)
                || imageUri.Scheme != Uri.UriSchemeHttps || !imageUri.IsDefaultPort
                || imageUri.UserInfo.Length != 0 || imageUri.Query.Length != 0
                || imageUri.Fragment.Length != 0
                || !string.Equals(trustedImageUrl, imageUri.AbsoluteUri, StringComparison.Ordinal)
                || !imageUri.AbsolutePath.EndsWith("/", StringComparison.Ordinal))
            {
                throw new ArgumentException("Specify an absolute HTTPS folder URL ending in '/' without credentials, query, or fragment.", nameof(trustedImageUrl));
            }

            if (string.IsNullOrWhiteSpace(directory) || !IsLocalAbsolutePath(directory))
            {
                throw new ArgumentException("Specify an absolute local resource directory.", nameof(directory));
            }

            this.root = Path.GetFullPath(directory).TrimEnd(Path.DirectorySeparatorChar,
                Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
            if (!Directory.Exists(this.root))
            {
                throw new DirectoryNotFoundException("The resource directory does not exist.");
            }
            if (HasLinkedComponent(this.root))
            {
                throw new ArgumentException("The resource directory cannot contain linked components.", nameof(directory));
            }

            this.trustedImageUrl = trustedImageUrl;
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public object Resolve(string resourceUri)
        {
            if (string.IsNullOrWhiteSpace(resourceUri))
            {
                return Deny();
            }

            if (IsTrustedImage(resourceUri))
            {
                return this.DownloadImageAsync(resourceUri).GetAwaiter().GetResult();
            }

            string path = resourceUri;
            if (resourceUri.StartsWith("file:", StringComparison.OrdinalIgnoreCase))
            {
                Uri uri;
                if (!Uri.TryCreate(resourceUri, UriKind.Absolute, out uri)
                    || !uri.IsFile || uri.IsUnc || uri.Host.Length != 0
                    || uri.Query.Length != 0 || uri.Fragment.Length != 0)
                {
                    return Deny();
                }
                path = uri.LocalPath;
            }

            string fullPath;
            try
            {
                // Reject network paths, URI schemes, and alternate data streams.
                if (path.StartsWith(@"\\", StringComparison.Ordinal)
                    || path.StartsWith("//", StringComparison.Ordinal)
                    || (Path.IsPathRooted(path) && !IsLocalAbsolutePath(path))
                    || path.IndexOf(':', IsLocalAbsolutePath(path) && Path.DirectorySeparatorChar == '\\' ? 2 : 0) >= 0)
                {
                    return Deny();
                }
                fullPath = Path.GetFullPath(Path.Combine(this.root, path));
            }
            catch (ArgumentException)
            {
                return Deny();
            }
            catch (NotSupportedException)
            {
                return Deny();
            }
            catch (PathTooLongException)
            {
                return Deny();
            }

            if (!fullPath.StartsWith(this.root, this.pathComparison))
            {
                return Deny();
            }
            if (HasLinkedComponent(fullPath))
            {
                return Deny();
            }

            var bytes = File.ReadAllBytes(fullPath);
            Trace.TraceInformation("The resource resolver approved a local resource.");
            return bytes;
        }

        bool IsTrustedImage(string resourceUri)
        {
            Uri uri;
            if (!Uri.TryCreate(resourceUri, UriKind.Absolute, out uri)
                || uri.Scheme != Uri.UriSchemeHttps || !uri.IsDefaultPort
                || uri.UserInfo.Length != 0 || uri.Query.Length != 0 || uri.Fragment.Length != 0
                || !string.Equals(resourceUri, uri.AbsoluteUri, StringComparison.Ordinal)
                || !resourceUri.StartsWith(this.trustedImageUrl, StringComparison.Ordinal))
            {
                return false;
            }

            var fileName = Uri.UnescapeDataString(resourceUri.Substring(this.trustedImageUrl.Length));
            return fileName.Length != 0 && fileName != "." && fileName != ".."
                && fileName.IndexOf('/') < 0 && fileName.IndexOf('\\') < 0;
        }

        static bool IsLocalAbsolutePath(string path)
        {
            if (Path.DirectorySeparatorChar == '\\')
            {
                return path.Length >= 3 && char.IsLetter(path[0]) && path[1] == ':'
                    && (path[2] == '\\' || path[2] == '/');
            }

            return path.StartsWith("/", StringComparison.Ordinal)
                && !path.StartsWith("//", StringComparison.Ordinal);
        }

        static bool HasLinkedComponent(string path)
        {
            var current = path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            if (current.Length < Path.GetPathRoot(path).Length)
            {
                current = Path.GetPathRoot(path);
            }
            while (!string.IsNullOrEmpty(current))
            {
                if ((File.GetAttributes(current) & FileAttributes.ReparsePoint) != 0)
                {
                    return true;
                }
                current = Path.GetDirectoryName(current);
            }
            return false;
        }

        static object Deny()
        {
            Trace.TraceWarning("The resource resolver denied a resource outside its policy.");
            return null;
        }

        async Task<byte[]> DownloadImageAsync(string resourceUri)
        {
            using (var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(15)))
            using (var response = await this.client.GetAsync(resourceUri,
                HttpCompletionOption.ResponseHeadersRead, timeout.Token).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                var contentType = response.Content.Headers.ContentType;
                if (contentType == null
                    || !string.Equals(contentType.MediaType, "image/png", StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidDataException("The trusted image response must be a PNG.");
                }
                if (response.Content.Headers.ContentLength > MaximumImageBytes)
                {
                    throw new InvalidDataException("The trusted image exceeds the size limit.");
                }

                using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                using (timeout.Token.Register(() => stream.Dispose()))
                using (var buffer = new MemoryStream())
                {
                    var chunk = new byte[8192];
                    int read;
                    while ((read = await stream.ReadAsync(chunk, 0, chunk.Length,
                        timeout.Token).ConfigureAwait(false)) != 0)
                    {
                        if (buffer.Length + read > MaximumImageBytes)
                        {
                            throw new InvalidDataException("The trusted image exceeds the size limit.");
                        }
                        buffer.Write(chunk, 0, read);
                    }
                    timeout.Token.ThrowIfCancellationRequested();
                    var bytes = buffer.ToArray();
                    if (bytes.Length < PngSignature.Length)
                    {
                        throw new InvalidDataException("The trusted image has no PNG signature.");
                    }
                    for (var i = 0; i < PngSignature.Length; i++)
                    {
                        if (bytes[i] != PngSignature[i])
                        {
                            throw new InvalidDataException("The trusted image has an invalid PNG signature.");
                        }
                    }
                    return bytes;
                }
            }
        }
    }
}
