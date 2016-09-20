using System.IO;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.SwaggerGen.Application;

namespace Digipolis.Web.Guidelines.Swagger
{
    internal class DefaultSwaggerSettings : SwaggerSettings<ResponseGuidelines>
    {
        protected override void Configuration(SwaggerGenOptions options)
        {
            options.DescribeAllEnumsAsStrings();
            options.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, PlatformServices.Default.Application.ApplicationName + ".xml"));
            options.OperationFilter<AddFileUploadParams>();
            options.DocumentFilter<EndPointPathsAndParamsToLower>();
            options.DocumentFilter<SetVersionInPaths>();
            options.SchemaFilter<PagedResultSchemaFilter>();
        }
    }
}
