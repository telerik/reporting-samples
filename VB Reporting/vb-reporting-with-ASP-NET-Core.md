---
title: VB Reporting
page_title: VB.NET – Telerik Reporting
description: "VB Reporting or VB.NET Reporting is generating and delivering dynamic reports through the Microsoft Visual Basic programming language."
slug: vb-reporting
tags: telerik, reporting, reports, vb, net
published: True
position: 99
---

# Developing VB.NET Reporting Applications

Microsoft provides different programming language for its most popular `.NET` and `.NET Framework` environments. The Visual Basic (VB or VB.NET) is an advanced programming language fully supported by Telerik Reporting.
This article elaborates on developing Telerik Reporting [report definitions]({%slug on-telerik-reporting%}#report-definition) with VB, and integrating Telerik Reporting into VB Desktop and Web Reporting applications.

## Developing VB.NET Report Definitions

The VB Type report definitions are VB Types inheriting the Telerik Reporting base [Report](/api/telerik.reporting.report) type.

The easiest and recommended approach for creating such a report in the .NET Framework is with the [Visual Studio Report Designer]({%slug telerikreporting/designing-reports/report-designer-tools/desktop-designers/visual-studio-report-designer/overview%}). The designer offers also a converter from declarative (TRDP and TRDX files) to VB type report definitions as explained in the article [Importing Reports Created with the Standalone or Web Report Designer]({%slug telerikreporting/designing-reports/report-designer-tools/desktop-designers/visual-studio-report-designer/how-to-import-reports-created-with-standalone-report-designer%}).

VB reports are supported also in .NET. The Visual Studio Report Designer is not yet available for .NET and the recommended approach for developing such reports is described in the KB article [How to use Visual Studio Report Designer to edit CS Reports in .NET Projects]({%slug how-to-use-vs-designer-in-dotnet-core%}).

## VB.NET Desktop Reporting Applications

### Displaying Reports in VB Desktop Reporting Applications

The Telerik Reporting product provides the following Report Viewers:

* [Windows Forms Report Viewer]({%slug telerikreporting/using-reports-in-applications/display-reports-in-applications/windows-forms-application/overview%})
* [WPF Report Viewer]({%slug telerikreporting/using-reports-in-applications/display-reports-in-applications/wpf-application/overview%})
* [WinUI Report Viewer]({%slug telerikreporting/using-reports-in-applications/display-reports-in-applications/winui-3-desktop-application/overview%})

#### Implementation

The Desktop Report Viewers may be embedded in .NET Framework and .NET Visual Basic projects. The steps for the corresponding viewers and environments are described in the following sections:

##### Implementing in VB projects targeting .NET Framework 4.6.2 and above

* [Adding the Windows Forms Report Viewer to a Windows Forms .NET Framework Project]({%slug telerikreporting/using-reports-in-applications/display-reports-in-applications/windows-forms-application/how-to-add-report-viewer-to-a-windows-forms'-.net-framework-project%})
* [Integrating the WPF Report Viewer in .NET Framework]({%slug telerikreporting/using-reports-in-applications/display-reports-in-applications/wpf-application/how-to-add-report-viewer-to-a-wpf-.net-framework-project%})

##### Implementing in VB projects targeting .NET 6.0 and above

The following Report Viewers may be configured manually in VB desktop projects. There are no Visual Studio Reporting templates available for Visual Basic projects targeting .NET:

* [Adding the Windows Forms Report Viewer Control to a Windows Forms .NET Project]({%slug telerikreporting/using-reports-in-applications/display-reports-in-applications/windows-forms-application/how-to-add-report-viewer-to-a-windows-forms'-.net-core-project%})
* [Integrating the WPF Report Viewer in .NET]({%slug telerikreporting/using-reports-in-applications/display-reports-in-applications/wpf-application/how-to-add-report-viewer-to-a-wpf-.net-core-project%})
* [Integrating the WinUI Report Viewer in .NET 6]({%slug telerikreporting/using-reports-in-applications/display-reports-in-applications/winui-3-desktop-application/how-to-add-report-viewer-to-a-.net-5-application%})

### Generating Reports in VB Desktop Reporting Applications

The VB Desktop Report Viewers may use an Embedded Reporting Engine, a [Telerik Reporting REST Services]({%slug telerikreporting/using-reports-in-applications/host-the-report-engine-remotely/telerik-reporting-rest-services/overview%}), or a [Telerik Report Server](https://docs.telerik.com/report-server/introduction). The [Connection String to Report Engine]({%slug telerikreporting/using-reports-in-applications/display-reports-in-applications/how-to-construct-a-string-to-connect-to-report-engine%}) determines the engine type used by the viewer.

The Embedded Reporting Engine is integrated within the viewer of the VB Reporting project.

The Reporting REST Service provides an Application Programming Interface (API) over HTTP to the Reporting Engine that processes and renders [Telerik Reports]({%slug on-telerik-reporting%}#report-definition). The fully functional implementation of the service is available in the `Telerik.Reporting.Services.WebApi` (for the .NET Framework) and `Telerik.Reporting.Services.AspNetCore` (for .NET) assembly/package. It may be embedded in any VB.NET Web Reporting Application and exposed in a custom controller (for example, `ReportsController`) inheriting the basic `ReportsControllerBase` class. The `ReportsController` may be used as a source of Report documents by VB Web Reporting, VB Desktop Reporting, and other third-party client applications.

The Report Server is a separate application exposing a [Web API](https://docs.telerik.com/report-server/implementer-guide/apis/rest-api/v2/api-reference) that may be used by the viewers in VB.NET Reporting applications to request reports.

## VB.NET Web Reporting Applications in .NET Framework

### Designing Reports in .NET Framework VB Web Reporting Applications

The [Web Report Designer]({%slug telerikreporting/designing-reports/report-designer-tools/web-report-designer/overview%}) is an HTML/CSS/JS client-side jQuery widget that lets developers embed a report designer into their VB Web Reporting applications. It is ready for embedding in the VB ASP.NET Reporting Applications to allow the users to create, edit, and preview their Telerik Reports directly from the browser.

#### VB Web Report Designer Requirements in .NET Framework

The VB Web Report Designer requires a Report Designer REST service, whose fully functional implementation resides in the [Telerik.WebReportDesigner.Services](/api/telerik.webreportdesigner.services) assembly/package as explained in the section [Web Report Designer Implementation](#web-report-designer-implementation).

#### VB Web Report Designer Implementation in .NET Framework

The tutorial [Configuring the Web Report Designer in ASP.NET Framework application]({%slug telerikreporting/designing-reports/report-designer-tools/web-report-designer/how-to-set-up-in-.net-framework-application%}) elaborates on implementing the web designer jQuery widget and its REST Service in ASP.NET Web Reporting projects.

A sample project may be found in the Telerik GitHub Reporting Samples repository - [VB Reporting](https://github.com/telerik/reporting-samples). The solution/project name is _WebDesignerNetFramework_.

### Generating Reports in VB Web Reporting Applications in .NET Framework

The [Telerik Reporting REST Services]({%slug telerikreporting/using-reports-in-applications/host-the-report-engine-remotely/telerik-reporting-rest-services/overview%}) provide an Application Programming Interface (API) over HTTP to the Reporting Engine that processes and renders [Telerik Reports]({%slug on-telerik-reporting%}#report-definition). The fully functional implementation of the service is available in the [Telerik.Reporting.Services.WebApi](/api/telerik.reporting.services.webapi) assembly/package. It may be embedded in any VB ASP.NET Reporting Application and exposed in a custom controller (for example, `ReportsController`) inheriting the basic `ReportsControllerBase` class. The `ReportsController` may be used as a source of Report documents by VB Web Reporting, VB Desktop Reporting, and other third-party client applications.

#### Implementation in .NET Framework

The article [Telerik Reporting REST Service ASP.NET Web API Implementation]({%slug telerikreporting/using-reports-in-applications/host-the-report-engine-remotely/telerik-reporting-rest-services/asp.net-web-api-implementation/overview%}) elaborates on the requirements, implementation, and hosting of the REST Service.

### Displaying Reports in .NET Framework VB Web Reporting Applications

The [HTML5 Report Viewer]({%slug telerikreporting/using-reports-in-applications/display-reports-in-applications/web-application/html5-report-viewer/overview%}) is an HTML/CSS/JS client-side widget suitable for any web application that works with JavaScript. This makes it the perfect widget for displaying Telerik Reporting Reports in all VB Web Reporting Applications.

Telerik Reporting provides also dedicated wrappers of the pure HTML5 Report Viewer for the most popular ASP.NET MVC and Web Forms frameworks:

* [HTML5 ASP.NET MVC Report Viewer]({%slug telerikreporting/using-reports-in-applications/display-reports-in-applications/web-application/html5-asp.net-mvc-report-viewer/overview%})
* [HTML5 ASP.NET Web Forms Report Viewer]({%slug telerikreporting/using-reports-in-applications/display-reports-in-applications/web-application/html5-asp.net-web-forms-report-viewer/overview%})

#### Implementation in .NET Framework

The article [Using the HTML5 Report Viewer with REST Service]({%slug telerikreporting/using-reports-in-applications/display-reports-in-applications/web-application/html5-report-viewer/how-to-use-html5-report-viewer-with-rest-service%}) is a step-by-step tutorial on embedding the HTML5 Report Viewer in ASP.NET Web Reporting projects.

The steps for integrating the viewers in ASP.NET MVC and Web Forms projects are as follows:

* [Using HTML5 ASP.NET MVC Report Viewer With Reporting REST Service]({%slug telerikreporting/using-reports-in-applications/display-reports-in-applications/web-application/html5-asp.net-mvc-report-viewer/how-to-use-html5-asp.net-mvc-report-viewer-with-rest-service%})
* [Using the HTML5 ASP.NET Web Forms Report Viewer with REST Service]({%slug telerikreporting/using-reports-in-applications/display-reports-in-applications/web-application/html5-asp.net-web-forms-report-viewer/how-to-use-html5-asp.net-web-forms-report-viewer-with-rest-service%})

## VB.NET Web Reporting Applications in .NET 6.0 and above

As a pure HTML/CSS/JS client-side jQuery widget the [Web Report Designer]({%slug telerikreporting/designing-reports/report-designer-tools/web-report-designer/overview%}) can be used in any web reporting application. It is ready for embedding in the VB ASP.NET Core Reporting Applications.

### Web Report Designer Requirements in .NET 6.0+

The Web Report Designer requires a Report Designer REST service, whose fully functional implementation resides in the `Telerik.WebReportDesigner.Services` assembly/package as explained in the section [Web Report Designer Implementation](#web-report-designer-implementation).

### Web Report Designer Implementation in .NET 6.0+

The tutorial [Setting up the Web Report Designer in .NET applications]({%slug telerikreporting/designing-reports/report-designer-tools/web-report-designer/how-to-set-up-in-.net-5-and-.net-core-3.1-applications%}) elaborates on implementing the web designer jQuery widget and its REST Service in ASP.NET Core Web Reporting projects.

!!! NO VB CODE in the referenced article !!!

## Generating Reports in .NET 6.0+ VB Web Reporting Applications

The [Telerik Reporting REST Services]({%slug telerikreporting/using-reports-in-applications/host-the-report-engine-remotely/telerik-reporting-rest-services/overview%}) provide an Application Programming Interface (API) over HTTP to the Reporting Engine that processes and renders [Telerik Reports]({%slug on-telerik-reporting%}#report-definition). The fully functional implementation of the service is available in the `Telerik.Reporting.Services.AspNetCore` assembly/package. It may be embedded in any VB ASP.NET Core Reporting Application and exposed in a custom controller (for example, `ReportsController`) inheriting the basic `ReportsControllerBase` class. The `ReportsController` may be used as a source of Report documents by VB Web Reporting, VB Desktop Reporting, and other third-party client applications.

### Implementation in .NET 6.0+

There are different methods for configuring the Telerik Reporting REST Service in an ASP.NET Core Reporting project:

* [Hosting the Telerik Reporting REST Service in ASP.NET Core in .NET 6+ with Minimal API]({%slug telerikreporting/using-reports-in-applications/host-the-report-engine-remotely/telerik-reporting-rest-services/asp.net-core-web-api-implementation/how-to-host-reports-service-in-asp.net-core-in-.net-6-with-minimal-api%})
* [Hosting the Telerik Reporting REST Service in ASP.NET Core in .NET 6 and .NET 8 with Top-Level Statements]({%slug telerikreporting/using-reports-in-applications/host-the-report-engine-remotely/telerik-reporting-rest-services/asp.net-core-web-api-implementation/host-reports-service-in-.net-with-controllers%})
* [Hosting the Telerik Reporting REST Service in an ASP.NET Core Application in .NET 6, .NET 8, and .NET 9 with Startup.cs]({%slug telerikreporting/using-reports-in-applications/host-the-report-engine-remotely/telerik-reporting-rest-services/asp.net-core-web-api-implementation/how-to-host-reports-service-in-asp.net-core-in-.net-5%})

!!! NO VB CODE in the referenced articles !!!

## Displaying Reports in .NET 6.0+ VB Web Reporting Applications

The [HTML5 Report Viewer]({%slug telerikreporting/using-reports-in-applications/display-reports-in-applications/web-application/html5-report-viewer/overview%}) is an HTML/CSS/JS client-side widget suitable for any web application that works with JavaScript. This makes it the perfect widget for displaying Telerik Reporting Reports in all VB Web Reporting Applications.

### Requirements in .NET 6.0+

.NET 6 introduced a [globalization-invariant mode](https://learn.microsoft.com/en-us/dotnet/core/runtime-config/globalization) which, if enabled, does not allow the creation of any culture except the invariant culture. The Report Engine relies on instantiating cultures to support localization, formatting, and unit conversion, so enabling invariant globalization will break the engine's functionality. Refer to the [Only invariant culture is supported in globalization-invariant mode]({%slug only-invariant-culture-is-supported-in-globalization-invariant-mode-error%}) article for instructions on how to disable the `Invariant Globalization` mode.

### Implementation in .NET 6.0+

The article [Using the HTML5 Report Viewer in an ASP.NET Core application in .NET]({%slug telerikreporting/using-reports-in-applications/display-reports-in-applications/web-application/html5-report-viewer/how-to-use-html5-report-viewer-in-an-asp.net-core-in-.net-5-application%}) is a step-by-step tutorial on embedding the HTML5 Report Viewer in ASP.NET Core Web Reporting projects.

!!! NO VB CODE in the referenced article !!!

## See Also

* [Report Definitions]({%slug on-telerik-reporting%}#report-definition)
* [Displaying Reports]({%slug telerikreporting/using-reports-in-applications/display-reports-in-applications/overview%})
* [Report Sources]({%slug telerikreporting/designing-reports/report-sources/overview%})
* [Telerik Report Viewer and Telerik Reporting REST Service]({%slug telerikreporting/using-reports-in-applications/export-and-configure/cache-management/html5-report-viewer-and-reporting-rest-services%})
