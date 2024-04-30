using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System;
using Telerik.Reporting.Data.Schema;
using Telerik.Reporting.Services.AspNetCore;
using Telerik.Reporting.Services;
using Telerik.WebReportDesigner.Services.Controllers;
using Telerik.WebReportDesigner.Services.Models;
using Telerik.WebReportDesigner.Services;

[ServiceFilter(typeof(ToNewtonsoftActionFilter))]
public class ReportDesignerControllerSTJBase : ReportDesignerControllerBase {
	private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor;

	public ReportDesignerControllerSTJBase(IReportDesignerServiceConfiguration reportDesignerServiceConfiguration, IReportServiceConfiguration reportServiceConfiguration) : base(reportDesignerServiceConfiguration, reportServiceConfiguration) { }
	[HttpGet("typeSchema/{typeName}", Order = -1)]
	public virtual new IActionResult GetTypeSchema(string? typeName) => base.GetTypeSchema(typeName);

	[HttpPost("typeSchemaCollection", Order = -1)]
	public virtual new IActionResult GetTypeSchemas([FromBody] string[] typeNames) => base.GetTypeSchemas(typeNames);

	[HttpGet("defaultPropertyStore/{typeName}", Order = -1)]
	public virtual new IActionResult GetDefaultComponent(string? typeName) => base.GetDefaultComponent(typeName);

	[HttpGet("fonts", Order = -1)]
	public virtual new IActionResult ListFonts() => base.ListFonts();

	[HttpGet("paperSizes", Order = -1)]
	public virtual new IActionResult PaperSizes() => base.PaperSizes();

	[HttpGet("cultureContext", Order = -1)]
	public virtual new IActionResult CultureContext() => base.CultureContext();

	[HttpGet("expressionBuilderHierarchy", Order = -1)]
	public virtual new IActionResult GetExpressionBuilderHierarchy() => base.GetExpressionBuilderHierarchy();

	[HttpPost("expressionBuilderParametersHierarchy", Order = -1)]
	public virtual new IActionResult GetExpressionBuilderParametersHierarchy([ModelBinder(typeof(NewtonsoftJsonModelBinder))] object rawParameters) => base.GetExpressionBuilderParametersHierarchy(rawParameters);

	[HttpGet("wizardStyleSheets/{typeName}", Order = -1)]
	public virtual new IActionResult GetWizardStyleSheets(string? typeName) => base.GetWizardStyleSheets(typeName);

	[HttpGet("designerresources/{folder}/{resourceName}", Order = -1)]
	[AllowAnonymous]
	public virtual new IActionResult GetDesignerResource(string? folder, string? resourceName) => base.GetDesignerResource(folder, resourceName);

	[HttpPost("evaluate", Order = -1)]
	public virtual new Task<IActionResult> Evaluate() => base.Evaluate();

	[HttpGet("data/ods/types", Order = -1)]
	public virtual new IActionResult GetTypes() => base.GetTypes();

	[HttpPost("data/ods/members", Order = -1)]
	[ValidateModelState]
	public virtual new IActionResult GetMembers([ModelBinder(typeof(NewtonsoftJsonModelBinder))] TypeInfoWithFilter input) => base.GetMembers(input);

	[HttpPost("data/ods/model", Order = -1)]
	[ValidateModelState]
	public virtual new IActionResult GetDataModel([ModelBinder(typeof(NewtonsoftJsonModelBinder))] ObjectDataSourceInfo input) => base.GetDataModel(input);

	[HttpPost("data/ods/measures", Order = -1)]
	[ValidateModelState]
	public virtual new IActionResult GetTypeMeasures([ModelBinder(typeof(NewtonsoftJsonModelBinder))] TypeInfo input) => base.GetTypeMeasures(input);

	[HttpPost("data/ods/preview", Order = -1)]
	public virtual new IActionResult Preview([ModelBinder(typeof(NewtonsoftJsonModelBinder))] DataSourceInfo dataSourceInfo) => base.Preview(dataSourceInfo);

	[HttpPost("render/graph", Order = -1)]
	public virtual new Task<IActionResult> RenderGraphAsync() => base.RenderGraphAsync();

	[HttpPost("render/map", Order = -1)]
	public virtual new Task<IActionResult> RenderMapAsync() => base.RenderMapAsync();

	[HttpPost("render/barcode", Order = -1)]
	public virtual new Task<IActionResult> RenderBarcodeAsync() => base.RenderBarcodeAsync();

