using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Telerik.Reporting.Services.AspNetCore;
using Telerik.Reporting.Services;

[ServiceFilter(typeof(ToNewtonsoftActionFilter))]
public class ReportsControllerSTJBase : ReportsControllerBase {
	public ReportsControllerSTJBase(IReportServiceConfiguration reportServiceConfiguration) : base(reportServiceConfiguration) { }
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