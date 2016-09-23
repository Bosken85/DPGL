using System.IO;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.SwaggerGen.Application;

namespace Digipolis.Web.Guidelines.Swagger
{
    public class ApiExtensionSwaggerSettings : SwaggerSettings<ResponseGuidelines>
    {
        protected override void Configuration(SwaggerGenOptions options)
        {
            var xmlPath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, PlatformServices.Default.Application.ApplicationName + ".xml");
            if (File.Exists(xmlPath)) options.IncludeXmlComments(xmlPath);
            options.DescribeAllEnumsAsStrings();
            options.OperationFilter<AddFileUploadParams>();
            options.DocumentFilter<EndPointPathsAndParamsToLower>();
            options.DocumentFilter<SetVersionInPaths>();
            options.SchemaFilter<PagedResultSchemaFilter>();
        }
    }
}