	[HttpPost("render/htmltextbox", Order = -1)]
	public virtual new Task<IActionResult> RenderHtmlTextBoxAsync() => base.RenderHtmlTextBoxAsync();

	[HttpPost("render/shape", Order = -1)]
	public virtual new Task<IActionResult> RenderShapeAsync() => base.RenderShapeAsync();

	[HttpPost("render/crosssectionitem", Order = -1)]
	public virtual new Task<IActionResult> RenderCrossSectionItemAsync() => base.RenderCrossSectionItemAsync();

	[HttpPost("render/checkbox", Order = -1)]
	public virtual new Task<IActionResult> RenderCheckBoxAsync() => base.RenderCheckBoxAsync();

	[HttpPost("render/picturebox", Order = -1)]
	public virtual new Task<IActionResult> RenderPictureBoxAsync() => base.RenderPictureBoxAsync();

	[HttpGet("definitionresources/folder/contents", Order = -1)]
	public virtual new IActionResult GetFolderContents([FromQuery] string? uri) => base.GetFolderContents(uri);

	[HttpGet("definitionresources/folder/hasContents", Order = -1)]
	public virtual new IActionResult FolderHasContents([FromQuery] string? uri) => base.FolderHasContents(uri);

	[HttpGet("definitionresources/folder/model", Order = -1)]
	public virtual new IActionResult GetFolderModel([FromQuery] string? uri) => base.GetFolderModel(uri);

	[HttpGet("definitionresources/folder/modelbyname", Order = -1)]
	public virtual new IActionResult GetFolderModelByName([FromQuery] string? name) => base.GetFolderModelByName(name);

	[HttpGet("definitionresources/folder/nameexists", Order = -1)]
	public virtual new IActionResult FolderNameExists([FromQuery] string? name) => base.FolderNameExists(name);

	[HttpGet("definitionresources/folder/exists", Order = -1)]
	public virtual new IActionResult FolderExists([FromQuery] string? uri) => base.FolderExists(uri);

	[HttpPost("definitionresources/folder", Order = -1)]
	[ValidateModelState]
	public virtual new IActionResult CreateFolder([FromBody][ModelBinder(typeof(NewtonsoftJsonModelBinder))] CreateFolderModel model) => base.CreateFolder(model);

	[HttpDelete("definitionresources/folder/delete", Order = -1)]
	[ValidateModelState]
	public virtual new IActionResult DeleteFolder([FromQuery] string? uri) => base.DeleteFolder(uri);

	[HttpPut("definitionresources/folder/move", Order = -1)]
	[ValidateModelState]
	public virtual new IActionResult MoveFolder([FromBody][ModelBinder(typeof(NewtonsoftJsonModelBinder))] MoveFolderModel model) => base.MoveFolder(model);

	[HttpPut("definitionresources/folder/rename", Order = -1)]
	[ValidateModelState]
	public virtual new IActionResult RenameFolder([FromBody][ModelBinder(typeof(NewtonsoftJsonModelBinder))] RenameFolderModel model) => base.RenameFolder(model);

	[HttpGet("definitionresources", Order = -1)]
	public virtual new IActionResult GetAllByExtension([FromQuery] string? extension) => base.GetAllByExtension(extension);

	[HttpGet("definitionresources/{resourceName}", Order = -1)]
	[Obsolete]
	public virtual new IActionResult Get(string? resourceName) => base.Get(resourceName);

	[HttpGet("definitionresources/model", Order = -1)]
	[ValidateModelState]
	public virtual new IActionResult GetResourceModel([FromQuery] string? uri) => base.GetResourceModel(uri);

	[HttpGet("definitionresources/modelbyname", Order = -1)]
	[ValidateModelState]
	public virtual new IActionResult GetResourceModelByName([FromQuery] string? name) => base.GetResourceModelByName(name);

	[HttpGet("definitionresources/raw", Order = -1)]
	[ValidateModelState]
	public virtual new IActionResult GetRawResource([FromQuery] string? uri) => base.GetRawResource(uri);

	[HttpGet("definitionresources/rawbyname", Order = -1)]
	[ValidateModelState]
	public virtual new IActionResult GetRawResourceByName([FromQuery] string? name) => base.GetRawResourceByName(name);

	[HttpGet("definitionresources/getfile", Order = -1)]
	[ValidateModelState]
	public virtual new IActionResult GetResourceFile([FromQuery] string? uri) => base.GetResourceFile(uri);

