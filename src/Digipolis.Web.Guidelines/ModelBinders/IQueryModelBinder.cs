using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digipolis.Web.Guidelines.Models;
using Digipolis.Web.Guidelines.Paging;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Digipolis.Web.Guidelines.ModelBinders
{
    //public class IQueryModelBinder : IModelBinder
    //{
    //    public Task BindModelAsync(ModelBindingContext bindingContext)
    //    {
    //        if (bindingContext == null)
    //        {
    //            throw new ArgumentNullException(nameof(bindingContext));
    //        }

    //        var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
    //        if (valueProviderResult == ValueProviderResult.None)
    //        {
    //            // no entry
    //            return TaskCache.CompletedTask;
    //        }

    //        bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

    //        if (bindingContext.ModelType == typeof(Query))
    //        {
    //            var request = bindingContext.HttpContext.Request;
    //            if (request.QueryString.HasValue)
    //            {
    //                int page = 1;
    //                if(request.Query.ContainsKey("page")) int.TryParse(request.Query["page"], out page);
    //                bindingContext.Result = ModelBindingResult.Success(new Query() { Page = page });
    //            }

    //        }
    //        return TaskCache.CompletedTask;
    //    }
    //}
}
