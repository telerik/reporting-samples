# Blazor DataGrid Meets Blazor Report Viewer

### Hi there 👋.

This project is based on the [Blazor DataGrid Meets Blazor Report Viewer](https://www.telerik.com/blogs/blazor-datagrid-meets-blazor-report-viewer) blog post, and it demonstrates the integration between [Telerik Reporting](https://www.telerik.com/products/reporting.aspx) and [Telerik UI for Blazor](https://www.telerik.com/blazor-ui).
The project is also used in the [Telerik UI for Blazor live demos](https://demos.telerik.com/blazor-ui/reporting-integration/grid-report).

### Instructions

This sample project demonstrates an integration between the **Telerik UI for Blazor** and **Telerik Reporting** products. 

Both the Blazor Grid and the Report Viewer are loaded into different TabStrips. The grid is the first tab open, and data is passed to the Grid and can be interacted with by applying filters, sorting, and grouping, which are stored in the state of the Grid. 

This state is being read and converted to JSON together with the data, which the Report Viewer uses to request a rendered report from the Report REST Service. The grid data is sent to the report as a JSON string via a report parameter that is handled on the server by a custom [IReportSourceResolver](https://www.telerik.com/products/reporting/documentation/api/telerik.reporting.services.ireportsourceresolver).


