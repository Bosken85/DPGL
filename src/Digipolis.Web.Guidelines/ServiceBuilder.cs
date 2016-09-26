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
using Digipolis.Web.Guidelines.Error;
using Digipolis.Web.Guidelines.Mvc;
using Digipolis.Web.Guidelines.Swagger;
using Digipolis.Web.Guidelines.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.Swagger.Model;
using Swashbuckle.SwaggerGen.Generator;

namespace Digipolis.Web.Guidelines
{
    public static class ServiceBuilder
    {
        //public static IServiceCollection AddApiExtensions(this IServiceCollection services, IConfigurationSection configurationSection = null, Action<IDigipolisBuilder> builder = null)
        //{
        //    services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        //    services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
        //    services.TryAddScoped<IErrorManager, ExceptionProvider>();

        //    if (configurationSection != null && builder != null) services.Configure<DigipolisOptions>(x => { });
        //    if (configurationSection != null) services.Configure<DigipolisOptions>(configurationSection);
        //    if (builder != null) services.Configure<DigipolisOptions>(x => builder?.Invoke(new DigipolisBuilder(services, x)));

        //    return services;
        //}

        public static IMvcBuilder AddApiExtensions(this IMvcBuilder builder, IConfigurationSection config = null, Action<ApiExtensionOptions> build = null, Type exception = null)
        {
            builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            var apiOptions = new ApiExtensionOptions();

            #region Register Options

            if (config != null && build != null) builder.Services.Configure<ApiExtensionOptions>(x => { });
            if (config != null)
            {
                builder.Services.Configure<ApiExtensionOptions>(config);
                config.Bind(apiOptions);
            }
            if (build != null)
            {
                builder.Services.Configure<ApiExtensionOptions>(build);
                build(apiOptions);
            }

            #endregion

            if (apiOptions.EnableGlobalErrorHandling)
            {
                if (apiOptions.ExceptionMapper == null)
                    throw new ArgumentNullException(nameof(exception), "An exceptionhadler must be provided on AddApiExtensions when global error handling is turned on.");

                builder.Services.AddSingleton<IExceptionMapper>(apiOptions.ExceptionMapper);
                builder.Services.AddSingleton<IExceptionHandler, ExceptionHandler>();
            }

            if (apiOptions.EnableVersioning)
            {
                builder.AddMvcOptions(options =>
                {
                    options.Conventions.Insert(0, new RouteConvention(new RouteAttribute("{apiVersion}")));
                    options.Filters.Add(typeof(GlobalExceptionFilter));
                });
            }

            builder.AddMvcOptions(options =>
            {
                options.Filters.Insert(0, new ConsumesAttribute("application/json"));
                options.Filters.Insert(1, new ProducesAttribute("application/json"));
            });

            builder.AddJsonOptions(x =>
            {
                x.SerializerSettings.ContractResolver = new BaseContractResolver();
                x.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                x.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                x.SerializerSettings.Converters.Add(new TimeSpanConverter());
                x.SerializerSettings.Converters.Add(new PageResultConverter());
                x.SerializerSettings.Converters.Add(new GuidConverter());
                x.SerializerSettings.Formatting = Formatting.None;
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

        /// <summary>
        /// This configures Swagger to follow the guidelines set out by Digipolis
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        //public static IServiceCollection ConfigureSwaggerGenExtensions(this IServiceCollection services, Action<SwaggerGenOptions> setupAction = null)
        //{
        //    return ConfigureSwaggerGen<ApiExtensionSwaggerSettings>(services, setupAction);
        //}

        /// <summary>
        /// Configure Swagger completly to your need by inheriting from <see cref="SwaggerSettings{ResponseGuidelines}"/>
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureSwaggerGen<TSwaggerSettings>(this IServiceCollection services, Action<SwaggerGenOptions> setupAction = null) where TSwaggerSettings : SwaggerSettings<ResponseGuidelines>, new()
        {
            var settings = new TSwaggerSettings();
            services.Configure<SwaggerGenOptions>(settings.Configure);
            if (setupAction != null) services.ConfigureSwaggerGen(setupAction);
            return services;
        }

        public static void MultipleApiVersions<TInfo>(this SwaggerGenOptions options, IEnumerable<TInfo> apiVersions)
            where TInfo : Info
        {
            options.MultipleApiVersions(apiVersions, (api, version) =>
            {
                var versionAttribute = api.ActionDescriptor.ActionConstraints.OfType<VersionsAttribute>().FirstOrDefault();
                return versionAttribute == null || versionAttribute.AcceptedVersions.Contains(version);
            });
        }

        public static void UseApiExtensions(this IApplicationBuilder app)
        {
            var settings = app.ApplicationServices.GetService<IOptions<ApiExtensionOptions>>();
            var httpContextAccessor = app.ApplicationServices.GetService<IActionContextAccessor>();

            if (settings?.Value?.EnableGlobalErrorHandling == true) app.UseMiddleware<HttpResponseMiddleware>();
            if (httpContextAccessor != null) Helpers.UrlHelper.Configure(httpContextAccessor);
        }
    }
}
