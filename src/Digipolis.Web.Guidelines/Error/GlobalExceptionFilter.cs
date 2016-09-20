using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Digipolis.Web.Guidelines.Error
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IExceptionHandler _handler;
        private readonly ILogger _logger;

        public GlobalExceptionFilter(ILoggerFactory logger, IExceptionHandler handler)
        {
            _handler = handler;
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            this._logger = logger.CreateLogger("Application error");
        }

        public override void OnException(ExceptionContext context)
        {
            var response = _handler.Resolve(context.Exception);
            context.Result = new ObjectResult(response)
            {
                StatusCode = response.Status,
                DeclaredType = response.GetType()
            };
            //context.HttpContext.Response.ContentType = "application/problem+json charset=utf-8";
            this._logger.LogError(response.Identifier.ToString(), context.Exception);
            context.ExceptionHandled = true;
        }
    }
}
