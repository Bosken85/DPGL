using Digipolis.Web.Guidelines.Error;

namespace Digipolis.Web.Guidelines
{
    public interface IDigipolisBuilder
    {
        IDigipolisBuilder EnableGlobalErrorHandling<TExceptionHandler>() where TExceptionHandler : ExceptionHandler;

    }
}