using System;
using System.Collections.Generic;
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
using Digipolis.Web.Guidelines.Swagger;
using Digipolis.Web.Guidelines.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Swashbuckle.Swagger.Model;

namespace Digipolis.Web.Guidelines
{
    public static class ServiceBuilder
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
                options.UseCentralRoutePrefix(new RouteAttribute("{apiVersion}"));
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

        public static void UseCentralRoutePrefix(this MvcOptions opts, IRouteTemplateProvider routeAttribute)
        {
            opts.Conventions.Insert(0, new RouteConvention(routeAttribute));
        }

        /// <summary>
        /// This configures Swagger to follow the guidelines set out by Digipolis
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureSwaggerGenDefaults(this IServiceCollection services)
        {
            return services.Configure<SwaggerGenOptions>(x =>
            {
                x.DescribeAllEnumsAsStrings();
                x.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, PlatformServices.Default.Application.ApplicationName + ".xml"));
                x.OperationFilter<DigipolisGuidelines>();
                x.OperationFilter<AddFileUploadParams>();
                x.DocumentFilter<EndPointPathsAndParamsToLower>();
                x.DocumentFilter<SetVersionInPaths>();
                x.SchemaFilter<PagedResultSchemaFilter>();
            });
        }

        /// <summary>
        /// This configures Swagger to follow the guidelines set out by Digipolis with the exception that you can override the setting made in <see cref="DigipolisGuidelines"/>
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureSwaggerGenDefaults<TResponseGuidelines>(this IServiceCollection services) where TResponseGuidelines : DigipolisGuidelines
        {
            return services.Configure<SwaggerGenOptions>(x =>
            {
                x.DescribeAllEnumsAsStrings();
                x.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, PlatformServices.Default.Application.ApplicationName + ".xml"));
                x.OperationFilter<TResponseGuidelines>();
                x.OperationFilter<AddFileUploadParams>();
                x.DocumentFilter<EndPointPathsAndParamsToLower>();
                x.DocumentFilter<SetVersionInPaths>();
                x.SchemaFilter<PagedResultSchemaFilter>();
            });
        }

        public static void MultipleApiVersions<TInfo>(this SwaggerGenOptions options, IEnumerable<TInfo> apiVersions)
            where TInfo : Info
        {
            options.MultipleApiVersions(apiVersions, ResolveVersionSupportByVersionsConstraint);
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
