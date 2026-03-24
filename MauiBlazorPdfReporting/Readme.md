The project demonstrates how to display a PDF document rendered with Telerik Reporting in the Telerik Blazor UI PDF Viewer in a MAUI Blazor application.

The `ReportingService` property in the page with the PDF Viewer (`Index.razor` in the `MauiBlazorViewer` project) defines the Reporting service that delivers the PDF document to the PDF Viewer:
* `ReportService.ReportingOnlineDemo` utilizes the [Telerik Reporting online demo](https://demos.telerik.com/reporting). When you select this option, you don't need the projects _ReportingWebApi_ and _RenderReports_.
* `ReportService.ReportingWebApi` utilizes the custom Web API service from the project __ReportingWebApi__. The Reporting code for rendering the reports is hosted in the project __RenderReports__. These two projects are needed only when you select this option for the `ReportingService`.
