# Blazor DataGrid Meets Blazor Report Viewer
### Hi there 👋.



This project is based on the [Blazor DataGrid Meets Blazor Report Viewer](https://www.telerik.com/blogs/blazor-datagrid-meets-blazor-report-viewer) blog post and it demonstrates
the integration between [Telerik Reporting](https://www.telerik.com/products/reporting.aspx) and [Telerik UI for Blazor](https://www.telerik.com/blazor-ui).
The project is also used in the [Telerik UI for Blazor live demos](https://demos.telerik.com/blazor-ui/reporting-integration/grid-report).




### Instructions

This project uses Dev (licensed) packages, if you want to use the Trial ones, you need to make the following changes:
- BlazorGridAndReport.csproj - add ".Trial" to the Telerik NuGet packages;
- _Host.cshtml - Add ".Trial" to the Telerik references in the file, example:
 <script src="_content/Telerik.UI.for.Blazor.Trial/js/telerik-blazor.js" defer></script>
 <link rel="stylesheet" href="_content/Telerik.UI.for.Blazor.Trial/css/kendo-theme-default/all.css" />
 <script src="_content/Telerik.ReportViewer.Blazor.Trial/interop.js" defer></script>
