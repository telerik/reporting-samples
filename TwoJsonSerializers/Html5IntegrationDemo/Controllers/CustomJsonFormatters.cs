using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using Telerik.Barcode;

namespace CSharp.Net6.Html5IntegrationDemo.Controllers
{
    // the custom attribute
    internal abstract class UseJsonAttribute : Attribute, IAsyncActionFilter
    {
        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) => next();
    }
    internal class UseSystemTextJsonAttribute : UseJsonAttribute { }
    internal class UseNewtonsoftJsonAttribute : UseJsonAttribute { }

    // Our Super Input Formatter
    internal class MySuperJsonInputFormatter : TextInputFormatter
    {
        public MySuperJsonInputFormatter()
        {
            SupportedEncodings.Add(UTF8EncodingWithoutBOM);
            SupportedEncodings.Add(UTF16EncodingLittleEndian);
            SupportedMediaTypes.Add("application/json");
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            var mvcOpt = context.HttpContext.RequestServices.GetRequiredService<IOptions<MvcOptions>>().Value;
            var formatters = mvcOpt.InputFormatters;
            TextInputFormatter formatter = null; // the real formatter : SystemTextJsonInput or Newtonsoft

            Endpoint endpoint = context.HttpContext.GetEndpoint();
            if (endpoint.Metadata.GetMetadata<UseSystemTextJsonAttribute>() != null)
            {
                formatter = formatters.OfType<SystemTextJsonInputFormatter>().FirstOrDefault();
                //formatter = formatter ?? SystemTextJsonInputFormatter
            }
            else if (endpoint.Metadata.GetMetadata<UseNewtonsoftJsonAttribute>() != null)
            {
                // don't use `Of<NewtonsoftJsonInputFormatter>` here because there's a NewtonsoftJsonPatchInputFormatter
                formatter = (NewtonsoftJsonInputFormatter)(formatters
                    .Where(f => typeof(NewtonsoftJsonInputFormatter) == f.GetType())
                    .FirstOrDefault());
            }
            else
            {
                throw new Exception("This formatter is only used for System.Text.Json InputFormatter or NewtonsoftJson InputFormatter");
            }
            var result = await formatter.ReadRequestBodyAsync(context, encoding);
            return result;
        }
    }

    internal class MySuperJsonOutputFormatter : TextOutputFormatter
    {
        public MySuperJsonOutputFormatter()
        {
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
            SupportedMediaTypes.Add("application/json");
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var httpContext = context.HttpContext;
            var mvcOpt = httpContext.RequestServices.GetRequiredService<IOptions<MvcOptions>>().Value;
            var formatters = mvcOpt.OutputFormatters;
            TextOutputFormatter formatter = null;

            Endpoint endpoint = httpContext.GetEndpoint();
            if (endpoint.Metadata.GetMetadata<UseSystemTextJsonAttribute>() != null)
            {
                formatter = formatters.OfType<SystemTextJsonOutputFormatter>().FirstOrDefault();
            }
            else if (endpoint.Metadata.GetMetadata<UseNewtonsoftJsonAttribute>() != null)
            {
                // don't use `Of<NewtonsoftJsonInputFormatter>` here because there's a NewtonsoftJsonPatchInputFormatter
                formatter = (NewtonsoftJsonOutputFormatter)(formatters
                    .Where(f => typeof(NewtonsoftJsonOutputFormatter) == f.GetType())
                    .FirstOrDefault());
            }
            else
            {
                throw new Exception("This formatter is only used for System.Text.Json InputFormatter or NewtonsoftJson InputFormatter");
            }

            await formatter.WriteResponseBodyAsync(context, selectedEncoding);
        }
    }
}
