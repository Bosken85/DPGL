using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;

namespace Digipolis.Web.Guidelines.Helpers
{
    internal static class UrlHelper
    {
        private static IActionContextAccessor _httpContextAccessor;
        public static void Configure(IActionContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static string AbsoluteAction(string actionName, string controllerName, object routeValues = null)
        {
            string scheme = _httpContextAccessor.ActionContext.HttpContext.Request.Scheme;
            var helper = new Microsoft.AspNetCore.Mvc.Routing.UrlHelper(_httpContextAccessor.ActionContext);
            return helper.Action(actionName, controllerName, routeValues, scheme);
        }

        public static string AbsoluteRoute(string routeName, object routeValues = null)
        {
            var helper = new Microsoft.AspNetCore.Mvc.Routing.UrlHelper(_httpContextAccessor.ActionContext);
            return helper.Link(routeName, routeValues);
        }
    }
}
