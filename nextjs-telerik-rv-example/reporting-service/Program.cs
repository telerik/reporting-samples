using Telerik.Reporting.Services.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var reportsPath = Path.Combine( builder.Environment.ContentRootPath, "Reports");

builder.Services.AddCors(options =>
{
    options.AddPolicy("report-viewers", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers().AddTelerikReporting("ReportingNet", reportsPath);

var app = builder.Build();

app.UseCors("report-viewers");
app.UseTelerikReporting();
app.UseRouting();
app.Run();


