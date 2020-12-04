namespace CSharp.AspNetCoreDemo
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;

    public class Program
    {
        public static void Main(string[] args)
        {
            EnableTracing();
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)

                // .UseWebRoot("path-to-the-wwwroot-directory") - uncomment this line when running on Linux to provide correct WebRoot path.
                .UseStartup<Startup>()
                .Build();

        /// <summary>
        /// Uncomment the lines to enable tracing in the current application.
        /// The trace log will be persisted in a file named log.txt in the application root directory.
        /// </summary>
        static void EnableTracing()
        {
            // System.Diagnostics.Trace.Listeners.Add(new System.Diagnostics.TextWriterTraceListener(File.CreateText("log.txt")));
            // System.Diagnostics.Trace.AutoFlush = true;
        }
    }
}
