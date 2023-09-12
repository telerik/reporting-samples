The project demonstrates how to display PDF document rendered with Telerik Reporting in Telerik Blazor UI PDF Viewer in MAUI Blazor application.

The `ReportingService` defines the Reporting service that delivers the PDF document to the PDF Viewer:
* `ReportService.ReportingOnlineDemo` utilizes the [Telerik Reporting online demo](https://demos.telerik.com/reporting)
* `ReportService.ReportingWebApi` utilizes the custom Web API service from the project `ReportingWebApi`. The Reporting code for rendering the reports is hosted in the project `RenderReports`. These two projects are needed only when you select this option for the `ReportingService`.
