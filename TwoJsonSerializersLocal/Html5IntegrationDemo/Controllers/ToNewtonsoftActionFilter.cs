using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

public class ToNewtonsoftActionFilter : IAsyncResultFilter 
{

	public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next) 
	{
		if (context.Result is JsonResult jsonResult) 
		{
			string jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(jsonResult.Value, (Newtonsoft.Json.JsonSerializerSettings?)jsonResult.SerializerSettings ?? new Newtonsoft.Json.JsonSerializerSettings {
				ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver { NamingStrategy = new Newtonsoft.Json.Serialization.CamelCaseNamingStrategy() }
			});

			context.Result = new ContentResult { Content = jsonStr, ContentType = "application/json" };
		}

		await next().ConfigureAwait(false);
	}
}
