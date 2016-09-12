using System.IO;
using Digipolis.Web.Guidelines.JsonConverters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.SwaggerGen.Application;
using System.Linq;
using Digipolis.Web.Guidelines.Models;
using Digipolis.Web.Guidelines.Paging;
using Digipolis.Web.Guidelines.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Swashbuckle.Swagger.Model;

namespace Digipolis.Web.Guidelines
{
    public static class ServiveBuilder
    {
        public static IServiceCollection AddDigipolis(this IServiceCollection services)
        {
            return services
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddSingleton<IActionContextAccessor, ActionContextAccessor>()
                .AddScoped<IErrorManager, ErrorManager>();
        }

        public static IMvcBuilder AddMvcDefaults(this IMvcBuilder builder)
        {
            builder.AddMvcOptions(options =>
            {
                options.Filters.Insert(0, new ConsumesAttribute("application/json"));
                options.Filters.Insert(1, new ProducesAttribute("application/json"));
            }).AddJsonOptions(x =>
            {
                x.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                x.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                x.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                x.SerializerSettings.Converters.Add(new TimeSpanConverter());
                x.SerializerSettings.Converters.Add(new PageResultConverter());
            });
            return builder;
        }

        public static IMvcBuilder AddInputFormatter(this IMvcBuilder builder, string contentType, IInputFormatter inputFormatter)
        {
            builder.AddMvcOptions(options =>
            {
                options.InputFormatters.Add(inputFormatter);
                var filter = options.Filters.FirstOrDefault(x => x is ConsumesAttribute);
                if (filter != null) options.Filters.Remove(filter);

                options.Filters.Insert(0, new ConsumesAttribute(contentType));
            });
            return builder;
        }

        public static IMvcBuilder AddOutputFormatter(this IMvcBuilder builder, string contentType, IOutputFormatter outputFormatter)
        {
            builder.AddMvcOptions(options =>
            {
                options.OutputFormatters.Add(outputFormatter);
                var filter = options.Filters.FirstOrDefault(x => x is ProducesAttribute);
                if (filter != null) options.Filters.Remove(filter);

                options.Filters.Insert(1, new ProducesAttribute(contentType));
            });
            return builder;
        }

        public static IServiceCollection ConfigureSwaggerGenDefaults(this IServiceCollection services)
        {
            return services.Configure<SwaggerGenOptions>(x =>
            {
                x.DescribeAllEnumsAsStrings();
                x.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, PlatformServices.Default.Application.ApplicationName + ".xml"));
                x.OperationFilter<DigipolisGuidelines>();
                x.DocumentFilter<EndPointPathsAndParamsToLower>();
                x.DocumentFilter<SetVersionInPaths>();
                x.MultipleApiVersions(
                    new[]
                    {
                        new Info { Version = "v1", Title = "API V1" },
                        new Info { Version = "v2", Title = "API V2" }
                    },
                    ResolveVersionSupportByVersionsConstraint
                );

                //x.SchemaFilter<>();
            });
        }

        public static IApplicationBuilder UseDigipolis(this IApplicationBuilder app)
        {
            var httpContextAccessor = app.ApplicationServices.GetService<IActionContextAccessor>();
            Helpers.UrlHelper.Configure(httpContextAccessor);
            return app;
        }

        private static bool ResolveVersionSupportByVersionsConstraint(ApiDescription apiDesc, string version)
        {
            var versionAttribute = apiDesc.ActionDescriptor.ActionConstraints.OfType<VersionsAttribute>().FirstOrDefault();
            return versionAttribute == null || versionAttribute.AcceptedVersions.Contains(version);
        }
    }
}
