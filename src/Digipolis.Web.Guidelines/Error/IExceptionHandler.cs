using System;

namespace Digipolis.Web.Guidelines.Error
{
    public interface IExceptionHandler
    {
        Error Resolve(Exception exception);
    }
}