	[HttpGet("definitionresources/nameexists", Order = -1)]
	[ValidateModelState]
	public virtual new IActionResult ResourceNameExists([FromQuery] string? name) => base.ResourceNameExists(name);

	[HttpGet("definitionresources/exists", Order = -1)]
	[ValidateModelState]
	public virtual new IActionResult ResourceExists([FromQuery] string? uri) => base.ResourceExists(uri);

	[HttpPost("definitionresources/{resourceName}", Order = -1)]
	[Obsolete]
	public virtual new Task<IActionResult> Save(string? resourceName) => base.Save(resourceName);

	[HttpPost("definitionresources/save", Order = -1)]
	[ValidateModelState]
	public virtual new Task<IActionResult> SaveResource([FromQuery][ModelBinder(typeof(NewtonsoftJsonModelBinder))] SaveResourceModel model) => base.SaveResource(model);

	[HttpPut("definitionresources/overwrite", Order = -1)]
	[ValidateModelState]
	public virtual new Task<IActionResult> OverwriteResource([FromQuery][ModelBinder(typeof(NewtonsoftJsonModelBinder))] OverwriteResourceModel model) => base.OverwriteResource(model);

	[HttpPut("definitionresources/rename", Order = -1)]
	[ValidateModelState]
	public virtual new IActionResult RenameResource([FromBody][ModelBinder(typeof(NewtonsoftJsonModelBinder))] RenameResourceModel model) => base.RenameResource(model);

	[HttpPut("definitionresources/move", Order = -1)]
	[ValidateModelState]
	public virtual new IActionResult MoveResource([FromBody][ModelBinder(typeof(NewtonsoftJsonModelBinder))] MoveResourceModel model) => base.MoveResource(model);

	[HttpDelete("definitionresources/delete", Order = -1)]
	[ValidateModelState]
	public virtual new IActionResult DeleteResource([FromQuery] string? uri) => base.DeleteResource(uri);

	[HttpGet("definitionresources/search", Order = -1)]
	[ValidateModelState]
	public virtual new IActionResult SearchResources([FromQuery][ModelBinder(typeof(NewtonsoftJsonModelBinder))] SearchResourcesModel model) => base.SearchResources(model);

	[HttpPost("definitionresources/preview/json", Order = -1)]
	[ValidateModelState]
	public virtual new IActionResult PreviewJson([ModelBinder(typeof(NewtonsoftJsonModelBinder))] DataSourceInfo dataSourceInfo) => base.PreviewJson(dataSourceInfo);

	[HttpPost("definitionresources/preview/csv", Order = -1)]
	[ValidateModelState]
	public virtual new IActionResult PreviewCsv([ModelBinder(typeof(NewtonsoftJsonModelBinder))] DataSourceInfo dataSourceInfo) => base.PreviewCsv(dataSourceInfo);

	[HttpPost("definitionresources/preview/webservice", Order = -1)]
	[ValidateModelState]
	public virtual new IActionResult PreviewWebServiceData([ModelBinder(typeof(NewtonsoftJsonModelBinder))] DataSourceInfo dataSourceInfo) => base.PreviewWebServiceData(dataSourceInfo);

	[HttpGet("datasources/folder/contents", Order = -1)]
	public virtual new IActionResult GetSharedDataSourceFolderContents([FromQuery] string? uri) => base.GetSharedDataSourceFolderContents(uri);

	[HttpPost("datasources/folder", Order = -1)]
	[ValidateModelState]
	public virtual new IActionResult CreateSharedDataSourceFolder([FromBody][ModelBinder(typeof(NewtonsoftJsonModelBinder))] CreateFolderModel model) => base.CreateSharedDataSourceFolder(model);

	[HttpPut("datasources/folder/rename", Order = -1)]
	[ValidateModelState]
	public virtual new IActionResult RenameSharedDataSourceFolder([FromBody][ModelBinder(typeof(NewtonsoftJsonModelBinder))] RenameFolderModel model) => base.RenameSharedDataSourceFolder(model);

	[HttpDelete("datasources/folder/delete", Order = -1)]
	[ValidateModelState]
	public virtual new IActionResult DeleteSharedDataSourceFolder([FromQuery] string? uri) => base.DeleteSharedDataSourceFolder(uri);

