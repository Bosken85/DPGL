using System.IO;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.SwaggerGen.Application;

namespace Digipolis.Web.Guidelines.Swagger
{
    public abstract class SwaggerSettings<TResponseGuidelines> where TResponseGuidelines : ResponseDefinitions
    {
        public void Configure(SwaggerGenOptions options)
        {
            options.OperationFilter<TResponseGuidelines>();

            var xmlPath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, PlatformServices.Default.Application.ApplicationName + ".xml");
            if (File.Exists(xmlPath)) options.IncludeXmlComments(xmlPath);

            Configuration(options);
        }

        protected abstract void Configuration(SwaggerGenOptions options);
    }
}