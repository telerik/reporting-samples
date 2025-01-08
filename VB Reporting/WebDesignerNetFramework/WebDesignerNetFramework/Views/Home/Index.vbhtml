@Imports Telerik.Reporting
@Code
    Layout = Nothing
End Code
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Telerik Web Report Designer Demo</title>
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <link href="https://fonts.googleapis.com/css?family=Roboto:400,500&display=swap" rel="stylesheet">
</head>
<body>
    <div id="webReportDesigner">
        loading...
    </div>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <script src="https://kendo.cdn.telerik.com/2022.3.913//js/kendo.all.min.js"></script>
    <script src="/api/reportdesigner/resources/js/telerikReportViewer"></script>
    <script src="/api/reportdesigner/designerresources/js/webReportDesigner"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#webReportDesigner").telerik_WebReportDesigner({
                persistSession: false,
                toolboxArea: {
                    layout: "list"
                },
                serviceUrl: "/api/reportdesigner/",
                report: "SampleReport.trdp"
            }).data("telerik_WebDesigner");
        });
    </script>
</body>
</html>