	[HttpGet("datasources/folder/nameexists", Order = -1)]
	public virtual new IActionResult SharedDataSourceFolderNameExists([FromQuery] string? name) => base.SharedDataSourceFolderNameExists(name);

	[HttpGet("datasources/folder/exists", Order = -1)]
	public virtual new IActionResult SharedDataSourceFolderExists([FromQuery] string? uri) => base.SharedDataSourceFolderExists(uri);

	[HttpGet("datasources/", Order = -1)]
	[ValidateModelState]
	public virtual new Task<IActionResult> GetSharedDataSource([FromQuery] string? uri) => base.GetSharedDataSource(uri);

	[HttpGet("datasources/model", Order = -1)]
	[ValidateModelState]
	public virtual new Task<IActionResult> GetSharedDataSourceModel([FromQuery] string? uri) => base.GetSharedDataSourceModel(uri);

	[HttpPost("datasources/save", Order = -1)]
	[ValidateModelState]
	public virtual new Task<IActionResult> SaveSharedDataSourceModel([FromQuery][ModelBinder(typeof(NewtonsoftJsonModelBinder))] SaveResourceModel model) => base.SaveSharedDataSourceModel(model);

	[HttpDelete("datasources/delete", Order = -1)]
	[ValidateModelState]
	public virtual new Task<IActionResult> DeleteSharedDataSource([FromQuery] string? uri) => base.DeleteSharedDataSource(uri);

	[HttpPut("datasources/rename", Order = -1)]
	[ValidateModelState]
	public virtual new Task<IActionResult> RenameSharedDataSource([FromBody][ModelBinder(typeof(NewtonsoftJsonModelBinder))] RenameResourceModel model) => base.RenameSharedDataSource(model);

	[HttpGet("datasources/raw", Order = -1)]
	[ValidateModelState]
	public virtual new Task<IActionResult> GetRawSharedDataSource([FromQuery] string? uri) => base.GetRawSharedDataSource(uri);

	[HttpPost("datasources/raw", Order = -1)]
	[ValidateModelState]
	public virtual new Task<IActionResult> UploadRawSharedDataSource([FromQuery][ModelBinder(typeof(NewtonsoftJsonModelBinder))] SaveResourceModel model) => base.UploadRawSharedDataSource(model);

	[HttpGet("datasources/exists", Order = -1)]
	[ValidateModelState]
	public virtual new Task<IActionResult> SharedDataSourceExists([FromQuery] string? uri) => base.SharedDataSourceExists(uri);

	[HttpPost("data/sql/connection/supported", Order = -1)]
	[ValidateModelState]
	public virtual new IActionResult CanConnect([ModelBinder(typeof(NewtonsoftJsonModelBinder))] ConnectionInfo input) => base.CanConnect(input);

	[HttpGet("connectionspermissions", Order = -1)]
	public virtual new IActionResult GetConnectionsPermissions() => base.GetConnectionsPermissions();

	[HttpPost("dataconnections", Order = -1)]
	[ValidateModelState]
	public virtual new IActionResult AddConnection([ModelBinder(typeof(NewtonsoftJsonModelBinder))] ConnectionInfo input) => base.AddConnection(input);

	[HttpGet("dataconnections", Order = -1)]
	public virtual new IActionResult GetDataConnections() => base.GetDataConnections();

	[HttpPost("data/sql/procedures", Order = -1)]
	[ValidateModelState]
	public virtual new IActionResult GetProceduresSchema([ModelBinder(typeof(NewtonsoftJsonModelBinder))] ConnectionInfo input) => base.GetProceduresSchema(input);

	[HttpGet("dataconnectionproviders", Order = -1)]
	public virtual new IActionResult GetDataConnectionProviders() => base.GetDataConnectionProviders();

	[HttpPost("data/sql/parameters", Order = -1)]
	[ValidateModelState]
	public virtual new IActionResult GetSqlParameters([ModelBinder(typeof(NewtonsoftJsonModelBinder))] QueryInfoWithParameters commandInfo) => base.GetSqlParameters(commandInfo);

	[HttpPost("data/sql/preview", Order = -1)]
	[ValidateModelState]
	public virtual new IActionResult GetSqlPreviewData([ModelBinder(typeof(NewtonsoftJsonModelBinder))] PreviewDataInfoWithParameters commandInfo) => base.GetSqlPreviewData(commandInfo);

