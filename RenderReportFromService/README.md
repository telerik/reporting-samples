# RenderReportFromService Project

## Project Details

- Project Type -  .NET 7 Console Application
- Telerik Reporting Packages - `DEV`

The project can also be executed with `TRIAL` packages after adding a `.TRIAL` suffix at the end of the Telerik Reporting packages referenced in the project and restoring them.

For details on how to use the Telerik feed to restore the packages, see [Adding the Telerik Private NuGet Feed to Visual Studio](https://docs.telerik.com/reporting/getting-started/installation/adding-private-nuget-feed)

## How to run

The project requires a running [Reporting REST Service](https://docs.telerik.com/reporting/embedding-reports/host-the-report-engine-remotely/overview) whose `api/reports` endpoint may be used as a base URL to the `ReportClient` class.

The Example demos shipped with the installation of Telerik Reporting can be used as the service. With a default installation, the demo projects can be found at:

`C:\Program Files (x86)\Progress\Telerik Reporting <Release>\Examples`

## Exported Location

Exported documents will be saved in the `C:\temp` directory.
