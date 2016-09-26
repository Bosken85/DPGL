using System;

namespace Digipolis.Web.Guidelines.Error
{
    public interface IExceptionMapper
    {
        Error Resolve(Exception exception);
    }
}