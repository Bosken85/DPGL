using Digipolis.Web.Guidelines.Error;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Digipolis.Web.Guidelines
{
    public class DigipolisBuilder : IDigipolisBuilder
    {
        private readonly IServiceCollection _services;
        private readonly DigipolisOptions _options;

        public DigipolisBuilder(IServiceCollection services, DigipolisOptions options)
        {
            _services = services;
            _options = options;
        }

        public IDigipolisBuilder EnableGlobalErrorHandling<TExceptionHandler>() where TExceptionHandler : ExceptionHandler
        {
            _services.AddSingleton<IExceptionHandler, TExceptionHandler>();
            _services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(typeof(GlobalExceptionFilter));
            });
            _options.EnableGlobalErrorHandling = true;
            return this;
        }

        public IDigipolisBuilder EnableVersioning()
        {
            _services.Configure<MvcOptions>(options =>
            {
                options.UseCentralRoutePrefix(new RouteAttribute("{apiVersion}"));
            });
            _options.EnableVersioning = true;
            return this;
        }
    }
}