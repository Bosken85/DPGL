using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Digipolis.Web.Guidelines.Error;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Digipolis.Web.Guidelines.Mvc
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid) return;
            else throw new ValidationException(context.ModelState.ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage)));
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ModelState.IsValid) await next();
            else throw new ValidationException(context.ModelState.ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage)));
        }
    }
}
