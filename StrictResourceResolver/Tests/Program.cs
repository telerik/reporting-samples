namespace StrictResourceResolver.Tests
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using ReportProcessor = Telerik.Reporting.Processing.ReportProcessor;
    using RenderingResult = Telerik.Reporting.Processing.RenderingResult;

    static class Program
    {
        const string SampleImageFolder =
            "https://raw.githubusercontent.com/telerik/reporting-docs/master/report-items/images/";
        const string SampleImageUrl =
            "https://raw.githubusercontent.com/telerik/reporting-docs/master/report-items/images/Textbox.png";

        static readonly byte[] Png = Convert.FromBase64String(
            "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mNk+A8AAQUBAScY42YAAAAASUVORK5CYII=");

        static void Main(string[] args)
        {
            var workspace = Path.Combine(Path.GetTempPath(), "StrictResolver-" + Guid.NewGuid().ToString("N"));
            var root = Path.Combine(workspace, "Resources");
            Directory.CreateDirectory(root);
            var localImage = Path.Combine(root, "image.png");
            File.WriteAllBytes(localImage, Png);
            File.WriteAllBytes(Path.Combine(workspace, "outside.png"), Png);
            var sibling = root + "Other";
            Directory.CreateDirectory(sibling);
            File.WriteAllBytes(Path.Combine(sibling, "image.png"), Png);

            try
            {
                CheckFiles(root, localImage, sibling);
                CheckDownloads(root);
                if (args.Contains("--public-image"))
                {
                    var bytes = (byte[])new TrustedResourceResolver(root, SampleImageFolder).Resolve(SampleImageUrl);
                    Assert(bytes.Length > 8, "Public image download is empty.");
                    Console.WriteLine("Public image URL checked.");
                }
                if (args.Contains("--integration"))
                {
                    CheckReporting(root);
                }
                Console.WriteLine("All requested checks passed on " + Environment.Version + ".");
            }
            finally
            {
                File.Delete(localImage);
                File.Delete(Path.Combine(workspace, "outside.png"));
                File.Delete(Path.Combine(sibling, "image.png"));
                Directory.Delete(sibling);
                Directory.Delete(root);
                Directory.Delete(workspace);
            }
        }

        static void CheckFiles(string root, string image, string sibling)
        {
            var resolver = new TrustedResourceResolver(root, SampleImageFolder);
            foreach (var resource in new[] { "image.png", image, new Uri(image).AbsoluteUri })
            {
                Assert(Png.SequenceEqual((byte[])resolver.Resolve(resource)), "Approved file bytes differ.");
            }
            foreach (var resource in new[]
            {
                null, "", " ", "invalid\0.png", @"..\outside.png", Path.Combine(sibling, "image.png"),
                @"\\server\share\image.png", @"\\?\C:\image.png", @"C:image.png",
                "image.png:other", "https://example.com/image.png",
                SampleImageUrl + "?x=1",
                SampleImageUrl + "#image",
                SampleImageUrl.Replace("/telerik/", "/other/"),
                SampleImageUrl.Replace("https:", "http:")
            })
            {
                Assert(resolver.Resolve(resource) == null, "Unapproved resource was allowed.");
            }
            Throws<ArgumentException>(() => new TrustedResourceResolver("relative", SampleImageFolder));
            Throws<ArgumentException>(() => new TrustedResourceResolver(null, SampleImageFolder));
            foreach (var url in new[]
            {
                null, "", "relative", "http://example.com/image.png",
                "https://user:password@example.com/image.png",
                "https://example.com:8443/image.png",
                SampleImageUrl, SampleImageFolder.TrimEnd('/'),
                SampleImageFolder + "?x=1", SampleImageFolder + "#image"
            })
            {
                Throws<ArgumentException>(() => new TrustedResourceResolver(root, url));
            }
            Throws<FileNotFoundException>(() => resolver.Resolve("missing.png"));
        }

        static void CheckDownloads(string root)
        {
            using (var handler = new FakeHandler(_ => Response(HttpStatusCode.OK, Png, "image/png")))
            using (var client = new HttpClient(handler))
            {
                var resolver = new TrustedResourceResolver(root, SampleImageFolder, client);
                foreach (var url in new[]
                {
                    SampleImageUrl + "?x=1", SampleImageUrl + "#image", SampleImageFolder,
                    SampleImageFolder.TrimEnd('/') + "-other/image.png",
                    SampleImageFolder + "nested/image.png",
                    SampleImageFolder + "../outside.png",
                    SampleImageFolder + "%2e%2e/outside.png",
                    SampleImageFolder + "nested%2fimage.png",
                    SampleImageFolder + "nested%5cimage.png",
                    SampleImageUrl.Replace("raw.githubusercontent.com", "raw.githubusercontent.com.example.org"),
                    SampleImageUrl.Replace("https:", "http:")
                })
                {
                    Assert(resolver.Resolve(url) == null, "Outside-folder or invalid URL was allowed: " + url);
                }
                Assert(handler.Calls == 0, "Denied URL reached the downloader.");
                Assert(Png.SequenceEqual((byte[])resolver.Resolve(SampleImageUrl)),
                    "Approved download bytes differ.");
                Assert(handler.Calls == 1, "Expected exactly one download.");
            }
            const string anotherImage = SampleImageFolder + "Shape.png";
            using (var handler = new FakeHandler(_ => Response(HttpStatusCode.OK, Png, "image/png"), anotherImage))
            using (var client = new HttpClient(handler))
            {
                var resolver = new TrustedResourceResolver(root, SampleImageFolder, client);
                Assert(Png.SequenceEqual((byte[])resolver.Resolve(anotherImage)),
                    "Another image in the trusted folder was denied.");
                Assert(handler.Calls == 1, "Expected download for another trusted-folder image.");
            }
            const string alternateFolder = "https://example.com/images/";
            const string alternateUrl = alternateFolder + "approved.png";
            using (var handler = new FakeHandler(_ => Response(HttpStatusCode.OK, Png, "image/png"), alternateUrl))
            using (var client = new HttpClient(handler))
            {
                var resolver = new TrustedResourceResolver(root, alternateFolder, client);
                Assert(resolver.Resolve(SampleImageUrl) == null, "Previously configured URL remained approved.");
                Assert(handler.Calls == 0, "Unconfigured URL reached the downloader.");
                Assert(Png.SequenceEqual((byte[])resolver.Resolve(alternateUrl)), "Configured URL was not downloaded.");
            }
            using (var handler = new FakeHandler(_ =>
            {
                var response = Response(HttpStatusCode.Redirect, Png, "image/png");
                response.Headers.Location = new Uri("https://example.com/image.png");
                return response;
            }))
            using (var client = new HttpClient(handler))
            {
                Throws<HttpRequestException>(() =>
                    new TrustedResourceResolver(root, SampleImageFolder, client).Resolve(SampleImageUrl));
                Assert(handler.Calls == 1, "Redirect initiated another request.");
            }
            CheckDownloadFailure<HttpRequestException>(root,
                () => Response(HttpStatusCode.NotFound, Png, "image/png"));
            CheckDownloadFailure<InvalidDataException>(root,
                () => Response(HttpStatusCode.OK, Png, "text/html"));
            CheckDownloadFailure<InvalidDataException>(root,
                () => Response(HttpStatusCode.OK, new byte[16], "image/png"));
            CheckDownloadFailure<InvalidDataException>(root, () =>
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new UnknownLengthContent(new byte[5 * 1024 * 1024 + 1])
                };
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
                return response;
            });
        }

        static void CheckDownloadFailure<T>(string root, Func<HttpResponseMessage> create) where T : Exception
        {
            using (var handler = new FakeHandler(_ => create()))
            using (var client = new HttpClient(handler))
            {
                Throws<T>(() =>
                    new TrustedResourceResolver(root, SampleImageFolder, client).Resolve(SampleImageUrl));
            }
        }

        static HttpResponseMessage Response(HttpStatusCode status, byte[] bytes, string mediaType)
        {
            var response = new HttpResponseMessage(status) { Content = new ByteArrayContent(bytes) };
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mediaType);
            return response;
        }

        static void CheckReporting(string root)
        {
            var output = AppContext.BaseDirectory;
#if NETFRAMEWORK
            var configurationPath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            var original = File.Exists(configurationPath) ? File.ReadAllText(configurationPath) : null;
            var config = XDocument.Load(Path.Combine(output, "Examples", "app.config"));
            config.Descendants("parameter").Single(p => (string)p.Attribute("name") == "directory")
                .SetAttributeValue("value", root);
#else
            var configurationPath = Path.Combine(output, "appsettings.json");
            var original = File.Exists(configurationPath) ? File.ReadAllText(configurationPath) : null;
#endif
            try
            {
#if NETFRAMEWORK
                config.Save(configurationPath);
                System.Configuration.ConfigurationManager.RefreshSection("Telerik.Reporting");
#else
                var json = File.ReadAllText(Path.Combine(output, "Examples", "appsettings.json"));
                File.WriteAllText(configurationPath, json.Replace(@"C:\\ReportingResources", root.Replace(@"\", @"\\")));
#endif
                var processor = new ReportProcessor();
                using (var diagnostics = new StringWriter())
                using (var listener = new TextWriterTraceListener(diagnostics))
                {
                    Trace.Listeners.Add(listener);
                    try
                    {
                        var allowed = RenderImage(processor, "image.png");
                        listener.Flush();
                        Assert(!allowed.HasErrors, "Configured resolver did not resolve the relative image.");
                        Assert(diagnostics.ToString().Contains("The resource resolver approved a local resource."),
                            "Approved image did not pass through the configured resolver.");
                        RenderImage(processor, @"..\outside.png");
                        listener.Flush();
                        Assert(diagnostics.ToString().Contains("The resource resolver denied a resource outside its policy."),
                            "Configured resolver did not validate the outside image.");
                    }
                    finally
                    {
                        Trace.Listeners.Remove(listener);
                    }
                }
                Console.WriteLine("Reporting configuration and image rendering checked.");
            }
            finally
            {
                if (original == null)
                {
                    File.Delete(configurationPath);
                }
                else
                {
                    File.WriteAllText(configurationPath, original);
                }
            }
        }

        static RenderingResult RenderImage(ReportProcessor processor, string value)
        {
            using (var report = new Report())
            {
                report.Width = Unit.Inch(1);
                var detail = new DetailSection { Height = Unit.Inch(1) };
                detail.Items.Add(new PictureBox
                {
                    Value = value,
                    Size = new SizeU(Unit.Inch(1), Unit.Inch(1))
                });
                report.Items.Add(detail);
                return processor.RenderReport("IMAGE", new InstanceReportSource { ReportDocument = report },
                    new Hashtable { { "OutputFormat", "PNG" } });
            }
        }

        static void Assert(bool condition, string message)
        {
            if (!condition)
            {
                throw new InvalidOperationException(message);
            }
        }

        static void Throws<T>(Action action) where T : Exception
        {
            try
            {
                action();
            }
            catch (T)
            {
                return;
            }
            throw new InvalidOperationException("Expected " + typeof(T).Name + ".");
        }

        sealed class FakeHandler : HttpMessageHandler
        {
            readonly Func<HttpRequestMessage, HttpResponseMessage> respond;
            readonly string expectedUrl;
            public int Calls { get; private set; }

            public FakeHandler(Func<HttpRequestMessage, HttpResponseMessage> respond, string expectedUrl = SampleImageUrl)
            {
                this.respond = respond;
                this.expectedUrl = expectedUrl;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                this.Calls++;
                Assert(request.RequestUri.AbsoluteUri == this.expectedUrl,
                    "Downloader requested an unapproved URI.");
                return Task.FromResult(this.respond(request));
            }
        }

        sealed class UnknownLengthContent : HttpContent
        {
            readonly byte[] bytes;

            public UnknownLengthContent(byte[] bytes)
            {
                this.bytes = bytes;
            }

            protected override bool TryComputeLength(out long length)
            {
                length = 0;
                return false;
            }

            protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
            {
                return stream.WriteAsync(this.bytes, 0, this.bytes.Length);
            }
        }
    }
}
