using System;
using System.Net;
using System.Threading.Tasks;
using Digipolis.Web.Guidelines.Error;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Digipolis.Web.Guidelines.Mvc
{
    internal class HttpResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public HttpResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var handler = context.RequestServices.GetService<IExceptionHandler>();
            if(handler == null) return;

            try
            {
                await this._next.Invoke(context);
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
