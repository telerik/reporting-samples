# Telerik Reporting Blazor Report Viewer and Designer Example

## Overview

This sample project serves to demonstrate how the [native Blazor Report viewer](https://docs.telerik.com/reporting/embedding-reports/display-reports-in-applications/web-application/native-blazor-report-viewer/overview) can be integrated alongside the [Web Report Designer](https://docs.telerik.com/reporting/designing-reports/report-designer-tools/web-report-designer/overview) in a Blazor project created using the ASP.NET Core Hosted project template.

The `WeatherReport.trdp` report in the project uses the [WebServiceDataSource](https://docs.telerik.com/reporting/designing-reports/connecting-to-data/data-source-components/webservicedatasource-component/overview) component in order to connect to a REST API for its data. In this case, the data is actually server from the local service though the `/WeatherForecast` endpoint which requires the solution to be running in order to preview the report. Even though the service does not require it, a JWT is also passed through the `WebServiceDataSource` component via an `Authorization` request header, as an example of how to pass additional request headers.

## Elements

- [Telerik Reporting R1 SP1 2023](https://www.telerik.com/support/whats-new/reporting/release-history/progress-telerik-reporting-r1-2023-sp1-17-0-23-315)
- ASP.Net Core Hosted 7 Blazor Client / Server Application
- Shows the `WeatherReport.trdp`report in the [native Blazor Report viewer](https://docs.telerik.com/reporting/embedding-reports/display-reports-in-applications/web-application/native-blazor-report-viewer/overview)
- Edits the `WeatherReport.trdp` report in the [Web Report Designer](https://docs.telerik.com/reporting/designing-reports/report-designer-tools/web-report-designer/overview)

## Trial / Dev NuGet Packages

This sample solution is setup for the Telerik Reporting `Trial` version.</br>
To make this sample work for the licensed Telerik Reporting, remove the `.Trial` suffix from the package, script, stylesheet, etc. references.

Additionally, please make sure that you have set up the private Telerik feed before attempting to run the project - [Adding the Telerik Private NuGet Feed to Visual Studio](https://docs.telerik.com/reporting/getting-started/installation/adding-private-nuget-feed)
