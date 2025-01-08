Imports System.IO
Imports Telerik.Reporting.Services
Imports Telerik.WebReportDesigner.Services
Imports Telerik.WebReportDesigner.Services.Controllers

'The class name determines the service URL.
Public Class ReportDesignerController
    Inherits ReportDesignerControllerBase

    Shared ReadOnly configurationInstance As ReportServiceConfiguration
    Shared ReadOnly designerConfigurationInstance As ReportDesignerServiceConfiguration

    Shared Sub New()
        'This is the folder that contains the report definitions
        'In this case this is the Reports folder
        Dim appPath = HttpContext.Current.Server.MapPath("~/")
        Dim reportsPath = Path.Combine(appPath, "Reports")
        'Add report source resolver for trdx/trdp report definitions,
        'then add resolver for class report definitions as fallback resolver;
        'finally create the resolver And use it in the ReportServiceConfiguration instance.
        Dim resolver = New UriReportSourceResolver(reportsPath).AddFallbackResolver(New TypeReportSourceResolver())

        Dim reportServiceConfiguration As New ReportServiceConfiguration()
        reportServiceConfiguration.HostAppId = "Html5App"
        reportServiceConfiguration.ReportSourceResolver = resolver
        reportServiceConfiguration.Storage = New Telerik.Reporting.Cache.File.FileStorage()
        configurationInstance = reportServiceConfiguration

        Dim designerServiceConfiguration As New ReportDesignerServiceConfiguration()
        designerServiceConfiguration.DefinitionStorage = New FileDefinitionStorage(reportsPath)
        designerServiceConfiguration.SettingsStorage = New FileSettingsStorage(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Telerik Reporting"))
        designerConfigurationInstance = designerServiceConfiguration
    End Sub

    Public Sub New()
        'nitialize the service configuration
        Me.ReportServiceConfiguration = configurationInstance
        Me.ReportDesignerServiceConfiguration = designerConfigurationInstance

    End Sub
End Class