	[HttpPost("data/model", Order = -1)]
	[ValidateModelState]
	public virtual new IActionResult GetDataModel([ModelBinder(typeof(NewtonsoftJsonModelBinder))] DataSourceInfo dsi) => base.GetDataModel(dsi);

	[HttpGet("reports/{reportId}", Order = -1)]
	[Obsolete("FromBaseClass")]
	public virtual new IActionResult GetReport(string? reportId) => base.GetReport(reportId);

	[HttpGet("reports/report", Order = -1)]
	public virtual new Task<IActionResult> GetReportAsync([FromQuery] string? uri) => base.GetReportAsync(uri);

	[HttpGet("reports/exists", Order = -1)]
	public virtual new Task<IActionResult> ReportExistsAsync([FromQuery] string? uri) => base.ReportExistsAsync(uri);

	[HttpPost("reports/{reportId}", Order = -1)]
	[Obsolete("FromBaseClass")]
	public virtual new Task<IActionResult> SaveReportAsync(string? reportId) => base.SaveReportAsync(reportId);

	[HttpPost("reports/save", Order = -1)]
	public virtual new Task<IActionResult> SaveReportByUriAsync([FromQuery] string? uri) => base.SaveReportByUriAsync(uri);

	[HttpGet("reports/{reportId}/file", Order = -1)]
	[Obsolete("FromBaseClass")]
	public virtual new IActionResult DownloadReport(string? reportId) => base.DownloadReport(reportId);

	[HttpGet("reports/file", Order = -1)]
	public virtual new Task<IActionResult> DownloadReportAsync([FromQuery] string? uri) => base.DownloadReportAsync(uri);

	[HttpPost("reports/file", Order = -1)]
	public virtual new Task<IActionResult> UploadReportAsync([FromQuery] string? uri) => base.UploadReportAsync(uri);

	[HttpDelete("reports/{reportId}", Order = -1)]
	[Obsolete("FromBaseClass")]
	public virtual new IActionResult DeleteReport(string? reportId) => base.DeleteReport(reportId);

	[HttpDelete("reports/delete", Order = -1)]
	public virtual new Task<IActionResult> DeleteReportAsync([FromQuery] string? uri) => base.DeleteReportAsync(uri);

	[HttpPut("reports/rename", Order = -1)]
	public virtual new Task<IActionResult> RenameReportAsync([FromBody][ModelBinder(typeof(NewtonsoftJsonModelBinder))] RenameResourceModel report) => base.RenameReportAsync(report);

	[HttpGet("reports/folders/contents", Order = -1)]
	public virtual new Task<IActionResult> GetReportsInFolder([FromQuery] string? uri) => base.GetReportsInFolder(uri);

	[HttpPost("reports/folder", Order = -1)]
	[ValidateModelState]
	public virtual new Task<IActionResult> CreateReportsFolder([FromBody][ModelBinder(typeof(NewtonsoftJsonModelBinder))] CreateFolderModel model) => base.CreateReportsFolder(model);

	[HttpGet("reports/folder/model", Order = -1)]
	public virtual new Task<IActionResult> GetReportsFolderModel([FromQuery] string? uri) => base.GetReportsFolderModel(uri);

	[HttpDelete("reports/folder/delete", Order = -1)]
	[ValidateModelState]
	public virtual new Task<IActionResult> DeleteReportsFolder([FromQuery] string? uri) => base.DeleteReportsFolder(uri);

	[HttpPut("reports/folder/rename", Order = -1)]
	[ValidateModelState]
	public virtual new Task<IActionResult> RenameReportsFolder([FromBody][ModelBinder(typeof(NewtonsoftJsonModelBinder))] RenameFolderModel model) => base.RenameReportsFolder(model);

	[HttpPut("clients/{clientID}/instances/{instanceID}/documents/{documentID}/actions/{actionID}", Order = -1)]
	public virtual new void ExecuteInteractiveAction(string? clientID, string? instanceID, string? documentID, string? actionID) => base.ExecuteInteractiveAction(clientID, instanceID, documentID, actionID);

	[HttpPost("clients", Order = -1)]
	public virtual new IActionResult RegisterClient() => base.RegisterClient();

	[HttpDelete("clients/{clientID}", Order = -1)]
	public virtual new void UnregisterClient(string? clientID) => base.UnregisterClient(clientID);

	[HttpPost("clients/keepAlive/{clientID}", Order = -1)]
	public virtual new IActionResult KeepClientAlive(string? clientID) => base.KeepClientAlive(clientID);

