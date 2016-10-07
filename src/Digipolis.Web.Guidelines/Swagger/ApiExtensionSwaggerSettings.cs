using System.IO;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.SwaggerGen.Application;

namespace Digipolis.Web.Guidelines.Swagger
{
    public class ApiExtensionSwaggerSettings : SwaggerSettings<ResponseDefinitions>
    {
        protected override void Configuration(SwaggerGenOptions options)
        {
            options.DescribeAllEnumsAsStrings();
            options.OperationFilter<AddFileUploadParams>();
            options.DocumentFilter<EndPointPathsAndParamsToLower>();
            options.DocumentFilter<SetVersionInPaths>();
            options.SchemaFilter<PagedResultSchemaFilter>();
        }
    }
}
