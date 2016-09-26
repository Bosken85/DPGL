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
            if(handler == null) return;

            try
            {
                await this.next.Invoke(context);
                if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    await handler.HandleAsync(context, new UnauthorizedAccessException());
                }
                if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
                {
                    await handler.HandleAsync(context, new UnauthorizedAccessException());
                }
            }
            catch (Exception ex)
            {
                await handler.HandleAsync(context, ex);
            }
        }
    }
}
