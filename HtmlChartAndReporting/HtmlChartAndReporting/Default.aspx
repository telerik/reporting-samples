<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Default" %>

<%@ Register TagPrefix="telerik" Assembly="Telerik.ReportViewer.Html5.WebForms" Namespace="Telerik.ReportViewer.Html5.WebForms" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <link rel="stylesheet" href="https://kendo.cdn.telerik.com/2019.3.1023/styles/kendo.default-v2.min.css" />

    <script src="https://kendo.cdn.telerik.com/2022.3.913/js/jquery.min.js" type="text/javascript"></script>
    <script src="https://kendo.cdn.telerik.com/2022.3.913/js/kendo.all.min.js" type="text/javascript"></script>

    <style>
        #reportViewer1 {
            position: absolute;
            left: 5px;
            right: 5px;
            top: 5px;
            bottom: 5px;
            padding-top: 400px;
            overflow: hidden;
            font-family: Verdana, Arial;
        }
    </style>
    <title></title>

    <telerik:RadStyleSheetManager ID="RadStyleSheetManager1" runat="server" />
</head>
<body>
    <form id="form1" runat="server">

        <telerik:RadScriptManager ID="RadScriptManager1" runat="server" EnableEmbeddedjQuery="false">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryExternal.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryPlugins.js" />
            </Scripts>
        </telerik:RadScriptManager>

        <telerik:RadHtmlChart ID="RadHtmlChart1" runat="server">
            <PlotArea>
                <Series>
                    <telerik:ColumnSeries DataFieldY="valueField"></telerik:ColumnSeries>
                </Series>
                <XAxis DataLabelsField="labelsField"></XAxis>
            </PlotArea>
        </telerik:RadHtmlChart>
        <telerik:ReportViewer
            ID="reportViewer1"
            Width="1300px"
            Height="900px"
            EnableAccessibility="false"
            runat="server">
            <ReportSource IdentifierType="UriReportSource" Identifier="SampleReport.trdp">
            </ReportSource>
            <%-- If set to true shows the Send Mail Message toolbar button --%>
            <SendEmail Enabled="false" />
        </telerik:ReportViewer>
    </form>
</body>
</html>
