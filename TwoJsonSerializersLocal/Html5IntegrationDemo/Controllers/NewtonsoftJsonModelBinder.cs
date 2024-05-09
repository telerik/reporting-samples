using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.IO;

public class NewtonsoftJsonModelBinder : IModelBinder 
{
	public async Task BindModelAsync(ModelBindingContext bindingContext) 
	{
		if (bindingContext == null) throw new ArgumentNullException(nameof(bindingContext));

		using var reader = new StreamReader(bindingContext.HttpContext.Request.Body);

		string body = await reader.ReadToEndAsync().ConfigureAwait(continueOnCapturedContext: false);
		object? value = Newtonsoft.Json.JsonConvert.DeserializeObject(body, bindingContext.ModelType);
		bindingContext.Result = ModelBindingResult.Success(value);
	}
}
