using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Digipolis.Web.Guidelines.Api.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.Swagger.Model;
using Swashbuckle.SwaggerGen.Application;
using Swashbuckle.SwaggerGen.Generator;

namespace Digipolis.Web.Guidelines.Api.Swagger
{
    [ApiExplorerSettings(IgnoreApi=true)]
    public class SwaggerUiController : Controller
    {
        [HttpGet("swagger/ui/index.html")]
        public IActionResult Index()
        {
            return View("~/Swagger/index.cshtml", GetDiscoveryUrls());
        }

        private IDictionary<string, string> GetDiscoveryUrls()
        {
            var versions = typeof(Settings.Versions)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
                .ToDictionary(x=> x.GetRawConstantValue().ToString(), x => string.Format("/swagger/{0}/swagger.json", x.GetRawConstantValue()));

            return versions;
        }
    }
}
