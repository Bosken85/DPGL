using Digipolis.Web.Guidelines.Error;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Digipolis.Web.Guidelines
{
    public class DigipolisBuilder : IDigipolisBuilder
    {
        private readonly IServiceCollection _services;

        public DigipolisBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public IDigipolisBuilder EnableGlobalErrorHandling<TExceptionHandler>() where TExceptionHandler : ExceptionHandler
        {
            _services.AddSingleton<IExceptionHandler, TExceptionHandler>();
            return this;
        }
    }
}