	[HttpGet("clients/sessionTimeout", Order = -1)]
	public virtual new IActionResult GetClientsSessionTimeoutSeconds() => base.GetClientsSessionTimeoutSeconds();

	[HttpPost("clients/{clientID}/instances/{instanceID}/documents", Order = -1)]
	public virtual new IActionResult CreateDocument(string? clientID, string? instanceID, [FromBody][ModelBinder(typeof(NewtonsoftJsonModelBinder))] CreateDocumentArgs args) => base.CreateDocument(clientID, instanceID, args);

	[HttpDelete("clients/{clientID}/instances/{instanceID}/documents/{documentID}", Order = -1)]
	public virtual new void DeleteDocument(string? clientID, string? instanceID, string? documentID) => base.DeleteDocument(clientID, instanceID, documentID);

	[HttpGet("clients/{clientID}/instances/{instanceID}/documents/{documentID}/info", Order = -1)]
	public virtual new IActionResult GetDocumentInfo(string? clientID, string? instanceID, string? documentID) => base.GetDocumentInfo(clientID, instanceID, documentID);

	[HttpGet("clients/{clientID}/instances/{instanceID}/documents/{documentID}", Order = -1)]
	[AllowAnonymous]
	public virtual new IActionResult GetDocument(string? clientID, string? instanceID, string? documentID) => base.GetDocument(clientID, instanceID, documentID);

	[HttpGet("formats", Order = -1)]
	public virtual new JsonResult GetDocumentFormats() => base.GetDocumentFormats();

	[HttpPost("clients/{clientID}/instances", Order = -1)]
	public virtual new IActionResult CreateInstance(string? clientID, [FromBody][ModelBinder(typeof(NewtonsoftJsonModelBinder))] ClientReportSource reportSource) => base.CreateInstance(clientID, reportSource);

	[HttpDelete("clients/{clientID}/instances/{instanceID}", Order = -1)]
	public virtual new void DeleteInstance(string? clientID, string? instanceID) => base.DeleteInstance(clientID, instanceID);

	[HttpPost("clients/{clientID}/pageSettings", Order = -1)]
	public virtual new IActionResult GetPageSettings(string? clientID, [FromBody][ModelBinder(typeof(NewtonsoftJsonModelBinder))] ClientReportSource reportSource) => base.GetPageSettings(clientID, reportSource);

	[HttpPost("clients/{clientID}/parameters", Order = -1)]
	public virtual new IActionResult GetParameters(string? clientID, [FromBody][ModelBinder(typeof(NewtonsoftJsonModelBinder))] ClientReportSource reportSource) => base.GetParameters(clientID, reportSource);

	[HttpGet("clients/{clientID}/instances/{instanceID}/documents/{documentID}/pages/{pageNumber}", Order = -1)]
	public virtual new IActionResult GetPage(string? clientID, string? instanceID, string? documentID, int pageNumber) => base.GetPage(clientID, instanceID, documentID, pageNumber);

	[HttpGet("clients/{clientID}/instances/{instanceID}/documents/{documentID}/resources/{resourceID}", Order = -1)]
	[AllowAnonymous]
	public virtual new Task GetResourceAsync(string? clientID, string? instanceID, string? documentID, string? resourceID) => base.GetResourceAsync(clientID, instanceID, documentID, resourceID);

	[HttpGet("resources/{folder}/{resourceName}", Order = -1)]
	[AllowAnonymous]
	public virtual new IActionResult GetResource(string? folder, string? resourceName) => base.GetResource(folder, resourceName);

	[HttpPost("clients/{clientID}/instances/{instanceID}/documents/{documentID}/search", Order = -1)]
	public virtual new IActionResult GetSearchResults(string? clientID, string? instanceID, string? documentID, [FromBody][ModelBinder(typeof(NewtonsoftJsonModelBinder))] SearchArgs args) => base.GetSearchResults(clientID, instanceID, documentID, args);

	[HttpPost("clients/{clientID}/instances/{instanceID}/documents/{documentID}/send", Order = -1)]
	public virtual new void SendDocument(string? clientID, string? instanceID, string? documentID, [FromBody][ModelBinder(typeof(NewtonsoftJsonModelBinder))] SendDocumentArgs args) => base.SendDocument(clientID, instanceID, documentID, args);

	[HttpGet("version", Order = -1)]
	public virtual new JsonResult GetVersion() => base.GetVersion();

}
