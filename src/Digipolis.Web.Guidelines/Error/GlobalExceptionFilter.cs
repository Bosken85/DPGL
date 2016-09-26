using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Digipolis.Web.Guidelines.Error
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IExceptionHandler _handler;

        public GlobalExceptionFilter(IExceptionHandler handler)
        {
            _handler = handler;
        }

        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            if(_handler == null) return;
            await _handler.HandleAsync(context.HttpContext, context.Exception);
            context.ExceptionHandled = true;
        }

        public override void OnException(ExceptionContext context)
        {
            if(_handler == null) return;
            _handler.Handle(context.HttpContext, context.Exception);
            context.ExceptionHandled = true;
        }
    }
}
