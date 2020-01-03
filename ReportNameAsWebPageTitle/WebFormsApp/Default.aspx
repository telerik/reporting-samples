<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebFormsApp.Default" %>

<%@ Register TagPrefix="telerik" Assembly="Telerik.ReportViewer.Html5.WebForms" Namespace="Telerik.ReportViewer.Html5.WebForms" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Telerik HTML5 Web Forms Report Viewer Form</title>
	<script src="http://code.jquery.com/jquery-1.9.1.min.js"></script>

	<style>
		#reportViewer1 {
			position: absolute;
			left: 5px;
			right: 5px;
			top: 5px;
			bottom: 5px;
			overflow: hidden;
			font-family: Verdana, Arial;
		}
	</style>
    <script>
        function OnRenderingEnd(e, args) {
            document.title = args.bookmarkNodes[0].text;
        }
    </script>
</head>
<body>
    <form runat="server">
        <telerik:ReportViewer
            ID="reportViewer1" 
			Width="1300px"
			Height="900px"
			EnableAccessibility="false"
            runat="server"
            DocumentMapVisible="false"
            ClientEvents-RenderingEnd="OnRenderingEnd">
            <ReportSource IdentifierType="UriReportSource" Identifier="SampleReport.trdp">
            </ReportSource>
        </telerik:ReportViewer>
    </form>
</body>
</html>