namespace ReportNameAsWebPageTitle
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.UseStaticFiles();
            app.UseRouting();

            //app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}
