using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Digipolis.Web.Guidelines.Error;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Digipolis.Web.Guidelines.Mvc
{
    internal class HttpResponseMiddleware
    {
        private readonly RequestDelegate next;

        public HttpResponseMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var handler = context.RequestServices.GetService<IExceptionHandler>();
            var options = context.RequestServices.GetService<IOptions<MvcJsonOptions>>();

            try
            {
                await this.next.Invoke(context);
                if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    var error = handler?.Resolve(new UnauthorizedAccessException());
                    if (error == null) return;
                    var json = JsonConvert.SerializeObject(error, Formatting.None, options.Value.SerializerSettings);
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = error.Status;
                    await context.Response.WriteAsync(json);
                }
                if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
                {
                    var error = handler?.Resolve(new UnauthorizedAccessException());
                    if (error == null) return;
                    var json = JsonConvert.SerializeObject(error, Formatting.None, options.Value.SerializerSettings);
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = error.Status;
                    await context.Response.WriteAsync(json);
                }
            }
            catch (Exception ex)
            {
                var logger = context.RequestServices.GetService<ILoggerFactory>()?.CreateLogger("Application error");

                var error = handler?.Resolve(ex);
                if (error == null) return;
                context.Response.Clear();
                context.Response.StatusCode = error.Status;
                context.Response.ContentType = "application/problem+json";
                var json = JsonConvert.SerializeObject(error, options.Value.SerializerSettings);
                await context.Response.WriteAsync(json);
                logger?.LogError(error.Identifier.ToString(), ex);
            }
        }
    }
}
